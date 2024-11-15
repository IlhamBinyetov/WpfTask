using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace WpfTask
{
    public class XMLLoader : IFileLoader
    {
        public List<TradeData> LoadFile(string filePath)
        {
            var trades = new List<TradeData>();

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            var tradeNodes = xmlDoc.SelectNodes("//Trade");
            if (tradeNodes != null)
            {
                foreach (XmlNode tradeNode in tradeNodes)
                {
                    trades.Add(new TradeData
                    {
                        Date = DateTime.Parse(tradeNode["Date"].InnerText),
                        Open = decimal.Parse(tradeNode["Open"].InnerText, CultureInfo.InvariantCulture),
                        High = decimal.Parse(tradeNode["High"].InnerText, CultureInfo.InvariantCulture),
                        Low = decimal.Parse(tradeNode["Low"].InnerText, CultureInfo.InvariantCulture),
                        Close = decimal.Parse(tradeNode["Close"].InnerText, CultureInfo.InvariantCulture),
                        Volume = int.Parse(tradeNode["Volume"].InnerText)
                    });
                }
            }

            return trades;
        }
    }
}
