using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareCSV.Support
{
    public class CsvRecord
    {
        public Dictionary<string, string> Fields { get; set; }
        public string Key { get; set; }
    }
}
