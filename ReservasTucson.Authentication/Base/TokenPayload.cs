namespace ReservasTucson.Authentication.Base
{
    public class TokenPayload
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> Grupos { get; set; } = Enumerable.Empty<string>();
    }
}

