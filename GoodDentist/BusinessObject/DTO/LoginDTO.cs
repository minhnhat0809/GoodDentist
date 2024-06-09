namespace BusinessObject.DTO
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ResponseLoginDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public string AccessToken { get; set; }
    }
}