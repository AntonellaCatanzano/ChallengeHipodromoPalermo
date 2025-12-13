
using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class EstadoReservaService : IEstadoReservaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EstadoReservaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EstadoReservaDTO>> GetAll()
        {
            var estadosReservas = await _unitOfWork.EstadoReservaRepository.GetAll();
            
            if (estadosReservas == null || !estadosReservas.Any())
                return new List<EstadoReservaDTO>();

            return _mapper.Map<List<EstadoReservaDTO>>(estadosReservas);
        }

        public async Task<EstadoReservaDTO> GetById(int id)
        {
            var estadoReserva = await _unitOfWork.EstadoReservaRepository.GetById(id);

            if (estadoReserva == null)
                return null;

            return _mapper.Map<EstadoReservaDTO>(estadoReserva);
        }


        public async Task<EstadoReservaDTO> InsertEstadoReserva(EstadoReservaDTO entity)
        {
            var model = _mapper.Map<EstadoReserva>(entity);

            var estadoReserva = await _unitOfWork.EstadoReservaRepository.InsertEstadoReserva(model);

            var resultado = _mapper.Map<EstadoReservaDTO>(estadoReserva);

            await _unitOfWork.SaveChanges();

            return resultado;
        }
    }
}
