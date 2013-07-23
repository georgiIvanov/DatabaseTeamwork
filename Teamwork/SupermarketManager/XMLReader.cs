using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SupermarketManager
{
    public class XMLReader
    {
        public static void ReadXml(string fileName)
        {
            // should read and write into the model
            using (XmlReader reader = XmlReader.Create("../../" + fileName))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) &&
                        (reader.Name == "expenses"))
                    {
                        Console.WriteLine(reader.GetAttribute("month"));
                        Console.WriteLine(reader.ReadElementString());
                    }
                }
            }

            //using (XmlReader reader = XmlReader.Create("../../" + fileName))
            //{
            //    while (reader.Read())
            //    {
            //        if (reader.NodeType == XmlNodeType.Element)
            //        {
            //            Console.WriteLine(reader.Name);
            //        }
            //    }
            //}
        }
    }
}
