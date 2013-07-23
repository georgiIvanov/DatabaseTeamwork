using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SQLStore.Data;
using SQLStore.Model;

namespace SupermarketManager
{
    public static class XMLCreator
    {
        public static void CreateXml(string fileName)
        {
            using (var context = new SQLStoreDb())
            {
                Encoding encoding = Encoding.GetEncoding("utf-8");
                using (XmlTextWriter writer = new XmlTextWriter(fileName, encoding))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.IndentChar = '\t';
                    writer.Indentation = 1;

                    writer.WriteStartDocument();
                    writer.WriteStartElement("sales");

                    foreach (var vendor in context.Vendors.ToList())
                    {
                        WriteSale(context, writer, vendor);
                    }

                    writer.WriteEndDocument();
                }
            }
        }


        private static void WriteSale(SQLStoreDb context, XmlWriter writer, Vendor vendor)
        {
            writer.WriteStartElement("sale");
            writer.WriteAttributeString("vendor", vendor.VendorName);

            var vendorSalesByDate = (from sale in context.Sales
                                     join pr in context.Products
                                     on sale.ProductID equals pr.Id
                                     join v in context.Vendors
                                     on pr.VendorId equals v.Id
                                     where v.VendorName == vendor.VendorName
                                     group sale by sale.SaleOnDate into sod
                                     select new
                                     {
                                         SaleOnDate = sod.Key.Date,
                                         TotalSum = sod.Sum(x => x.Sum)
                                     }
                                    );

            foreach (var v in vendorSalesByDate)
            {
                WriteSummary(writer, v.SaleOnDate, v.TotalSum);
            }

            writer.WriteEndElement();
        }

        private static void WriteSummary(XmlWriter writer, DateTime date, decimal totalSum)
        {
            writer.WriteStartElement("summary");
            writer.WriteAttributeString("total-sum", totalSum.ToString());
            writer.WriteAttributeString("date", date.ToString());

            writer.WriteEndElement();
        }
    }
}
