using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using System.Net.Http.Json;


namespace ReservasTucson.IntegrationTests
{
    [TestClass]
    public class ReservaControllerIntegrationTest
    {
        private static WebApplicationFactory<Program> _factory;
        private static HttpClient _client;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Reemplazar DbContext real por InMemory
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<ReservasTucsonDBContext>));
                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<ReservasTucsonDBContext>(options =>
                        {
                            options.UseInMemoryDatabase("ReservasTestDb");
                        });
                    });
                });

            _client = _factory.CreateClient();
        }

        [TestInitialize]
        public async Task TestInit()
        {
            
            var scope = _factory.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ReservasTucsonDBContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            context.Clientes.Add(new Cliente
            {
                Id = 1,
                Nombre = "Test",
                Apellido = "Cliente",
                Email = "test@cliente.com",
                Telefono = "12345678",
                EsPersonaFisica = true
            });

            await context.SaveChangesAsync();
        }

        #region Crear Reservas

        [TestMethod]
        public async Task CrearReservaEstandar_ReturnsOk()
        {
            var dto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"),
                CantidadPersonas = 2
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", dto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.CantidadPersonas, result.CantidadPersonas);
        }

        [TestMethod]
        public async Task CrearReservaVip_ReturnsOk()
        {
            var dto = new ReservaCreateVipDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).AddHours(12).ToString("dd/MM/yyyy HH:mm:ss"),
                CantidadPersonas = 3,
                CodigoVip = "VIP123"
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/CrearVip", dto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.CantidadPersonas, result.CantidadPersonas);
            Assert.AreEqual((int)ReservasTucson.Domain.Enums.TipoReservaEnum.Vip, result.TipoReservaId);
        }

        [TestMethod]
        public async Task CrearReservaCumple_ReturnsOk()
        {
            var dto = new ReservaCreateCumpleDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy HH:mm:ss"),
                CantidadPersonas = 6,
                TraeTorta = true,
                EdadCumpleaniero = 10
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/CrearCumple", dto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.CantidadPersonas, result.CantidadPersonas);
            Assert.AreEqual((int)ReservasTucson.Domain.Enums.TipoReservaEnum.Cumpleanios, result.TipoReservaId);
        }

        #endregion

        #region Acciones sobre Reserva

        [TestMethod]
        public async Task ConfirmarCancelarMarcarNoAsistio_Flows()
        {
            // Crear reserva estándar primero
            var createDto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"),
                CantidadPersonas = 2
            };
            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", createDto);
            var reserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            // Confirmar reserva
            var confirmResponse = await _client.PostAsync($"/api/Reserva/Confirmar/{reserva.Id}", null);
            confirmResponse.EnsureSuccessStatusCode();
            var confirmResult = await confirmResponse.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.AreEqual((int)ReservasTucson.Domain.Enums.EstadoReservaEnum.Confirmada, confirmResult.EstadoReservaId);

            // Cancelar reserva
            var cancelResponse = await _client.PostAsJsonAsync($"/api/Reserva/Cancelar/{reserva.Id}", "Cancelación de prueba");
            cancelResponse.EnsureSuccessStatusCode();
            var cancelResult = await cancelResponse.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.AreEqual((int)ReservasTucson.Domain.Enums.EstadoReservaEnum.Cancelada, cancelResult.EstadoReservaId);

            // Marcar no asistió (creamos otra reserva)
            createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", createDto);
            reserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            var noAsistioResponse = await _client.PostAsync($"/api/Reserva/NoAsistio/{reserva.Id}", null);
            noAsistioResponse.EnsureSuccessStatusCode();
            var noAsistioResult = await noAsistioResponse.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.AreEqual((int)ReservasTucson.Domain.Enums.EstadoReservaEnum.NoAsistio, noAsistioResult.EstadoReservaId);
        }

        #endregion

        #region Consultas

        [TestMethod]
        public async Task GetById_GetAll_ReturnsOk()
        {
            
            var dto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"),
                CantidadPersonas = 2
            };
            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", dto);
            var reserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            
            var getResponse = await _client.GetAsync($"/api/Reserva/GetById/{reserva.Id}");
            getResponse.EnsureSuccessStatusCode();
            var getResult = await getResponse.Content.ReadFromJsonAsync<ReservaDetailDTO>();
            Assert.AreEqual(reserva.Id, getResult.Id);

            
            var getAllResponse = await _client.GetAsync("/api/Reserva/GetAll");
            getAllResponse.EnsureSuccessStatusCode();
            var list = await getAllResponse.Content.ReadFromJsonAsync<List<ReservaListItemDTO>>();
            Assert.IsTrue(list.Count > 0);
        }

        #endregion

        #region Asignar Mesas

        [TestMethod]
        public async Task AsignarMesas_ReturnsOk()
        {
            
            var dto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"),
                CantidadPersonas = 2
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", dto);
            var reserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            
            var asignarDto = new AsignarMesasRequestDTO
            {
                ReservaId = reserva.Id,
                MesaIds = new List<int> { 1, 2 }
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/AsignarMesas", asignarDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<int>>();

            CollectionAssert.AreEqual(asignarDto.MesaIds, result);
        }

        #endregion
    }
}
