using Photography.Data;
using Photography.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Photography.XmlImport
{
    

    class XmlImport
    {
        private static string AccessoriesPath = "../../../datasets/accessories.xml";
        private static string Error = "Error: Invalid data.";

        static void Main(string[] args)
        {
            UnitOfWork unit = new UnitOfWork();
            XmlImportAccessories(unit);
        }

        private static void XmlImportAccessories(UnitOfWork unit)
        {
            XDocument document = XDocument.Load(AccessoriesPath);
            var accessoriesXmls = document.Descendants("accessory");
            foreach (XElement accessoryXml in accessoriesXmls)
            {
                var accessoryNameAttr = accessoryXml.Attribute("name");

                if (accessoryNameAttr == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                Random rnd = new Random();

                Accessory accessory = new Accessory()
                {
                    Name = accessoryNameAttr.Value,
                    OwnerId = rnd.Next(1, unit.Photographers.Count() + 1)
                };

                unit.Accessories.Add(accessory);
                unit.Commit();
                Console.WriteLine($"Successfully imported {accessory.Name}");
            }
        }
    }
}
