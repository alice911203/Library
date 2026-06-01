using booksystem.api.DTOs.Auth;

namespace booksystem.api.Service
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto dto);
        Task<string> RegisterAsync(RegisterDto dto);
    }
}
