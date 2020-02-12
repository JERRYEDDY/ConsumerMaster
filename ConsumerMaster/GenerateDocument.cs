using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Styles;
namespace ConsumerMaster
{
    public class GenerateDocument
    {

        private string GetMimeType(string fileExt)
        {
            string mimeType = String.Empty;
            switch (fileExt.ToLower())
            {
                case ".docx": mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
                case ".rtf": mimeType = "application/rtf"; break;
                case ".html": mimeType = "text/html"; break;
                case ".txt": mimeType = "text/plain"; break;
            }
            return mimeType;
        }

        private RadFlowDocument CreateDocument()
        {
            RadFlowDocument document = new RadFlowDocument();
            RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);
            editor.ParagraphFormatting.TextAlignment.LocalValue = Alignment.Justified;

            // Body
            editor.InsertLine("Dear Telerik User,");
            editor.InsertText("We�re happy to introduce the new Telerik RadWordsProcessing component for WPF. High performance library that enables you to read, write and manipulate documents in DOCX, RTF and plain text format. The document model is independent from UI and ");
            Run run = editor.InsertText("does not require");
            run.Underline.Pattern = UnderlinePattern.Single;
            editor.InsertLine(" Microsoft Office.");

            editor.InsertText("The current community preview version comes with full rich-text capabilities including ");
            editor.InsertText("bold, ").FontWeight = FontWeights.Bold;
            editor.InsertText("italic, ").FontStyle = FontStyles.Italic;
            editor.InsertText("underline,").Underline.Pattern = UnderlinePattern.Single;
            editor.InsertText(" font sizes and ").FontSize = 20;
            editor.InsertText("colors ").ForegroundColor = ThemableColor.FromArgb(100, 92, 230, 0);

            editor.InsertLine("as well as text alignment and indentation. Other options include tables, hyperlinks, inline and floating images. Even more sweetness is added by the built-in styles and themes.");

            editor.InsertText("Here at Telerik we strive to provide the best services possible and fulfill all needs you as a customer may have. We would appreciate any feedback you send our way through the ");
            editor.InsertHyperlink("public forums", "http://www.telerik.com/forums", false, "Telerik Forums");
            editor.InsertLine(" or support ticketing system.");

            editor.InsertLine("We hope you�ll enjoy RadWordsProcessing as much as we do. Happy coding!");
            editor.InsertParagraph();
            editor.InsertText("Kind regards,");
            //this.CreateSignature(editor);

            //this.CreateHeader(editor);

            //this.CreateFooter(editor);

            return document;
        }

        private void CreateFooter(RadFlowDocumentEditor editor)
        {
            String line1 = "655 Jefferson Avenue, Washington, PA 15301";
            String line2 = "Telephone: (724)225-8145 Fax: (724)225-4934";
            String line3 = "www.yourawc.org";

            Footer footer = editor.Document.Sections.First().Footers.Add();
            Paragraph paragraph = footer.Blocks.AddParagraph();
            paragraph.TextAlignment = Alignment.Right;

            editor.MoveToParagraphStart(paragraph);
            editor.InsertLine(line1);
            editor.InsertLine(line2);
            editor.InsertLine(line3);

        }

        private void CreateHeader(RadFlowDocumentEditor editor)
        {
            Header header = editor.Document.Sections.First().Headers.Add();
            editor.MoveToParagraphStart(header.Blocks.AddParagraph());
            using (FileStream fs = new FileStream("./Images/Telerik_develop_experiences.png", FileMode.Open))
            {
                editor.InsertImageInline(fs, "png", new Size(660, 237));
            }
        }
    }
}