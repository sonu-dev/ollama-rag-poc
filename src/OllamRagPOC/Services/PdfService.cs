using UglyToad.PdfPig;

namespace OllamRagPOC.Services
{
    public class PdfService : IPdfService
    {
        public string ExtractText(string filePath)
        {
            using var pdf = PdfDocument.Open(filePath);

            var text = "";

            foreach (var page in pdf.GetPages())
            {
                text += page.Text + Environment.NewLine;
            }

            return text;
        }
    }

}
