using System;
using System.Collections.Generic;


namespace ReservasTucson.Domain.Support.Helpers
{
    public class NotFoundException : BackendException
    {
        public NotFoundException(string message)
            : base(
                  statusCode: 404,
                  title: "El recurso no fue encontrado",
                  errors: new List<string> { message },
                  error: message)
        {
        }

        public NotFoundException(List<string> errors, string message = "Recurso no encontrado")
            : base(
                  statusCode: 404,
                  title: "El recurso no fue encontrado",
                  errors: errors,
                  error: message)
        {
        }
    }
}

