using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;


namespace ReservasTucson.Services.Implementations
{
    public class ReservaMesaService : IReservaMesaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservaMesaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ReservaMesaDTO>> GetAll()
        {
            var reservasMesas = await _unitOfWork.ReservaMesaRepository.GetAll();

            return _mapper.Map<List<ReservaMesaDTO>>(reservasMesas);
        }

        public async Task<ReservaMesaDTO> GetById(int id)
        {
            var reservaMesa = await _unitOfWork.ReservaMesaRepository.GetById(id);

            return _mapper.Map<ReservaMesaDTO>(reservaMesa);
        }

        public async Task<ReservaMesaDTO> GetByReservaId(int reservaId)
        {
            var reserva = await _unitOfWork.ReservaMesaRepository.GetByReservaId(reservaId);

            return _mapper.Map<ReservaMesaDTO>(reserva);
        }

        public async Task<List<ReservaMesaDTO>> GetMesasByReservaId(int reservaId)
        {
            var mesasReserva = await _unitOfWork.ReservaMesaRepository.GetMesasByReservaId(reservaId);

            return _mapper.Map<List<ReservaMesaDTO>>(mesasReserva);
        }
    }
}
