using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoUsuarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TipoUsuarioDTO>> GetAll()
        {
            var tiposUsuario = await _unitOfWork.TipoUsuarioRepository.GetAll();

            return _mapper.Map<List<TipoUsuarioDTO>>(tiposUsuario);
        }

        public async Task<TipoUsuarioDTO> GetById(int id)
        {
            var tipoUsuario = await _unitOfWork.TipoUsuarioRepository.GetById(id);

            return _mapper.Map<TipoUsuarioDTO>(tipoUsuario);
        }

        public async Task<TipoUsuarioDTO> InsertTipoUsuario(TipoUsuarioDTO entity)
        {
            var model = _mapper.Map<TipoUsuario>(entity);

            var tipoUsuario = await _unitOfWork.TipoUsuarioRepository.InsertTipoUsuario(model);

            var resultado = _mapper.Map<TipoUsuarioDTO>(tipoUsuario);

            await _unitOfWork.SaveChanges();

            return resultado;
        }
    }
}
