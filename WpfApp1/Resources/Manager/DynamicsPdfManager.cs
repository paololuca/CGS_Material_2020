using BusinessEntity.Entity;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    class DynamicsPdfManager
    {



        #region Member Variables

        // Top, bottom, left and right margins of report
        private static float margin = 36;
        // Height of the header
        private static float headerHeight = 74;
        // Height of the footer
        private static float footerHeight = 14;
        // Size of paper to use
        private static PageDimensions pageSize = new PageDimensions(PageSize.Letter, PageOrientation.Landscape, margin);
        // Bottom Y coordinate for the body of the report
        private static float bodyBottom = pageSize.Height - (margin * 2) - footerHeight;

        // Current page that elements are being added to
        private ceTe.DynamicPDF.Page currentPage = null;

        // Current Y coordinate where elements are being added
        private float currentY = 0;
        // Used to control the alternating background
        private bool alternateBG = false;
        // Used to test for grouping
        private string currentFirstI = string.Empty;
        // Template for header and footer elements

        private static Template template = null;
        // Connection string to the Northwind database


        #endregion

        #region Page Load

        public void StampaGironi(List<List<AtletaEntity>> gironi, String nomeTorneo, String disciplina)
        {


            if (template == null)
                SetTemplate(nomeTorneo, disciplina);

            // Create a document and set it's properties
            Document document = new Document();


            document.Creator = "HTS";
            document.Author = "PL Keypresser Inc.";
            document.Title = "Fighter's list";
            document.Template = template;


            // Builds the report
            BuildDocument(document, gironi);



            // Outputs the ContactList to the current web page
            //document.DrawToWeb("ContactList.pdf");

            document.Draw(".\\PDF\\Gironi_dynamic" + nomeTorneo + "_" + disciplina + ".pdf");
            //System.IO.File.WriteAllBytes(".\\PDF\\Gironi_" + nomeTorneo + "_" + disciplina + ".pdf", pdfBytes);

            Process.Start(".\\PDF\\Gironi_dynamic" + nomeTorneo + "_" + disciplina + ".pdf");
        }

        #endregion

        #region Private Methods

        private void SetTemplate(string nomeTorneo, string disciplina)
        {
            template = new Template();

            // Uncomment the line below to add a layout grid to the each page
            //template.Elements.Add( new LayoutGrid() );

            // Adds header elements to the template
            //template.Elements.Add(new Image(MapPath("../Images/DPDFLogo.png"), 0, 0, 0.21f));
            template.Elements.Add(new Label(nomeTorneo + " - " + disciplina, 0, 0, 720, 18, Font.HelveticaBold, 18, TextAlign.Center));
            template.Elements.Add(new Label("Gironi", 0, 21, 720, 12, Font.Helvetica, 12, TextAlign.Center));
            //template.Elements.Add(new Label(DateTime.Now.ToString("dd MMM yyyy, H:mm:ss EST"), 0, 36, 720, 12, Font.Helvetica, 12, TextAlign.Center));
            template.Elements.Add(new Rectangle(0, 56, 720, 16, Grayscale.Black, new WebColor("0000A0"), 0.0F));
            template.Elements.Add(new Label("Surname", 2, 57, 156, 12, Font.HelveticaBold, 12, TextAlign.Left, Grayscale.White));
            template.Elements.Add(new Label("Name", 160, 57, 156, 12, Font.HelveticaBold, 12, TextAlign.Left,Grayscale.White));
            template.Elements.Add(new Label("Club", 318, 57, 600, 12, Font.HelveticaBold, 12, TextAlign.Left,Grayscale.White));

            // Adds footer elements to the template
            // PageNumberingLabel pageNumLabel = new PageNumberingLabel("Page %%CP(i)%% of %%TP(i)%%", 0, bodyBottom + 5, 720, 10,
            //Font.Helvetica, 10, TextAlign.Center);
            // template.Elements.Add(pageNumLabel);
        }

        private void BuildDocument(Document document, List<List<AtletaEntity>> gironi)
        {
            // Builds the PDF document with data from the DataReader
            AddNewPage(document);
            
            int i = 1;

            foreach (var g in gironi)
            {
                foreach (var data in g)
                    // Add current record to the document
                    AddRecord(document, data, "Girone " + i);

                i++;
            }            
        }

        private void AddRecord(Document document, AtletaEntity data, string girone)
        {
            // Creates TextAreas that are expandable
            TextArea name = new TextArea(data.Cognome, 2, currentY + 3, 156, 11, Font.TimesRoman, 11);
            TextArea surname = new TextArea(data.Nome, 160, currentY + 3, 136, 11, Font.TimesRoman, 11);
            TextArea club = new TextArea(data.Asd, 318, currentY + 3, 156, 11, Font.TimesRoman, 11);

            // Gets the height required for the current record
            float requiredHeight = SetExpandableRecords(document, data, name, surname, club, girone);

            // Creates non expandable Labels
            //Label customerID = new Label(data., 2, currentY + 3, 58, 11, Font.TimesRoman, 11);
            //Label phone = new Label(data.GetString(5), 522, currentY + 3, 96, 11, Font.TimesRoman, 11);
            //Label fax = new Label(data.IsDBNull(6) ? "" : data.GetString(6), 622, currentY + 3, 96, 11, Font.TimesRoman, 11);

            // Adds alternating background if required
            if (alternateBG)
            {
                currentPage.Elements.Add(new Rectangle(0, currentY, 720, requiredHeight + 6, RgbColor.Black, new WebColor("E0E0FF"), 0.0F));
            }

            // Toggles alternating background
            alternateBG = !alternateBG;

            // Adds elements to the current page
            //currentPage.Elements.Add(customerID);
            currentPage.Elements.Add(name);
            currentPage.Elements.Add(surname);
            currentPage.Elements.Add(club);
            //currentPage.Elements.Add(phone);
            //currentPage.Elements.Add(fax);


            // increments the current Y position on the page
            currentY += requiredHeight + 6;
        }

        private float SetExpandableRecords(Document document, AtletaEntity atleta, TextArea name, TextArea surname,
       TextArea club, string girone)
        {
            // Gets the maximum height requred of the three TextAreas
            float requiredHeight = GetMaxRecordHeight(name, surname, club);

            // Add space for the section header if required
            float sectionHeaderHeight = 0;

            if (currentFirstI != girone) sectionHeaderHeight = 26;

            // Add a new page if needed
            if (bodyBottom < currentY + requiredHeight + sectionHeaderHeight + 20)
            {
                AddNewPage(document);

                if (sectionHeaderHeight == 0)
                {
                    // Update Y coordinate of TextArea when placed on the new page
                    name.Y = currentY + 3;
                    surname.Y = currentY + 3;
                    club.Y = currentY + 3;
                }
            }

            // Add section header if required
            if (sectionHeaderHeight > 0)
            {
                AddSectionHeader(girone);
                name.Y = currentY + 3;
                surname.Y = currentY + 3;
                club.Y = currentY + 3;
            }
            return requiredHeight;
        }

        private float GetMaxRecordHeight(TextArea name, TextArea surname, TextArea contactTitle)
        {
            // Returns the maximum required height of the three TextAreas
            float requiredHeight = 11;
            float requiredHeightB = 0;

            requiredHeight = name.GetRequiredHeight();
            requiredHeightB = surname.GetRequiredHeight();
            if (requiredHeightB > requiredHeight) requiredHeight = requiredHeightB;
            requiredHeightB = contactTitle.GetRequiredHeight();
            if (requiredHeightB > requiredHeight) requiredHeight = requiredHeightB;

            if (requiredHeight > 11)
            {
                name.Height = requiredHeight;
                surname.Height = requiredHeight;
                contactTitle.Height = requiredHeight;
            }
            return requiredHeight;
        }

        private void AddSectionHeader(string girone)
        {
            // Adds a section header to the current Y coordinate of the current page
            currentFirstI = girone;
            currentPage.Elements.Add(new Label("- " + currentFirstI + " -", 0, currentY + 6, 720, 18, Font.HelveticaBold, 18,TextAlign.Center));
            currentY += 26;
            alternateBG = false;
        }

        private void AddNewPage(Document document)
        {
            // Adds a new page to the document
            currentPage = new Page(pageSize);
            currentY = headerHeight;
            alternateBG = false;
            document.Pages.Add(currentPage);
        }



        #endregion
    }

}
