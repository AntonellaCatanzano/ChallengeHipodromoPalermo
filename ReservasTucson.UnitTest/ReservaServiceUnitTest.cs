using AutoMapper;
using Moq;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Support.Helpers;
using ReservasTucson.Repositories.Interfaces;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Implementations;


namespace ReservasTucson.Tests.Services
{
    [TestClass]
    public class ReservaServiceUnitTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IReservaRepository> _mockReservaRepository;
        private Mock<IMapper> _mockMapper;
        private IReservaService _reservaService;

        [TestInitialize]
        public void Setup()
        {
            _mockReservaRepository = new Mock<IReservaRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            // Configuramos el unit of work para devolver el repo mockeado
            _mockUnitOfWork
                .Setup(u => u.ReservaRepository)
                .Returns(_mockReservaRepository.Object);

            // Configuramos el mapper genérico
            _mockMapper
                .Setup(m => m.Map<ReservaDTO>(It.IsAny<Reserva>()))
                .Returns((Reserva r) => new ReservaDTO { Id = r.Id });

            _mockMapper
                .Setup(m => m.Map<ReservaDetailDTO>(It.IsAny<Reserva>()))
                .Returns((Reserva r) => new ReservaDetailDTO { Id = r.Id });

            _mockMapper
                .Setup(m => m.Map<List<int>>(It.IsAny<List<ReservaMesa>>()))
                .Returns((List<ReservaMesa> list) => list.ConvertAll(rm => rm.MesaId));

            
            _reservaService = new ReservaService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region Crear Reservas

        [TestMethod]
        public async Task CrearReservaEstandar_GuardarReserva()
        {
            var dto = new ReservaCreateStandardDTO
            {
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),
                CantidadPersonas = 2,
                Nombre = "Juan",
                Apellido = "Perez",
                Email = "juan@example.com",
                Telefono = "123456789"
            };

            _mockReservaRepository
                .Setup(r => r.CrearReservaEstandarAsync(It.IsAny<Reserva>()))
                .ReturnsAsync((Reserva r) => { r.Id = 1; return r; });

            var result = await _reservaService.CrearReservaEstandarAsync(dto, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task CrearReservaEstandar_FechaPasada_LanzarException()
        {
            var dto = new ReservaCreateStandardDTO
            {
                FechaHora = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"),
                CantidadPersonas = 2,
                Nombre = "Juan",
                Apellido = "Perez",
                Email = "juan@example.com",
                Telefono = "123456789"
            };

            await _reservaService.CrearReservaEstandarAsync(dto, 1);
        }

        [TestMethod]
        public async Task CrearReservaVip_GuardarReserva()
        {
            var dto = new ReservaCreateVipDTO
            {
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),
                CantidadPersonas = 3,
                Nombre = "Ana",
                Apellido = "Gomez",
                Email = "ana@example.com",
                Telefono = "987654321",
                CodigoVip = "VIP123"
            };

            _mockReservaRepository
                .Setup(r => r.CrearReservaVipAsync(It.IsAny<Reserva>()))
                .ReturnsAsync((Reserva r) => { r.Id = 2; return r; });

            var result = await _reservaService.CrearReservaVipAsync(dto, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
        }

        [TestMethod]
        public async Task CrearReservaCumple_GuardarReserva()
        {
            var dto = new ReservaCreateCumpleDTO
            {
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),
                CantidadPersonas = 6,
                Nombre = "Carlos",
                Apellido = "Lopez",
                Email = "carlos@example.com",
                Telefono = "555555555",
                TraeTorta = true,
                EdadCumpleaniero = 10
            };

            _mockReservaRepository
                .Setup(r => r.CrearReservaCumpleAsync(It.IsAny<Reserva>()))
                .ReturnsAsync((Reserva r) => { r.Id = 3; return r; });

            var result = await _reservaService.CrearReservaCumpleAsync(dto, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
        }

        #endregion

        #region Acciones sobre Reserva

        [TestMethod]
        public async Task ConfirmarReserva_CambiarEstado()
        {
            var reserva = new Reserva { Id = 4, EstadoReservaId = 1 };

            _mockReservaRepository
                .Setup(r => r.ConfirmarReservaAsync(4))
                .ReturnsAsync(reserva);

            var result = await _reservaService.ConfirmarReservaAsync(4);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Id);
        }

        [TestMethod]
        public async Task CancelarReserva_CambiarEstadoYGuardarObservacion()
        {
            var reserva = new Reserva { Id = 5, EstadoReservaId = 1 };

            _mockReservaRepository
                .Setup(r => r.CancelarReservaAsync(5, "Cliente solicitó cancelar"))
                .ReturnsAsync(reserva);

            var result = await _reservaService.CancelarReservaAsync(5, "Cliente solicitó cancelar");

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Id);
        }

        [TestMethod]
        public async Task MarcarNoAsistio_CambiarEstado()
        {
            var reserva = new Reserva { Id = 6, EstadoReservaId = 2 };

            _mockReservaRepository
                .Setup(r => r.MarcarNoAsistioAsync(6))
                .ReturnsAsync(reserva);

            var result = await _reservaService.MarcarNoAsistioAsync(6);

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Id);
        }

        #endregion

        #region Asignar Mesas

        [TestMethod]
        public async Task AsignarMesas_RetornarListaDeIds()
        {
            var dto = new AsignarMesasRequestDTO
            {
                ReservaId = 7,
                MesaIds = new List<int> { 1, 2 }
            };

            var mesasAsignadas = new List<ReservaMesa>
            {
                new ReservaMesa { ReservaId = 7, MesaId = 1 },
                new ReservaMesa { ReservaId = 7, MesaId = 2 }
            };

            _mockReservaRepository
                .Setup(r => r.AsignarMesasAsync(dto.ReservaId, dto.MesaIds))
                .ReturnsAsync(mesasAsignadas);

            var result = await _reservaService.AsignarMesasAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(new List<int> { 1, 2 }, result);
        }

        #endregion

        #region Consultas

        [TestMethod]
        public async Task GetById_RetornarReserva()
        {
            var reserva = new Reserva { Id = 8 };

            _mockReservaRepository
                .Setup(r => r.GetByIdAsync(8))
                .ReturnsAsync(reserva);

            var result = await _reservaService.GetByIdAsync(8);

            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Id);
        }

        [TestMethod]
        public async Task GetAll_RetornarLista()
        {
            var lista = new List<Reserva> { new Reserva { Id = 9 } };

            _mockReservaRepository
                .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(lista);

            var result = await _reservaService.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(9, result[0].Id);
        }

        #endregion
    }
}
