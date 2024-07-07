using BusinessObject.DTO;

namespace Services;

public interface IAuthService
{
    Task<ResponseLoginDTO> Authenticate(LoginDTO loginDto);
    Task<ResponseDTO> GetAccountByEmailOrPhone(string emailOrPhone);

    Task<ResponseLoginDTO> ResetPassword(Guid userId, string password);
    Task<ResponseDTO> GetUserAsync(Guid userId);
}