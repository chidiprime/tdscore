using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDS.Api.Utility
{
    public class CSVHelper<T>
    {
        public static MemoryStream Download(List<T> data)
        {
            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using (var cw = new CsvWriter(sw, cc))
                    {
                        cw.WriteRecords(data);
                    }// The stream gets flushed here.
                   // return File(ms.ToArray(), "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
                }
                return ms;
            }
        }
    }
}
