using System;
using System.Collections.Generic;

namespace ReservasTucson.Domain.Support.Helpers
{
    public class ValidationException : BackendException
    {
        public ValidationException(string message)
            : base(
                  statusCode: 400,
                  title: "Validation Error",
                  errors: new List<string> { message },
                  error: message)
        {
        }

        public ValidationException(List<string> errors, string message = "Validation Error")
            : base(
                  statusCode: 400,
                  title: "Validation Error",
                  errors: errors,
                  error: message)
        {
        }
    }
}

