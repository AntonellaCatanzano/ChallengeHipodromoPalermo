
namespace ReservasTucson.Authentication.Interfaces
{
    public interface ITokenParameters
    {
        string Id { get; set; }
        string UserName { get; set; }
        string PasswordHash { get; set; }
    }

}
