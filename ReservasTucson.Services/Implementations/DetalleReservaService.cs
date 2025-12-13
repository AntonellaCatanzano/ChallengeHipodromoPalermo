using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class DetalleReservaService : IDetalleReservaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetalleReservaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<DetalleReservaDTO>> GetAll()
        {
            var detallesReservas = await _unitOfWork.DetalleReservaRepository.GetAll();

            return _mapper.Map<List<DetalleReservaDTO>>(detallesReservas);
        }

        public async Task<DetalleReservaDTO> GetById(int id)
        {
            var detalleReserva = await _unitOfWork.DetalleReservaRepository.GetById(id);

            return _mapper.Map<DetalleReservaDTO>(detalleReserva);
        }

        public async Task<DetalleReservaDTO> GetByReservaId(int reservaId)
        {
            var reserva = await _unitOfWork.DetalleReservaRepository.GetByReservaId(reservaId);

            return _mapper.Map<DetalleReservaDTO>(reserva);
        }
    }
}
