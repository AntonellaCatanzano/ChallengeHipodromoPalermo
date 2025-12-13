using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class EventoService : IEventoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EventoDTO>> GetAll()
        {
            var eventos = await _unitOfWork.EventoRepository.GetAll();

            return _mapper.Map<List<EventoDTO>>(eventos);
        }

        public async Task<EventoDTO> GetById(int id)
        {
            var evento = await _unitOfWork.EventoRepository.GetById(id);

            return _mapper.Map<EventoDTO>(evento);
        }

        public async Task<EventoDTO> InsertEvento(EventoDTO entity)
        {
            var model = _mapper.Map<Evento>(entity);

            var evento = await _unitOfWork.EventoRepository.InsertEvento(model);

            var resultado = _mapper.Map<EventoDTO>(evento);

            await _unitOfWork.SaveChanges();

            return resultado;
        }
    }
}
