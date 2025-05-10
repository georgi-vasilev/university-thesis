namespace Infrastructure.Authentication
{
    using ErrorOr;

    public interface IAuthService
    {
        Task<ErrorOr<string>> RegisterAsync(string email, string password, string role);
        Task<ErrorOr<string>> LoginAsync(string email, string password);
    }
}
