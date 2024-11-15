using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTask
{
    public interface IFileLoader
    {
        List<TradeData> LoadFile(string filePath);
    }
}
