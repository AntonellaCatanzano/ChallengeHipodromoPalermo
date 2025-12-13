using ReservasTucson.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> InsertCliente(Cliente entity);
        Task<List<Cliente>> GetAll();
        Task<Cliente> GetById(int id);

        Task<Cliente> GetByEmailAsync(string email);
    }
}
