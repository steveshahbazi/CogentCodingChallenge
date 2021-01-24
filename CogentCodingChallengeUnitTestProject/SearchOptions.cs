using CogentCodingChallenge.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CogentCodingChallengeUnitTestProject
{
    /// <summary>
    /// this class can use any kind of data sample to test the application; I simply used the given test cases
    /// I recommend to use more test cases for a commercial usage
    /// </summary>
    [TestClass]
    public class SearchOptions
    {
        string samplePath = Path.Combine(System.IO.Path.GetFullPath(@"..\..\..\"), "TestSamples");

        [TestMethod]
        public void SearchOnlyBasedOnNames()
        {
            var query = FileScanner<string>.GetDuplicates(samplePath, ScanMode.Name);

            //there are 2 duplicate groups in the test sample based on file name (s-01324.jpg and mew.jpg)
            int expectedDuplicates = 2;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }

        [TestMethod]
        public void SearchOnlyBasedOnContents()
        {
            var query = FileScanner<FileCompareKey>.GetDuplicates(samplePath, ScanMode.Content);

            //there is only 1 duplicate groups in the test sample based on content (mew.jpg)
            int expectedDuplicates = 1;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }

        [TestMethod]
        public void SearchOnlyBasedOnSize()
        {
            var query = FileScanner<FileCompareKey>.GetDuplicates(samplePath, ScanMode.Size);

            //there is only 1 duplicate groups in the test sample based on size
            int expectedDuplicates = 1;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }

        [TestMethod]
        public void SearchOnlyBasedOnDate()
        {
            var query = FileScanner<FileCompareKey>.GetDuplicates(samplePath, ScanMode.Date);

            //there is only no duplicate groups in the test sample based on date (last write time)
            int expectedDuplicates = 0;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }

        [TestMethod]
        public void SearchOnlyBasedOnSizeName()
        {
            var query = FileScanner<FileCompareKey>.GetDuplicates(samplePath, ScanMode.Size | ScanMode.Name);

            //there is only 1 duplicate groups in the test sample based on size and content
            int expectedDuplicates = 1;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }

        [TestMethod]
        public void SearchOnlyBasedOnNameDate()
        {
            var query = FileScanner<FileCompareKey>.GetDuplicates(samplePath, ScanMode.Date | ScanMode.Name);

            //there is only 1 duplicate groups in the test sample based on size and content
            int expectedDuplicates = 0;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }

        [TestMethod]
        public void SearchBasedAll()
        {
            var query = FileScanner<FileCompareKey>.GetDuplicates(samplePath, ScanMode.Date | ScanMode.Name | ScanMode.Content | ScanMode.Size);

            //there is only 1 duplicate groups in the test sample based on size and content
            int expectedDuplicates = 0;
            int duplicateCount = 0;
            foreach (var filegroup in query)
            {
                duplicateCount++;
            }
            Assert.AreEqual(expectedDuplicates, duplicateCount);
        }
    }
}
