namespace WebAPI.StandaloneServices.EmailService.Dtos
{
    public class PdfCertificate
    {
        public byte[] File { get; set; }
        public string FileName { get; set; } = "certificate.pdf";
    }
}
