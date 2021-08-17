using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Output
{
    public class DefaultInputCSVFormatterr : OutputFileHandler<int>
    {
        public DefaultInputCSVFormatterr(Settings settings, string filePath, string seperator) : base(settings, filePath, seperator)
        {
        }

        public override void Format(int input = 0)
        {
            var csv = new StringBuilder();

            csv.AppendLine($"Id{Seperator}Name{Seperator}Preference1{Seperator}Preference2{Seperator}Preference3{Seperator}");
            csv.AppendLine($"0{Seperator}a{Seperator}1{Seperator}2{Seperator}{Seperator}");
            csv.AppendLine($"1{Seperator}b{Seperator}0{Seperator}{Seperator}{Seperator}");
            csv.AppendLine($"2{Seperator}c{Seperator}1{Seperator}{Seperator}{Seperator}");

            WriteToFile(csv.ToString());
        }
    }
}
