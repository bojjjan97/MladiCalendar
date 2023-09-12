namespace WebAPI.AppDtos.ConfigModels
{
    public class AuthConfig
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}
