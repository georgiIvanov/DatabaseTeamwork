using iTextSharp.text;
using iTextSharp.text.pdf;
using SQLStore.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Windows;

namespace SupermarketManager
{
    class PdfRow
    {
        public string ProductName { get; set; }
        public decimal Sum { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }

    class GeneratePDF
    {
        public GeneratePDF()
        {

        }

        public static List<IGrouping<DateTime, PdfRow>> GetSalesByDate()
        {
            List<IGrouping<DateTime, PdfRow>> collection;
            using (SQLStoreDb db = new SQLStoreDb())
            {
                collection = (from sale in db.Sales
                              join loc in db.Locations
                              on sale.LocationID equals loc.LocationID
                              select new PdfRow
                              {
                                  ProductName = sale.Product.ProductName,
                                  Sum = sale.Sum,
                                  Quantity = sale.Quantity,
                                  UnitPrice = sale.UnitPrice,
                                  Location = sale.Location.LocationName,
                                  Date = sale.SaleOnDate.Date
                              }).GroupBy(x => x.Date).ToList();


            }
            return collection;
        }

        public static void CreateTable(string pdfFile)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var collection = GetSalesByDate();
            decimal grandTotal = 0;
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document,
                               new FileStream(pdfFile, FileMode.Create));
            document.Open();

            PdfPTable table = new PdfPTable(5);
            CreateCell(table, "Aggregated Sales Report", false, 5, true);

            foreach (var rows in collection)
            {
                CreateCell(table, rows.Key.ToShortDateString(), true, 5);
                CreateCell(table, "ProductName", true, 0, true);

                CreateCell(table, "Quantity", true, 0, true);

                CreateCell(table, "Unit Price", true, 0, true);

                CreateCell(table, "Location", true, 0, true);

                CreateCell(table, "Sum", true, 0, true);

                foreach (var row in rows)
                {
                    CreateCell(table, row.ProductName, false);
                    CreateCell(table, row.Quantity.ToString(), false);
                    CreateCell(table, row.UnitPrice.ToString(), false);
                    CreateCell(table, row.Location, false);
                    CreateCell(table, row.Sum.ToString(), false);
                }

                var totalSum = rows.Sum(x => x.Sum);
                grandTotal += totalSum;
                CreateCell(table, string.Format("Total sum for {0}:  ", rows.Key.ToShortDateString()), false, 4);
                CreateCell(table, totalSum.ToString(), false, 0, true);
                //document.Add(table);
            }

            //PdfPTable grandSumTable = new PdfPTable(5);
            CreateCell(table, "Grand Total:  ", false, 4);
            CreateCell(table, grandTotal.ToString(), false, 0, true);
            document.Add(table);
            document.Close();
        }

        private static void CreateCell(PdfPTable table, string name, bool hasColor = true, int columnSpan = 0, bool isBold = false)
        {
            Cell cell = new Cell();
            Phrase phrase = new Phrase();

            if (isBold)
            {
                Font font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD);
                phrase.Add(new Chunk(name, font));
            }
            else
            {
                phrase.Add(new Chunk(name));
            }

            cell.AddElement(phrase);
            cell.Border = PdfCell.BOTTOM_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.TOP_BORDER;
            if (hasColor)
            {
                cell.BackgroundColor = Color.LIGHT_GRAY;
            }

            if (columnSpan > 0)
            {
                cell.Colspan = columnSpan;
            }

            if (columnSpan == 4)
            {
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            }
            else if (name == "Aggregated Sales Report")
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }

            table.AddCell(cell.CreatePdfPCell());
        }
    }
}
