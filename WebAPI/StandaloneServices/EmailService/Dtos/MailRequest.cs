namespace WebAPI.StandaloneServices.EmailService.Dtos
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public PdfCertificate Attachment { get; set; }
    }
}

//using (PdfReader pdfReader = new PdfReader(existingPdfPath))
//{
//    using (PdfWriter pdfWriter = new PdfWriter(outputPdfPath))
//    {
//        using (PdfDocument pdfDocument = new PdfDocument(pdfReader, pdfWriter))
//        {
//            Document document = new Document(pdfDocument);

//            // Get the first page of the PDF
//            PdfPage page = pdfDocument.GetFirstPage();

//            // Add text over the image
//            PdfCanvas canvas = new PdfCanvas(page);
//            canvas.BeginText()
//                  .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(), 40)
//                  .MoveText(300, 330) // X, Y position of the text
//                  .ShowText("Hello, iText 7!")
//                  .EndText();

//            canvas.BeginText()
//                  .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(), 40)
//                  .MoveText(550, 30) // X, Y position of the text
//                  .ShowText("1.2.2020")
//                  .EndText();

//            // Draw a rectangle over the image
//            //canvas.SetFillColor(iText.Kernel.Colors.ColorConstants.RED)
//            //      .Rectangle(300, 200, 400, 500) // X, Y, width, height of the rectangle
//            //      .Fill();

//            // Close the document to save changes
//            document.Close();

//        }
//    }
//}
