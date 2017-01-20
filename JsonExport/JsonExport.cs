using Newtonsoft.Json;
using Photography.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonExport
{
    class JsonExport
    {
        static void Main(string[] args)
        {
            UnitOfWork unit = new UnitOfWork();
            ExportOrderedPhotographers(unit);
            ExportLendscapePhotographers(unit);
        }

        private static void ExportLendscapePhotographers(UnitOfWork unit)
        {
            var photographer = unit.Photographers.GetAll()
                .OrderBy(photogr => photogr.FirstName)
                .Where(photo => photo.PrimaryCamera.Type == "DSLR")
                .Select(ph => new
                {
                    FirstName = ph.FirstName,
                    LastName = ph.LastName,
                    CameraMake = ph.PrimaryCamera.Make,
                    LensesCount = ph.Lenses.Count
                });

            string json = JsonConvert.SerializeObject(photographer, Formatting.Indented);
            File.WriteAllText("../../../results/landscape-photographers.json", json);
        }

        private static void ExportOrderedPhotographers(UnitOfWork unit)
        {
            var photographer = unit.Photographers.GetAll()
                .OrderBy(photog => photog.FirstName)
                .ThenByDescending(photog => photog.LastName)
                .Select(photogr => new
                {
                    FirstName = photogr.FirstName,
                    LastName = photogr.LastName,
                    Phone = photogr.Phone
                });

            string json = JsonConvert.SerializeObject(photographer, Formatting.Indented);
            File.WriteAllText("../../../results/photographers-ordered.json", json);
        }
    }
}