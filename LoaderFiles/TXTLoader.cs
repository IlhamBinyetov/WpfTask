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
    public class TXTLoader : IFileLoader
    {
        public List<TradeData> LoadFile(string filePath)
        {
            var data = new List<TradeData>();
            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) 
            {
                var columns = lines[i].Split(';');

                
                DateTime parsedDate;
                if (DateTime.TryParse(columns[0], out parsedDate))
                {
                    decimal parsedOpen, parsedHigh, parsedLow, parsedClose;
                    int parsedVolume;

                    
                    bool isOpenValid = decimal.TryParse(columns[1], NumberStyles.Any, CultureInfo.InvariantCulture, out parsedOpen);
                    bool isHighValid = decimal.TryParse(columns[2], NumberStyles.Any, CultureInfo.InvariantCulture, out parsedHigh);
                    bool isLowValid = decimal.TryParse(columns[3], NumberStyles.Any, CultureInfo.InvariantCulture, out parsedLow);
                    bool isCloseValid = decimal.TryParse(columns[4], NumberStyles.Any, CultureInfo.InvariantCulture, out parsedClose);
                    bool isVolumeValid = int.TryParse(columns[5], out parsedVolume);

                    
                    if (isOpenValid && isHighValid && isLowValid && isCloseValid && isVolumeValid)
                    {
                        data.Add(new TradeData
                        {
                            Date = parsedDate,
                            Open = parsedOpen,
                            High = parsedHigh,
                            Low = parsedLow,
                            Close = parsedClose,
                            Volume = parsedVolume
                        });
                    }
                    else
                    {
                        
                        Console.WriteLine($"Geçersiz veri satırı: {lines[i]}");
                    }
                }
                else
                {
                    
                    Console.WriteLine($"Geçersiz tarih formatı: {columns[0]}");
                }
            }
            return data;
        }
    }
}
