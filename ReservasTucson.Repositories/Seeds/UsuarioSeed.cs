using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Enums;
using ReservasTucson.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Seeds
{
    public static class UsuarioSeed
    {
        public static async Task SeedAsync(IUsuarioRepository usuarioRepository)
        {
            if (await usuarioRepository.AnyAsync())
                return;

            var usuario = new Usuario
            {
                Nombre = "Recepción Tucson",
                Email = "recepcion@tucson.com",
                PasswordHash = ComputeSha256Hash("recepcionTucson@2025"),
                TipoUsuarioId = (int)TipoUsuarioEnum.Recepcionista
            };

            await usuarioRepository.InsertUsuarioAsync(usuario);
        }

        private static string ComputeSha256Hash(string pass)
        {
            using var sha = SHA256.Create();

            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));

            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
