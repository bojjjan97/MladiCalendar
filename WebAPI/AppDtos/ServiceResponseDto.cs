using WebAPI.AppDtos.Enums;

namespace WebAPI.AppDtos
{
    public class ServiceResponseDto
    {
        public EResponseType ResponseType { get; set; }
        public string Message { get; set; }
        public object? ResponseObject { get; set; }
    }
}
