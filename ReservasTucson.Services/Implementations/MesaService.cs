using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class MesaService : IMesaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MesaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MesaDTO>> GetAll()
        {
            var mesas = await _unitOfWork.MesaRepository.GetAll();

            return _mapper.Map<List<MesaDTO>>(mesas);
        }

        public async Task<MesaDTO> GetById(int id)
        {
            var mesa = await _unitOfWork.MesaRepository.GetById(id);

            return _mapper.Map<MesaDTO>(mesa);
        }

        public async Task<MesaDTO> InsertMesa(MesaDTO entity)
        {
            var model = _mapper.Map<Mesa>(entity);

            var mesa = await _unitOfWork.MesaRepository.InsertMesa(model);

            var resultado = _mapper.Map<MesaDTO>(mesa);

            await _unitOfWork.SaveChanges();

            return resultado;
        }
    }
}
