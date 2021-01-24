using CogentCodingChallenge.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CogentCodingChallenge
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to my coding challenge to find duplicate files!");
            Console.WriteLine("The search is based on the file name, last write time, size, and the actual content. To specify the search mode please " +
                "choose a combination of the following terms: [name, date, size, content] (e.g., 'name content')");

            var scanMode = GetScanMode(Console.ReadLine().ToLower());
            string input;
            do
            {
                Console.WriteLine("Please enter the path you want to search for duplicate files, e.g., 'c:\\root\\my files', or 'quit' to exit.");
                input = Console.ReadLine();
                if (input.ToLower() == "quit")
                    System.Environment.Exit(1);
                if (string.IsNullOrEmpty(input) || !System.IO.Directory.Exists(input))
                    Console.WriteLine("Invalid path, please try again!");
                else
                    break;
            }
            while (true);

            if(scanMode == ScanMode.Name)
                PageOutput(FileScanner<string>.GetDuplicates(input, scanMode));
            else
                PageOutput(FileScanner<FileCompareKey>.GetDuplicates(input, scanMode));

            // Keep the console window open in debug mode.  
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        internal static ScanMode GetScanMode(string input)
        {
            var scanMode = ScanMode.None;
            if (input.Contains("name"))
                scanMode |= ScanMode.Name;
            if (input.Contains("date"))
                scanMode |= ScanMode.Date;
            if (input.Contains("size"))
                scanMode |= ScanMode.Size;
            if (input.Contains("content"))
                scanMode |= ScanMode.Content;
            return scanMode;
        }

        /// <summary>
        /// this function chunks the output into multiple pages
        /// </summary>
        /// <typeparam name="K">key</typeparam>
        /// <typeparam name="V">value</typeparam>
        /// <param name="files">a list of duplicates (files)</param>
        private static void PageOutput<K, V>(IEnumerable<IGrouping<K, V>> files)
        {
            // to be used on user request to break showing more results  
            bool shouldBreak = false;

            // wasting 3 lines for comments at each show  
            int maxLines = Console.WindowHeight - 3;

            // Iterate through the outer collection of groups.  
            foreach (var filegroup in files)
            {
                // each redundant file needs a new view  
                int currentLine = 0;

                // only show parts of the output which can be fit to the window size.
                do
                {
                    Console.Clear();
                    Console.WriteLine("Filename = {0}", filegroup.Key.ToString() == String.Empty ? "[none]" : filegroup.Key.ToString());

                    //skip the lines which have already shown. Since Linq has abilities to execute the query on the fly, it is an efficient 
                    //way to deal with large lists
                    var resultPage = filegroup.Skip(currentLine).Take(maxLines);
                    if (resultPage != null && resultPage.Count() > 0)
                        Console.Write("File locations:");
                    foreach (var fileName in resultPage)
                        Console.WriteLine("\t{0}", fileName);

                    currentLine += maxLines;

                    Console.WriteLine("To see more results please press any key or the 'Escape' key to break...");
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Escape)
                    {
                        shouldBreak = true;
                        break;
                    }
                } while (currentLine < filegroup.Count());

                if (shouldBreak)
                    break;
            }
        }
    }
}
