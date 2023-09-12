namespace WebAPI.Controllers.Event.Dtos
{
    public class PaginatedResponseDto
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public IEnumerable<dynamic> Data { get; set; }

        public int TotalCertificatesSent { get; set; }
    }
}
