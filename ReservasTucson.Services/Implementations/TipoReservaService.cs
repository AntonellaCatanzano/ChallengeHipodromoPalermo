using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class TipoReservaService : ITipoReservaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoReservaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TipoReservaDTO>> GetAll()
        {
            var tiposReservas = await _unitOfWork.TipoReservaRepository.GetAll();

            return _mapper.Map<List<TipoReservaDTO>>(tiposReservas);
        }

        public async Task<TipoReservaDTO> GetById(int id)
        {
            var tipoReserva = await _unitOfWork.TipoReservaRepository.GetById(id);

            return _mapper.Map<TipoReservaDTO>(tipoReserva);
        }

        public async Task<TipoReservaDTO> InsertTipoReserva(TipoReservaDTO entity)
        {
            var model = _mapper.Map<TipoReserva>(entity);

            var tipoReserva = await _unitOfWork.TipoReservaRepository.InsertTipoReserva(model);

            var resultado = _mapper.Map<TipoReservaDTO>(tipoReserva);

            await _unitOfWork.SaveChanges();

            return resultado;
        }
    }
}
