using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTask.LoaderFiles
{
    public class CSVLoader : IFileLoader
    {
        public List<TradeData> LoadFile(string filePath)
        {
            var data = new List<TradeData>();
            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++) 
            {
                var columns = lines[i].Split(',');

                data.Add(new TradeData
                {
                    Date = DateTime.Parse(columns[0]),
                    Open = decimal.Parse(columns[1], CultureInfo.InvariantCulture),
                    High = decimal.Parse(columns[2], CultureInfo.InvariantCulture),
                    Low = decimal.Parse(columns[3], CultureInfo.InvariantCulture),
                    Close = decimal.Parse(columns[4], CultureInfo.InvariantCulture),
                    Volume = int.Parse(columns[5])
                });
            }

            return data;
        }
    }
}
