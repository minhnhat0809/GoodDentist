using BusinessObject.DTO;

namespace Services;

public interface IAuthService
{
    Task<ResponseLoginDTO> Authenticate(LoginDTO loginDto);
}