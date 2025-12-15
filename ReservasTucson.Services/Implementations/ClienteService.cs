using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;

namespace ReservasTucson.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ClienteDTO>> GetAll()
        {
            var clientes = await _unitOfWork.ClienteRepository.GetAll();

            return _mapper.Map<List<ClienteDTO>>(clientes);
        }        

        public async Task<ClienteDTO> GetById(int id)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetById(id);

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> GetByEmail(string email)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetByEmailAsync(email);

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> GetByEmailOrCuitAsync(string email, string cuit)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetByEmailOrCuitAsync(email, cuit);

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> InsertCliente(ClienteDTO entity)
        {
            var model = _mapper.Map<Cliente>(entity);

            var cliente = await _unitOfWork.ClienteRepository.InsertCliente(model);

            var resultado = _mapper.Map<ClienteDTO>(cliente);

            await _unitOfWork.SaveChanges();

            return resultado;
        }
    }
}
