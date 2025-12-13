
namespace ReservasTucson.Domain.Support.Helpers
{
    public class BusinessException : BackendException
    {
        public BusinessException(string message)
            : base(
                  statusCode: 422, // Código HTTP sugerido para errores de negocio
                  title: "Business Error",
                  errors: new List<string> { message },
                  error: message)
        {
        }

        public BusinessException(List<string> errors, string message = "Business Error")
            : base(
                  statusCode: 422,
                  title: "Business Error",
                  errors: errors,
                  error: message)
        {
        }
    }
}
