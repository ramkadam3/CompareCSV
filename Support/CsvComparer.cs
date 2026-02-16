using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareCSV.Support
{
    public class CsvComparer
    {
        private List<string> _keyFields;

        public CsvComparer(List<string> keyFields)
        {
            _keyFields = keyFields;
        }

        public void Compare(string expectedPath, string actualPath, string outputDir)
        {
            var expectedReader = new CsvReader();
            var actualReader = new CsvReader();

            expectedReader.Read(expectedPath, _keyFields);
            actualReader.Read(actualPath, _keyFields);

            if (expectedReader.Headers.Length != actualReader.Headers.Length)
                throw new Exception("Header count mismatch.");
            for (int i = 0; i < expectedReader.Headers.Length; i++)
            {
                if (expectedReader.Headers[i] != actualReader.Headers[i])
                    throw new Exception($"Header mismatch at position {i}: {expectedReader.Headers[i]} vs {actualReader.Headers[i]}");
            }

            var expectedDict = new Dictionary<string, CsvRecord>();
            foreach (var rec in expectedReader.Records)
                expectedDict[rec.Key] = rec;

            var actualDict = new Dictionary<string, CsvRecord>();
            foreach (var rec in actualReader.Records)
                actualDict[rec.Key] = rec;

            var missingInActual = new List<CsvRecord>();
            foreach (var key in expectedDict.Keys)
            {
                if (!actualDict.ContainsKey(key))
                    missingInActual.Add(expectedDict[key]);
            }

            var missingInExpected = new List<CsvRecord>();
            foreach (var key in actualDict.Keys)
            {
                if (!expectedDict.ContainsKey(key))
                    missingInExpected.Add(actualDict[key]);
            }

            var fieldMismatches = new List<string>();
            foreach (var key in expectedDict.Keys)
            {
                if (actualDict.ContainsKey(key))
                {
                    var expectedRec = expectedDict[key];
                    var actualRec = actualDict[key];
                    foreach (var field in expectedRec.Fields.Keys)
                    {
                        if (_keyFields.Contains(field)) continue;
                        if (expectedRec.Fields[field] != actualRec.Fields[field])
                        {
                            fieldMismatches.Add(
                                $"Failed Fieldname: {field} | Expected Input Value: \"{expectedRec.Fields[field]}\" | Actual Value: \"{actualRec.Fields[field]}\" | for record having unique field(s): {string.Join(", ", _keyFields)} with value: \"{key}\""
                            );
                        }
                    }
                }
            }

            Directory.CreateDirectory(outputDir);
            WriteRecordsToFile(missingInActual, Path.Combine(outputDir, "MissingInActual.txt"));
            WriteRecordsToFile(missingInExpected, Path.Combine(outputDir, "MissingInExpected.txt"));
            File.WriteAllLines(Path.Combine(outputDir, "FieldMismatches.txt"), fieldMismatches);
        }

        private void WriteRecordsToFile(List<CsvRecord> records, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var rec in records)
                {
                    writer.WriteLine(string.Join(",", rec.Fields.Values));
                }
            }
        }
    }
}

