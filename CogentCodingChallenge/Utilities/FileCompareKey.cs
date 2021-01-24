using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CogentCodingChallenge.Utilities
{
    public class FileCompareKey
    {
        public string Name { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Length { get; set; }
        public ScanMode ScanMode { get; set; }
        public string FullPath { get; set; }

        public override bool Equals(object obj)
        {
            FileCompareKey other = (FileCompareKey)obj;
            bool result = true;
            if ((this.ScanMode & ScanMode.Name) == ScanMode.Name)
                result = other.Name == this.Name;
            if (result && (this.ScanMode & ScanMode.Date) == ScanMode.Date)
                result = other.LastWriteTime == this.LastWriteTime;
            if (result && (this.ScanMode & ScanMode.Size) == ScanMode.Size)
                result = other.Length == this.Length;
            if (result && (this.ScanMode & ScanMode.Content) == ScanMode.Content)
                result = FileCompareByteToByte(other.FullPath, this.FullPath);
            return result;
        }

        public override int GetHashCode()
        {
            string str = $"{this.LastWriteTime}{this.Length}{this.Name}";
            return str.GetHashCode();
        }
        public override string ToString()
        {
            return $"{this.Name} {this.Length} {this.LastWriteTime}";
        }

        // This method accepts two strings the represent two files to
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the
        // files are not the same.
        private bool FileCompareByteToByte(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is
            // equal to "file2byte" at this point only if the files are
            // the same.
            return ((file1byte - file2byte) == 0);
        }
    }
}
