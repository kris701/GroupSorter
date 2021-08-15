using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Input
{
    public abstract class InputFileHandler<T>
    {
        public Settings Settings { get; set; }
        public string FilePath { get; set; }
        public string Seperator { get; set; }

        public InputFileHandler(Settings settings, string filePath, string seperator)
        {
            Settings = settings;
            FilePath = filePath;
            Seperator = seperator;
        }

        public List<string[]> GetCSVFile()
        {
            List<string[]> returnList = new List<string[]>();

            try
            {
                if (!File.Exists(FilePath))
                    throw new FileNotFoundException($"The requested file ({FilePath}) was not found!");

                string[] lines = File.ReadAllLines(FilePath, Encoding.UTF8);

                foreach (string s in lines)
                {
                    returnList.Add(s.Split(Seperator));
                }
            } catch {
                ConsoleHelper.WriteColorLine("Error in reading the CSV file! Either the file is in use, missing or incorrectly formatted!", ConsoleColor.Red);
                ConsoleHelper.WriteColorLine("\t Detailed error:", ConsoleColor.Red);
                throw;
            }

            return returnList;
        }

        public abstract T Format();

        public bool IsDigitsOnly(string input)
        {
            if (input == "")
                return false;
            foreach (char c in input)
                if (c <= '0' && c >= '9')
                    return false;
            return true;
        }
    }
}
