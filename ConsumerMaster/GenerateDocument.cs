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
using System.Windows.Media;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;

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

        public RadFlowDocument CreateDocument()
        {

            RadFlowDocument document = new RadFlowDocument();
            RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);

            try
            {
                Paragraph paragraph0 = editor.InsertParagraph();
                paragraph0.Spacing.SpacingBefore = 0;
                paragraph0.Spacing.SpacingAfter = 0;
                paragraph0.Spacing.LineSpacing = 1;
                editor.InsertField("MERGEFIELD FirstName ", "«FirstName»");
                editor.InsertText(" ");
                editor.InsertField("MERGEFIELD LastName ", "«LastName»");

                Paragraph paragraph1 = editor.InsertParagraph();
                paragraph1.Spacing.SpacingBefore = 0;
                paragraph1.Spacing.SpacingAfter = 0;
                paragraph1.Spacing.LineSpacing = 1;
                editor.InsertText("225 Mary St.");

                Paragraph paragraph2 = editor.InsertParagraph();
                paragraph2.Spacing.SpacingBefore = 0;
                paragraph2.Spacing.SpacingAfter = 0;
                paragraph2.Spacing.LineSpacing = 1;
                editor.InsertText("New Castle, PA 16101");


                editor.InsertParagraph();
                editor.InsertParagraph();
                editor.InsertText("On behalf of ");
                editor.InsertField("MERGEFIELD CompanyName ", "«CompanyName»");
                editor.InsertText(", ");
                editor.InsertText("I would like to thank you for purchasing ");
                editor.InsertField("MERGEFIELD PurchasedItemsCount ", "«PurchasedItemsCount»");
                editor.InsertText(" ");
                editor.InsertField("MERGEFIELD ProductName ", "«ProductName»");
                editor.InsertText(" from us.");
                editor.InsertParagraph();
                editor.InsertText("We are committed to provide you with the highest level of customer satisfaction possible. ");
                editor.InsertText("If for any reasons you have questions or comments please call ");
                editor.InsertField("MERGEFIELD ProductSupportPhone ", "«ProductSupportPhone»");
                editor.InsertText(" ");
                editor.InsertField("MERGEFIELD ProductSupportPhoneAvailability ", "«ProductSupportPhoneAvailability»");
                editor.InsertText(" or email us at ");
                editor.InsertField("MERGEFIELD ProductSupportEmail ", "«ProductSupportEmail»");
                editor.InsertText(".");
                editor.InsertParagraph();
                editor.InsertText("Once again thank you for choosing ");
                editor.InsertField("MERGEFIELD CompanyName ", "«CompanyName»");
                editor.InsertText(".");
                editor.InsertParagraph();
                editor.InsertParagraph();
                editor.InsertText("Sincerely yours,");
                editor.InsertParagraph();
                editor.InsertField("MERGEFIELD SalesRepFirstName ", "«SalesRepFirstName»");
                editor.InsertText(" ");
                editor.InsertField("MERGEFIELD SalesRepLastName ", "«SalesRepLastName»");
                editor.InsertText(",");
                editor.InsertParagraph();
                editor.InsertField("MERGEFIELD SalesRepTitle ", "«SalesRepTitle»");
            } 
            catch (Exception ex)
            {

            };

            return document;
        }

        private void CreateFooter(RadFlowDocumentEditor editor)
        {
            try
            {
                String line1 = "655 Jefferson Avenue, Washington, PA 15301";
                String line2 = "Telephone: (724)225-8145 Fax: (724)225-4934";
                String line3 = "www.yourawc.org";

                Footer footer = editor.Document.Sections.First().Footers.Add();
                editor.ParagraphFormatting.TextAlignment.LocalValue = Alignment.Center;
                Paragraph paragraph = footer.Blocks.AddParagraph();
                //paragraph.TextAlignment = Alignment.Center;
                //paragraph.Spacing.LineSpacing = 1;
                //editor.MoveToParagraphStart(paragraph);

                editor.InsertText(line1 + Environment.NewLine);
                editor.InsertLine(line2);
                editor.InsertLine(line3);
            }
            catch(Exception ex)
            {

            }
        }

        private void CreateHeader(RadFlowDocumentEditor editor)
        {
            try
            {
                String line1 = "PATHWAYS OF SOUTHWESTERN PA, INC.";
                String line2 = "655 JEFFERSON AVENUE";
                String line3 = "WASHINGTON, PA 15301";

                Header header = editor.Document.Sections.First().Headers.Add();
                editor.MoveToParagraphStart(header.Blocks.AddParagraph());
                editor.InsertLine(line1);
                editor.InsertLine(line2);
                editor.InsertLine(line3);
            }
            catch(Exception ex)
            {

            }
        }

        public class FollowUpData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
            public int PurchasedItemsCount { get; set; }
            public string ProductName { get; set; }
            public string ProductSupportPhone { get; set; }
            public string ProductSupportPhoneAvailability { get; set; }
            public string ProductSupportEmail { get; set; }
            public string SalesRepFirstName { get; set; }
            public string SalesRepLastName { get; set; }
            public string SalesRepTitle { get; set; }
        }

    }
}