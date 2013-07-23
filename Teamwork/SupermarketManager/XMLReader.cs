using System;
using System.Linq;
using System.Xml;
using SQLStore.Data;
using SQLStore.Model;

namespace SupermarketManager
{
    public class XMLReader
    {
        public static void ReadXml(string fileName)
        {
            using (var context = new SQLStoreDb())
            {
                using (XmlReader reader = XmlReader.Create(string.Format("../../{0}", fileName)))
                {
                    Vendor vendor = new Vendor();
                    while (reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) &&
                            (reader.Name == "sale"))
                        {
                            string vendorName = reader.GetAttribute("vendor");
                            vendor = context.Vendors.Where(x => x.VendorName == vendorName).First();
                        }
                        else if (reader.NodeType == XmlNodeType.Element &&
                                 reader.Name == "expenses")
                        {
                            Month month = new Month();
                            month.MonthDate = DateTime.Parse(reader.GetAttribute("month"));
                            if (!context.Months.Any(x => x.MonthDate == month.MonthDate))
                            {
                                context.Months.Add(month);
                                context.SaveChanges();
                            }

                            VendorMonth vendorMonth = new VendorMonth();
                            vendorMonth.VendorID = vendor.Id;
                            vendorMonth.MonthID = context.Months.Where(x => x.MonthDate == month.MonthDate).First().MonthId;
                            vendorMonth.Expenses = decimal.Parse(reader.ReadElementString());
                            context.VendorMonths.Add(vendorMonth);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
