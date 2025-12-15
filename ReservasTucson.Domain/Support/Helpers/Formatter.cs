using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace ReservasTucson.Domain.Support.Helpers
{
    public static class Formatter
    {
        public static string CompletePadLeft(int quantity, string value = "", bool reverse = false)
        {
            int min = 0;
            int max = quantity;
            if (reverse && value.Length > quantity)
            {
                min = value.Length - quantity;
            }

            return value?.PadLeft(quantity, '0').Substring(min, max);
        }

        /// <summary>
        /// Cheque si la lista es nula o esta vacia
        /// </summary>
        /// <typeparam name="T">Objeto tipado de la lista</typeparam>
        /// <param name="list">Lista de objetos a revisar</param>
        /// <returns></returns>
        public static bool ListNotNullOrEmpty<T>(
            this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        public static bool IntNotNullOrEmpty(
            this int? value)
        {
            return value.HasValue && value.Value > 0;
        }

        #region Encriptacion

        public static string EncryptWord = "Reservas Tucson";
        /// <summary>
        /// Encripta el valor a base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64Encode(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Desencripta el valor de base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64Decode(string value)
        {
            var base64EncodedBytes = Convert.FromBase64String(value);
            var plainText = Encoding.UTF8.GetString(base64EncodedBytes);
            return plainText;
        }
        public static string GetSHA256(string value)
        {
            // Get the bytes of the string
            var valueBytes = Encoding.UTF8.GetBytes(value);

            byte[] stream = SHA256.Create().ComputeHash(valueBytes);

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < stream.Length; i++)
            {
                builder.Append(stream[i].ToString("x2"));
            }
            return builder.ToString();
        }

        #endregion

        #region Business logic
        public static string? ForNullDateToString(this DateTime? date)
        {
            string? dateString = null;
            if (date.HasValue)
            {
                dateString = date.Value.ToString(Constants.DateFormat);
            }

            return dateString;
        }       


        

        public static bool ValidarPassword(string password)
        {
            // Verificar la longitud de la contraseña
            if (password.Length < 8)
            {
                return false;
            }

            // Verificar la presencia de al menos una letra mayúscula
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return false;
            }

            // Verificar la presencia de al menos una letra minúscula
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return false;
            }

            // Verificar la presencia de al menos un número
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }

            // Verificar la presencia de al menos un caracter especial
            if (!Regex.IsMatch(password, "[!@#$%^&*]"))
            {
                return false;
            }

            return true;
        }

       
        #endregion
    }
}
