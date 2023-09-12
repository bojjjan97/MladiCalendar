using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using Microsoft.AspNetCore.Server.IIS.Core;
using Org.BouncyCastle.Utilities;
using WebAPI.AppDtos.Enums;
using WebAPI.AppDtos;
using WebAPI.Data;
using WebAPI.StandaloneServices.EmailService.Dtos;
using WebAPI.StandaloneServices.EmailServiceNamespace;
using Microsoft.EntityFrameworkCore;
using WebAPI.StandaloneServices.CertificateBuilderService.Models;
using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.StandaloneServices.CertificateBuilderService
{
    public class CertificateBuilderService
    {
        private EmailServiceNamespace.EmailService emailService;
        private MainContext _context;
        private IWebHostEnvironment env;
        public CertificateBuilderService(EmailServiceNamespace.EmailService emailService, MainContext context, IWebHostEnvironment env)
        {
            this.emailService = emailService;
            this._context = context;
            this.env = env;
        }

        public async Task<ServiceResponseDto> SendCertificate(long eventId,[FromBody] string[] userIds)
        {
            try
            {
                var ev = await _context.Events
                    .Include(x => x.UsersOnEvent)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == eventId);


                var certificateData = ev.UsersOnEvent.Where(uoe => userIds.Contains(uoe.UserId)).Select(x => new UserCertificateData
                {
                    Email = x.User.Email,
                    Name = $"{x.User.FirstName} {x.User.LastName}",
                    EventDate = ev.EventDateTime.ToString("dd.MM.yyyy.")
                }).ToList();

                certificateData.ForEach(async x =>
               {
                   var cert = this.GenerateCertifcate(x.Name, x.EventDate);
                   await this.emailService.SendEmailAsync(new MailRequest
                   {
                       Body = "Certificate for participating on event.",
                       Attachment = cert,
                       ToEmail = x.Email,
                       Subject = "Certificate"
                   });
               });
                await _context.UsersOnEvents.Where(uoe => userIds.Contains(uoe.UserId))
                    .ExecuteUpdateAsync(uoe => uoe.SetProperty(x=>x.EmailSent,true));
                await _context.SaveChangesAsync();
                return new ServiceResponseDto { Message = "Certificates successfully sent.", ResponseType = EResponseType.Success };
            }
            catch
            {
                return new ServiceResponseDto { Message="Error while sending certificates, please try again later.",ResponseType= EResponseType.Error };
            }
        }

        private PdfCertificate GenerateCertifcate(string name,string date)
        {
            var templateFolderPath = Path.Combine(env.ContentRootPath, "StandaloneServices","CertificateBuilderService","Templates","Certificate.pdf");
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                using (PdfReader pdfReader = new PdfReader(templateFolderPath))
                {
                    using (PdfWriter pdfWriter = new PdfWriter(ms))
                    {
                        using (PdfDocument pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                        {
                            Document document = new Document(pdfDocument);

                            Paragraph pdfName = new Paragraph(name);
                            pdfName.SetTextAlignment(TextAlignment.CENTER)
                                .SetFixedPosition(160, 330, 500)
                                .SetFontColor(iText.Kernel.Colors.ColorConstants.BLACK)
                                .SetFontSize(32)
                                .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                            document.Add(pdfName);
                            Paragraph pdfDate = new Paragraph(date);
                            pdfDate.SetTextAlignment(TextAlignment.CENTER)
                                .SetFixedPosition(650, 75, 100)
                                .SetFontColor(new DeviceRgb(0x1F, 0x1D, 0x1F))
                                .SetFontSize(12)
                                .SetBackgroundColor(new DeviceRgb(0xE8, 0xF2, 0xFA));
                            document.Add(pdfDate);

                            document.Close();
                            bytes = ms.ToArray();

                        }
                    }
                }
            }
            return new PdfCertificate { File = bytes, FileName = "Certificate.pdf" };
        }
    }
}
