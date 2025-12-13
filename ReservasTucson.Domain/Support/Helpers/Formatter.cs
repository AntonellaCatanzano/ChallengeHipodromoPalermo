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

        public static string EncryptWord = "Evalia";
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

        public static string GenerateVerificationCode()
        {
            Random rnd = new Random();
            int val = rnd.Next(100000, 999999);
            return val.ToString();
        }


        /// <summary>
        /// Metodo de validación de legajo con digito verificador
        /// dentro de la cadena
        /// </summary>
        /// <param name="rut">string</param>
        /// <returns>booleano</returns>
        public static bool ValidarLegajo(string legajo)
        {
            legajo = legajo.Replace(".", "").ToUpper();

            Regex expresion = new Regex("^([0-9]+-[0-9K])$");

            string dv = legajo.Substring(legajo.Length - 1, 1);

            if (!expresion.IsMatch(legajo))
            {
                return false;
            }
            char[] charCorte = { '-' };
            string[] legajoTemp = legajo.Split(charCorte);
            if (dv != Digito(int.Parse(legajoTemp[0])))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Método que valida el rut con el digito verificador
        /// por separado
        /// </summary>
        /// <param name="legajo">integer</param>
        /// <param name="dv">char</param>
        /// <returns>booleano</returns>
        public static bool ValidarLegajo(string legajo, string dv)
        {
            return ValidarLegajo(legajo + "-" + dv);
        }


        /// <summary>
        /// método que calcula el digito verificador a partir
        /// de la mantisa del legajo
        /// </summary>
        /// <param name="legajo"></param>
        /// <returns></returns>

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

        public static string Digito(int legajo)
        {
            int suma = 0;
            int multiplicador = 1;

            while (legajo != 0)
            {
                multiplicador++;
                if (multiplicador == 8) multiplicador = 2;

                suma += (legajo % 10) * multiplicador;
                legajo = legajo / 10;
            }

            suma = 11 - (suma % 11);

            if (suma == 11)
            {
                return "0";
            }
            else if (suma == 10)
            {
                return "K";
            }
            else
            {
                return suma.ToString();
            }
        }
        #endregion
    }
}
