using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareCSV.Support;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecFlow;
namespace CompareCSV.StepDefinitions
{   
    [Binding]
    public class CsvComparisonSteps
    {
        private string _expectedPath;
        private string _actualPath;
        private string _outputDir;
        private List<string> _keyFields;

        [Given(@"I have an expected CSV file ""(.*)""")]
        public void GivenIHaveAnExpectedCSVFile(string fileName)
        {
            _expectedPath = fileName;
        }

        [Given(@"I have an actual CSV file ""(.*)""")]
        public void GivenIHaveAnActualCSVFile(string fileName)
        {
            _actualPath = fileName;
        }

        [When(@"I compare the files using primary key fields ""(.*)""")]
        public void WhenICompareTheFilesUsingPrimaryKeyFields(string keyFields)
        {
            _keyFields = new List<string>(keyFields.Split(','));
            _outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Output"); 
            _outputDir = Path.GetFullPath(_outputDir);
            Directory.CreateDirectory(_outputDir);
            var comparer = new CsvComparer(_keyFields);
            comparer.Compare(_expectedPath, _actualPath, _outputDir);
        }

        [Then(@"I should see the comparison results in the output directory ""(.*)""")]
        public void ThenIShouldSeeTheComparisonResults(string outputDir)
        {
            Assert.IsTrue(File.Exists(Path.Combine(outputDir, "MissingInActual.txt")));
            Assert.IsTrue(File.Exists(Path.Combine(outputDir, "MissingInExpected.txt")));
            Assert.IsTrue(File.Exists(Path.Combine(outputDir, "FieldMismatches.txt")));
        }
    }
}
