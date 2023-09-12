namespace WebAPI.Controllers.Auth.Dtos
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
