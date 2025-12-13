using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces;


namespace ReservasTucson.Repositories.Implementations
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public ClienteRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Cliente>> GetAll()
        {
            var clientes = await _dbContext.Clientes.ToListAsync();

            return clientes;
        }

        public async Task<Cliente> GetById(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);

            return cliente;
        }

        public async Task<Cliente> InsertCliente(Cliente entity)
        {
            await _dbContext.Clientes.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Cliente> GetByEmailAsync(string email)
        {
            return await _dbContext.Clientes
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }
    }
}
