using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CogentCodingChallenge.Utilities
{
    [Flags]
    public enum ScanMode
    {
        None = 0,
        Name = 1,
        Size = 2,
        Date = 3,
        Content = 8
    }
    public class FileScanner<T>
    {
        public static IEnumerable<IGrouping<T, string>> GetDuplicates(string rootPath, ScanMode mode)
        {
            if (mode == ScanMode.Name)
                return (IEnumerable<IGrouping<T, string>>)GetDuplicatesBasedOnNames(rootPath);
            return (IEnumerable<IGrouping<T, string>>)GetDuplicatesBasedOnSize(rootPath, mode);
        }

        static IEnumerable<IGrouping<string, string>> GetDuplicatesBasedOnNames(string rootPath)
        {
            var dir = new DirectoryInfo(rootPath);
            IEnumerable<FileInfo> files = dir.GetFiles("*.*", SearchOption.AllDirectories);

            // this is due to the limitatin of the app to force showing each file path in maximum one line 
            int charsToSkip = rootPath.Length;

            IEnumerable<IGrouping<string, string>> dupFileNames =
                from file in files
                group file.FullName[charsToSkip..] by file.Name into fileGroup
                where fileGroup.Count() > 1 //more than 1 means there is a duplicate
                select fileGroup;

            return dupFileNames;
        }

        static IEnumerable<IGrouping<FileCompareKey, string>> GetDuplicatesBasedOnSize(string rootPath, ScanMode mode)
        {
            var dir = new DirectoryInfo(rootPath);
            IEnumerable<FileInfo> files = dir.GetFiles("*.*", SearchOption.AllDirectories);

            // this is due to the limitatin of the app to force showing each file path in maximum one line 
            int charsToSkip = rootPath.Length;

            var dupFileNames =
                from file in files
                group file.FullName.Substring(charsToSkip) by
                    new FileCompareKey { Name = file.Name, LastWriteTime = file.LastWriteTime, Length = file.Length,
                        ScanMode = mode, FullPath = file.FullName } into fileGroup
                where fileGroup.Count() > 1 //more than 1 means there is a duplicate
                select fileGroup;

            return dupFileNames;
        }
    }
}
