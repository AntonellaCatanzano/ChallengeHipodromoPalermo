
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> GetAll()
        {
            var usuarios = await _unitOfWork.UsuarioRepository.GetAll();

            return _mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        public async Task<UsuarioDTO> GetById(int id)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetById(id);

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO> InsertUsuario(UsuarioDTO entity)
        {
            var tipoUsuario = await _unitOfWork.TipoUsuarioRepository.GetById(entity.TipoUsuarioId);

            if (tipoUsuario == null)
            {
                throw new Exception("El Tipo de Usuario especificado no existe.");
            }

            var model = _mapper.Map<Usuario>(entity);            

            var usuario = await _unitOfWork.UsuarioRepository.InsertUsuarioAsync(model);

            await _unitOfWork.SaveChanges();

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<List<UsuarioDTO>> GetByTipoUsuario(int tipoUsuarioId)
        {
            var tipoUsuario = await _unitOfWork.UsuarioRepository.GetByTipoUsuario(tipoUsuarioId);

            return _mapper.Map<List<UsuarioDTO>>(tipoUsuario);
        }

        public async Task<UsuarioDTO?> GetByEmail(string email)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetByEmail(email);

            return _mapper.Map<UsuarioDTO>(usuario);

        }
    }
}
