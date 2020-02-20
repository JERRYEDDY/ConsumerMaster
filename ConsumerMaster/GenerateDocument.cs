using System;
using GemBox.Document;
using System.Data;
using System.IO;

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

        //public RadFlowDocument CreateDocument()
        //{

        //    //RadFlowDocument document = new RadFlowDocument();
        //    //RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);

        //    //try
        //    //{
        //    //    Paragraph paragraph0 = editor.InsertParagraph();
        //    //    paragraph0.Spacing.SpacingBefore = 0;
        //    //    paragraph0.Spacing.SpacingAfter = 0;
        //    //    paragraph0.Spacing.LineSpacing = 1;
        //    //    editor.InsertField("MERGEFIELD FirstName ", "«FirstName»");
        //    //    editor.InsertText(" ");
        //    //    editor.InsertField("MERGEFIELD LastName ", "«LastName»");

        //    //    Paragraph paragraph1 = editor.InsertParagraph();
        //    //    paragraph1.Spacing.SpacingBefore = 0;
        //    //    paragraph1.Spacing.SpacingAfter = 0;
        //    //    paragraph1.Spacing.LineSpacing = 1;
        //    //    editor.InsertText("225 Mary St.");

        //    //    Paragraph paragraph2 = editor.InsertParagraph();
        //    //    paragraph2.Spacing.SpacingBefore = 0;
        //    //    paragraph2.Spacing.SpacingAfter = 0;
        //    //    paragraph2.Spacing.LineSpacing = 1;
        //    //    editor.InsertText("New Castle, PA 16101");

        //    //    ImageInline imageInline = paragraph2.Inlines.AddImageInline();

        //    //    editor.InsertParagraph();
        //    //    editor.InsertParagraph();
        //    //    editor.InsertText("On behalf of ");
        //    //    editor.InsertField("MERGEFIELD CompanyName ", "«CompanyName»");
        //    //    editor.InsertText(", ");
        //    //    editor.InsertText("I would like to thank you for purchasing ");
        //    //    editor.InsertField("MERGEFIELD PurchasedItemsCount ", "«PurchasedItemsCount»");
        //    //    editor.InsertText(" ");
        //    //    editor.InsertField("MERGEFIELD ProductName ", "«ProductName»");
        //    //    editor.InsertText(" from us.");
        //    //    editor.InsertParagraph();
        //    //    editor.InsertText("We are committed to provide you with the highest level of customer satisfaction possible. ");
        //    //    editor.InsertText("If for any reasons you have questions or comments please call ");
        //    //    editor.InsertField("MERGEFIELD ProductSupportPhone ", "«ProductSupportPhone»");
        //    //    editor.InsertText(" ");
        //    //    editor.InsertField("MERGEFIELD ProductSupportPhoneAvailability ", "«ProductSupportPhoneAvailability»");
        //    //    editor.InsertText(" or email us at ");
        //    //    editor.InsertField("MERGEFIELD ProductSupportEmail ", "«ProductSupportEmail»");
        //    //    editor.InsertText(".");
        //    //    editor.InsertParagraph();
        //    //    editor.InsertText("Once again thank you for choosing ");
        //    //    editor.InsertField("MERGEFIELD CompanyName ", "«CompanyName»");
        //    //    editor.InsertText(".");
        //    //    editor.InsertParagraph();
        //    //    editor.InsertParagraph();
        //    //    editor.InsertText("Sincerely yours,");
        //    //    editor.InsertParagraph();
        //    //    editor.InsertField("MERGEFIELD SalesRepFirstName ", "«SalesRepFirstName»");
        //    //    editor.InsertText(" ");
        //    //    editor.InsertField("MERGEFIELD SalesRepLastName ", "«SalesRepLastName»");
        //    //    editor.InsertText(",");
        //    //    editor.InsertParagraph();
        //    //    editor.InsertField("MERGEFIELD SalesRepTitle ", "«SalesRepTitle»");
        //    //} 
        //    //catch (Exception ex)
        //    //{

        //    //};

        //    //return document;
        //}

        //private void CreateFooter(RadFlowDocumentEditor editor)
        //{
        //    try
        //    {
        //    //    String line1 = "655 Jefferson Avenue, Washington, PA 15301";
        //    //    String line2 = "Telephone: (724)225-8145 Fax: (724)225-4934";
        //    //    String line3 = "www.yourawc.org";

        //    //    Footer footer = editor.Document.Sections.First().Footers.Add();
        //    //    editor.ParagraphFormatting.TextAlignment.LocalValue = Alignment.Center;
        //    //    Paragraph paragraph = footer.Blocks.AddParagraph();
        //    //    //paragraph.TextAlignment = Alignment.Center;
        //    //    //paragraph.Spacing.LineSpacing = 1;
        //    //    //editor.MoveToParagraphStart(paragraph);

        //    //    editor.InsertText(line1 + Environment.NewLine);
        //    //    editor.InsertLine(line2);
        //    //    editor.InsertLine(line3);
        //    //}
        //    //catch(Exception ex)
        //    //{

        //    //}
        //}

        //private void CreateHeader(RadFlowDocumentEditor editor)
        //{
        //    try
        //    {
        //        String line1 = "PATHWAYS OF SOUTHWESTERN PA, INC.";
        //        String line2 = "655 JEFFERSON AVENUE";
        //        String line3 = "WASHINGTON, PA 15301";

        //        Header header = editor.Document.Sections.First().Headers.Add();
        //        editor.MoveToParagraphStart(header.Blocks.AddParagraph());
        //        editor.InsertLine(line1);
        //        editor.InsertLine(line2);
        //        editor.InsertLine(line3);
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

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

        public DataSet CreateClientStaffAuthorizationDataSet()
        {
            string clientsRangeName = "Clients";
            string membersRangeName = "Members";
            string authorizationsRangeName = "Authorizations";

            try
            {
            // Create relational data.
                var clients = new DataTable(clientsRangeName);
                clients.Columns.Add("ClientID", typeof(int));
                clients.Columns.Add("ClientFirst", typeof(string));
                clients.Columns.Add("ClientLast", typeof(string));
                clients.Columns.Add("StreetAddress1", typeof(string));
                clients.Columns.Add("StreetAddress2", typeof(string));
                clients.Columns.Add("City", typeof(string));
                clients.Columns.Add("State", typeof(string));
                clients.Columns.Add("ZipCode", typeof(string));
                clients.Columns.Add("EmailAddress", typeof(string));

                var members = new DataTable(membersRangeName);
                members.Columns.Add("ClientID", typeof(int));
                members.Columns.Add("MemberID", typeof(string));
                members.Columns.Add("MemberName", typeof(string));
                members.Columns.Add("MemberRole", typeof(string));

                var authorizations = new DataTable(authorizationsRangeName);
                authorizations.Columns.Add("ClientID", typeof(int));
                authorizations.Columns.Add("From", typeof(string));
                authorizations.Columns.Add("To", typeof(string));
                authorizations.Columns.Add("Service", typeof(string));
                authorizations.Columns.Add("Total", typeof(string));
                authorizations.Columns.Add("Used", typeof(string));
                authorizations.Columns.Add("Balance", typeof(string));

                // Create DataSet with parent-child relation.
                var data = new DataSet();
                data.Tables.Add(clients);
                data.Tables.Add(members);
                data.Tables.Add(authorizations);
                data.Relations.Add(membersRangeName, clients.Columns["ClientID"], members.Columns["ClientID"]);
                data.Relations.Add(authorizationsRangeName, clients.Columns["ClientID"], authorizations.Columns["ClientID"]);

                int clientID = 238;
                string clientFirst = "Thomas";
                string clientLast = "Ali";
                string streetAddress1 = "135 Fox Chase Drive";
                string streetAddress2 = " ";
                string city = "Canonsburg";
                string state = "Pennsylvania";
                string zipCode = "153170000";
                string emailAddress = "tali@gmail.com";

                clients.Rows.Add(clientID, clientFirst, clientLast, streetAddress1, streetAddress2, city, state, zipCode, emailAddress);

                for (int itemIndex = 1; itemIndex <= 5; itemIndex++)
                {
                    string memberID = "4386";
                    string memberName = "Smith, John";
                    string memberRole = "Support Service Professional";

                    members.Rows.Add(clientID, memberID, memberName, memberRole);
                }

                for (int authorizationIndex = 1; authorizationIndex <= 5; authorizationIndex++)
                {
                    string from = "07/01/2019";
                    string to = "06/30/2020";
                    string service = "Respite 1:1 Enhanced 15 min W/B (W9863)";
                    string total = "8000";
                    string used = "0";
                    string balance = "8000";
                    //string staffRole = "Respite 1:1 Enhanced 15 min W/B (W9863)";

                    authorizations.Rows.Add(clientID, from, to, service, total, used, balance);
                }

                clients.Rows.Add(493, "Hoda","Kotb", "655 Jefferson Avenue", " ", "Pittsburgh", "Pennsylvania", "153170000", "hkotb@hotmail.com");

                for (int itemIndex = 1; itemIndex <= 5; itemIndex++)
                {
                    string memberID = "6348";
                    string memberName = "Jones, Alex";
                    string memberRole = "Support Service Professional";

                    members.Rows.Add(493, memberID, memberName, memberRole);
                }

                for (int authorizationIndex = 1; authorizationIndex <= 5; authorizationIndex++)
                {
                    string from = "07/01/2019";
                    string to = "06/30/2020";
                    string service = "Companion W/B (W1726)";
                    string total = "6000";
                    string used = "200";
                    string balance = "5800";

                    authorizations.Rows.Add(493, from, to, service, total, used, balance);
                }

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void GemBoxNestMailMerge(DataSet data)
        {
            try
            {
                // If using Professional version, put your serial key below.
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

                //var data = CreateClientStaffAuthorizationDataSet();

                var document = DocumentModel.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/MergeNestedRanges.docx"));

                // Execute nested mail merge.
                document.MailMerge.Execute(data, null);

                using (var ms = new MemoryStream())
                {

                }

                //document.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Merged Nested Ranges Output.docx"));


            }
            catch (Exception ex)
            {

            }
        }
    }
}