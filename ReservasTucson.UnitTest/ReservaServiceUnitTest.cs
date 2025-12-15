using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Enums;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Implementations;
using ReservasTucson.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservasTucson.Tests.Services
{
    [TestClass]
    public class ReservaServiceUnitTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private IReservaService _reservaService;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            // Repositorios del UnitOfWork
            var mockReservaRepo = new Mock<IReservaRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();
            var mockUsuarioRepo = new Mock<IUsuarioRepository>();
            var mockMesaRepo = new Mock<IMesaRepository>();
            var mockReservaMesaRepo = new Mock<IReservaMesaRepository>();

            _mockUnitOfWork.Setup(u => u.ReservaRepository).Returns(mockReservaRepo.Object);
            _mockUnitOfWork.Setup(u => u.ClienteRepository).Returns(mockClienteRepo.Object);
            _mockUnitOfWork.Setup(u => u.UsuarioRepository).Returns(mockUsuarioRepo.Object);
            _mockUnitOfWork.Setup(u => u.MesaRepository).Returns(mockMesaRepo.Object);
            _mockUnitOfWork.Setup(u => u.ReservaMesaRepository).Returns(mockReservaMesaRepo.Object);

            // Mapper
            _mockMapper.Setup(m => m.Map<ReservaDTO>(It.IsAny<Reserva>()))
                .Returns((Reserva r) => new ReservaDTO { Id = r.Id });

            _mockMapper.Setup(m => m.Map<ReservaDetailDTO>(It.IsAny<Reserva>()))
                .Returns((Reserva r) => new ReservaDetailDTO { Id = r.Id });

            _mockMapper.Setup(m => m.Map<DetalleReservaDTO>(It.IsAny<DetalleReserva>()))
                .Returns((DetalleReserva d) => new DetalleReservaDTO
                {
                    Id = d.Id,
                    TraeTorta = d.TraeTorta,
                    EdadCumpleaniero = d.EdadCumpleaniero,
                    Decoracion = d.Decoracion,
                    ComentariosDecoracion = d.ComentariosDecoracion,
                    PaqueteContratado = d.PaqueteContratado
                });

            _mockMapper.Setup(m => m.Map<ReservaMesaDTO>(It.IsAny<ReservaMesa>()))
                .Returns((ReservaMesa rm) => new ReservaMesaDTO { ReservaId = rm.ReservaId, MesaId = rm.MesaId });

            _reservaService = new ReservaService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region CrearReservaEstandarAsync

        [TestMethod]
        public async Task CrearReservaEstandarAsync_Valida_CreaReserva()
        {
            var dto = new ReservaCreateStandardDTO
            {
                FechaHora = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2,
                Nombre = "Juan",
                Apellido = "Perez",
                Email = "juan@test.com"
            };

            _mockUnitOfWork.Setup(u => u.UsuarioRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Usuario { Id = 1 });

            _mockUnitOfWork.Setup(u => u.ClienteRepository.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Cliente?)null);

            _mockUnitOfWork.Setup(u => u.ClienteRepository.InsertCliente(It.IsAny<Cliente>()))
                .ReturnsAsync((Cliente c) => { c.Id = 1; return c; });

            _mockUnitOfWork.Setup(u => u.ReservaRepository.AddAsync(It.IsAny<Reserva>()))
                .ReturnsAsync((Reserva r) => { r.Id = 10; return r; });

            var result = await _reservaService.CrearReservaEstandarAsync(dto, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task CrearReservaEstandarAsync_FechaPasada_LanzaException()
        {
            var dto = new ReservaCreateStandardDTO
            {
                FechaHora = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2,
                Nombre = "Juan",
                Apellido = "Perez",
                Email = "juan@test.com"
            };

            _mockUnitOfWork.Setup(u => u.UsuarioRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Usuario { Id = 1 });

            await _reservaService.CrearReservaEstandarAsync(dto, 1);
        }

        #endregion

        #region CrearReservaVipAsync

        [TestMethod]
        public async Task CrearReservaVipAsync_Valida_CreaReservaConMesa()
        {
            var dto = new ReservaCreateVipDTO
            {
                FechaHora = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 2,
                CodigoVip = "VIP1234",
                Nombre = "Ana",
                Apellido = "Lopez",
                Email = "ana@test.com"
            };

            _mockUnitOfWork.Setup(u => u.UsuarioRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Usuario { Id = 2 });

            _mockUnitOfWork.Setup(u => u.ClienteRepository.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Cliente?)null);

            _mockUnitOfWork.Setup(u => u.ClienteRepository.InsertCliente(It.IsAny<Cliente>()))
                .ReturnsAsync((Cliente c) => { c.Id = 2; return c; });

            _mockUnitOfWork.Setup(u => u.MesaRepository.GetDisponiblesAsync(It.IsAny<DateTime>(), It.IsAny<int>(), true))
                .ReturnsAsync(new List<Mesa> { new Mesa { Id = 5, Capacidad = 4 } });

            _mockUnitOfWork.Setup(u => u.ReservaRepository.AddAsync(It.IsAny<Reserva>()))
                .ReturnsAsync((Reserva r) => { r.Id = 20; return r; });

            _mockUnitOfWork.Setup(u => u.ReservaMesaRepository.AddAsync(It.IsAny<ReservaMesa>()))
                .ReturnsAsync((ReservaMesa rm) => rm);

            _mockUnitOfWork.Setup(u => u.SaveChanges()).Returns(Task.CompletedTask);

            var result = await _reservaService.CrearReservaVipAsync(dto, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.Id);
        }

        #endregion

        #region CrearReservaCumpleAsync

        [TestMethod]
        public async Task CrearReservaCumpleAsync_Valida_CreaReserva()
        {
            var dto = new ReservaCreateCumpleDTO
            {
                FechaHora = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm"),
                CantidadPersonas = 6,
                Nombre = "Lucas",
                Apellido = "Gomez",
                Email = "lucas@test.com",
                TraeTorta = false,
                EdadCumpleaniero = 10
            };

            _mockUnitOfWork.Setup(u => u.UsuarioRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Usuario { Id = 3 });

            _mockUnitOfWork.Setup(u => u.ClienteRepository.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Cliente?)null);

            _mockUnitOfWork.Setup(u => u.ClienteRepository.InsertCliente(It.IsAny<Cliente>()))
                .ReturnsAsync((Cliente c) => { c.Id = 3; return c; });

            _mockUnitOfWork.Setup(u => u.ReservaRepository.AddAsync(It.IsAny<Reserva>()))
                .ReturnsAsync((Reserva r) => { r.Id = 30; return r; });

            var result = await _reservaService.CrearReservaCumpleAsync(dto, 3);

            Assert.IsNotNull(result);
            Assert.AreEqual(30, result.Id);
        }

        #endregion

        #region GetDetalleReservaAsync

        [TestMethod]
        public async Task GetDetalleReservaAsync_RetornaDetalle()
        {
            var reserva = new Reserva
            {
                Id = 1,
                DetalleReserva = new DetalleReserva
                {
                    Id = 5,
                    TraeTorta = true,
                    EdadCumpleaniero = 8
                },
                ReservasMesas = new List<ReservaMesa>
                {
                    new ReservaMesa { MesaId = 2, ReservaId = 1 }
                }
            };

            _mockUnitOfWork.Setup(u => u.ReservaRepository.GetByIdAsync(1))
                .ReturnsAsync(reserva);

            var result = await _reservaService.GetDetalleReservaAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DetalleReserva.Id);
            Assert.AreEqual(1, result.ReservasMesas.First().ReservaId);
        }

        #endregion
    }
}