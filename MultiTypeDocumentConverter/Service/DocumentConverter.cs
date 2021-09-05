using System.IO;
using System.Text;

namespace MultiTypeDocumentConverter.Service
{
    public class DocumentConverter : IDocumentConverter
    {
        private string basePath = @"E:\Workspace";
        public string PdfToHtml()
        {
            // Load source PDF file
            Aspose.Pdf.Document doc = new Aspose.Pdf.Document(Path.Combine(basePath, "ABHINEET_VERMA_Resume.pdf"));
            // Instantiate HTML Save options object
            Aspose.Pdf.HtmlSaveOptions newOptions = new Aspose.Pdf.HtmlSaveOptions();

            // Enable option to embed all resources inside the HTML
            newOptions.PartsEmbeddingMode = Aspose.Pdf.HtmlSaveOptions.PartsEmbeddingModes.EmbedAllIntoHtml;

            // This is just optimization for IE and can be omitted 
            newOptions.LettersPositioningMethod = Aspose.Pdf.HtmlSaveOptions.LettersPositioningMethods.UseEmUnitsAndCompensationOfRoundingErrorsInCss;
            newOptions.RasterImagesSavingMode = Aspose.Pdf.HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
            newOptions.FontSavingMode = Aspose.Pdf.HtmlSaveOptions.FontSavingModes.SaveInAllFormats;
            // Output file path 
            string outHtmlFile = Path.Combine(basePath, "output.html");
            doc.Save(outHtmlFile, newOptions);
            return null;
        }

        public string DocToHtml(string FilePath)
        {
            string outFolder = "";
            if (FilePath.IndexOf(".doc") != -1)
                outFolder = FilePath.Replace(".doc", ".html");
            else
                outFolder = FilePath.Replace(".docx", ".html");

            if (!File.Exists(outFolder))
            {
                // Load source PDF file
                Aspose.Words.Document doc = new Aspose.Words.Document(FilePath);
                // Instantiate HTML Save options object
                doc.Save(outFolder, Aspose.Words.SaveFormat.Html);
            }

            byte[] fileTypes = File.ReadAllBytes(outFolder);
            string Html = Encoding.ASCII.GetString(fileTypes);
            return Html;
        }
    }
}
