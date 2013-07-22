using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using context;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace TestConnectionString
{
    public static class PDFFileCreator
    {
        public static void CreateTable(string pdfFile)
        {
            SupermarketSalesContext model = new SupermarketSalesContext();
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document,
                               new FileStream(pdfFile, FileMode.Create));

            document.Open();
            PdfPTable table = new PdfPTable(5);

            CreateCell(table, DateTime.Now.ToString(), true, true);

            CreateCell(table, "Product");

            CreateCell(table, "Quantity");

            CreateCell(table, "Unit Price");

            CreateCell(table, "Location");

            CreateCell(table, "Sum");


            var products = model.Locations.Select(p => p);

            foreach (var product in products)
            {
                CreateCell(table, product.LocationName, false, false);
            }

            document.Add(table);
            document.Close();
        }

        private static void CreateCell(PdfPTable table, string name, bool hasColor = true, bool hasColumnSpan = false)
        {
            Cell cell = new Cell();
            Phrase phrase = new Phrase();
            phrase.Add(new Chunk(name));
            cell.AddElement(phrase);
            cell.Border = PdfCell.BOTTOM_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.TOP_BORDER;
            if (hasColor)
            {
                cell.BackgroundColor = Color.LIGHT_GRAY;
            }

            if (hasColumnSpan)
            {
                cell.Colspan = 5;
            }

            table.AddCell(cell.CreatePdfPCell());
        }
    }
}
