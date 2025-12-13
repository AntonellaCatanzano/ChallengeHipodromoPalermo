    using ReservasTucson.Authentication.Interfaces;


    namespace ReservasTucson.Authentication.Implementations
    {
        public class TokenParameters : ITokenParameters
        {
            public string UserName { get; set; } = null!;

            public string PasswordHash { get; set; } = null!;

            public string Id { get; set; } = null!;
        }
    }
