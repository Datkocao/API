namespace API.Models.DTO
{
    public class LoginRequestDTO
    {
        public string JwtToken { set; get; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
