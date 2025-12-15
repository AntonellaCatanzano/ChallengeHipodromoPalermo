using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Enums;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

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

            // Simular usuario autenticado con Claim
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "TestToken"); // Simulando un token JWT
        }

        [TestInitialize]
        public async Task TestInit()
        {
            using var scope = _factory.Services.CreateScope();
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

            context.Usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                Nombre = "UsuarioTest",
                TipoUsuarioId = 1,
                Email = "user@test.com",
                PasswordHash = "hashed" 
            });

            await context.SaveChangesAsync();
        }

        #region Crear Reservas

        [TestMethod]
        public async Task CrearReservaEstandar_ReturnsCreated()
        {
            var dto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", dto);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.CantidadPersonas, result.CantidadPersonas);
        }

        [TestMethod]
        public async Task CrearReservaVip_ReturnsCreated()
        {
            var dto = new ReservaCreateVipDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).AddHours(12).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 3,
                CodigoVip = "VIP123"
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/CrearVip", dto);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.CantidadPersonas, result.CantidadPersonas);
        }

        [TestMethod]
        public async Task CrearReservaCumple_ReturnsCreated()
        {
            var dto = new ReservaCreateCumpleDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 6,
                TraeTorta = true,
                EdadCumpleaniero = 10
            };

            var response = await _client.PostAsJsonAsync("/api/Reserva/CrearCumple", dto);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.CantidadPersonas, result.CantidadPersonas);
        }

        #endregion

        #region Acciones sobre Reserva

        [TestMethod]
        public async Task ConfirmarReserva_ReturnsOk()
        {
            var reservaDto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", reservaDto);
            var createdReserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            var response = await _client.PostAsJsonAsync($"/api/Reserva/{createdReserva.Id}/Confirmar", new { });
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.AreEqual((int)EstadoReservaEnum.Confirmada, result.EstadoReservaId);
        }

        [TestMethod]
        public async Task CancelarReserva_ReturnsOk()
        {
            var reservaDto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", reservaDto);
            var createdReserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            var response = await _client.PostAsJsonAsync($"/api/Reserva/Cancelar/{createdReserva.Id}", new { observacion = "Cancelación por motivo X" });
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.AreEqual((int)EstadoReservaEnum.Cancelada, result.EstadoReservaId);
        }

        [TestMethod]
        public async Task MarcarNoAsistio_ReturnsOk()
        {
            var reservaDto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", reservaDto);
            var createdReserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            var response = await _client.PostAsJsonAsync($"/api/Reserva/NoAsistio/{createdReserva.Id}", new { });
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.AreEqual((int)EstadoReservaEnum.NoAsistio, result.EstadoReservaId);
        }

        #endregion

        #region Consultas

        [TestMethod]
        public async Task GetDetalleReserva_ReturnsOk()
        {
            var reservaDto = new ReservaCreateStandardDTO
            {
                IdCliente = 1,
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Reserva/CrearEstandar", reservaDto);
            var createdReserva = await createResponse.Content.ReadFromJsonAsync<ReservaDTO>();

            var response = await _client.GetAsync($"/api/Reserva/GetDetalle/{createdReserva.Id}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ReservaDetailDTO>();
            Assert.IsNotNull(result);
            Assert.AreEqual(createdReserva.Id, result.Id);
        }

        [TestMethod]
        public async Task GetReservasPaginadas_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Reserva/ListadoConFiltros?pageNumber=1&pageSize=10");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<PaginatedList<ReservaListItemDTO>>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Any());
        }

        [TestMethod]
        public async Task GetAllReservas_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Reserva/GetAll");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<ReservaDTO>>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        #endregion
    }
}