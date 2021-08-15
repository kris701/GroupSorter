using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Output
{
    public abstract class OutputFileHandler<T>
    {
        public Settings Settings { get; set; }
        public string FilePath { get; set; }
        public string Seperator { get; set; }

        protected OutputFileHandler(Settings settings, string filePath, string seperator)
        {
            Settings = settings;
            FilePath = filePath;
            Seperator = seperator;
        }

        public void WriteToFile(string text)
        {
            try {
                File.WriteAllText(FilePath, text);
            }
            catch
            {
                ConsoleHelper.WriteColorLine("Error in outputting to CSV file! The file might be in use!", ConsoleColor.Red);
                ConsoleHelper.WriteColorLine("\t Detailed error:", ConsoleColor.Red);
                throw;
            }
        }

        public abstract void Format(T input);
    }
}
