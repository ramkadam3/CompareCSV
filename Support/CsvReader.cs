using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareCSV.Support
{
    public class CsvReader
    {
        public string[] Headers { get; private set; }
        public List<CsvRecord> Records { get; private set; } = new List<CsvRecord>();

        public void Read(string filePath, List<string> keyFields)
        {
            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                if (headerLine == null)
                    throw new Exception("CSV file is empty.");

                Headers = headerLine.Split(',');

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    if (values.Length != Headers.Length)
                        throw new Exception("Field count mismatch in line: " + line);

                    var record = new CsvRecord { Fields = new Dictionary<string, string>() };
                    for (int i = 0; i < Headers.Length; i++)
                    {
                        record.Fields[Headers[i]] = values[i];
                    }
                    record.Key = string.Join("|", keyFields.ConvertAll(f => record.Fields[f]));
                    Records.Add(record);
                }
            }
        }
    }
}

