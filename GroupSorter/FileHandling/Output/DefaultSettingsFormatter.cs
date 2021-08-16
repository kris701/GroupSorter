using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Output
{
    public class DefaultSettingsFormatter : OutputFileHandler<Settings>
    {
        public DefaultSettingsFormatter(Settings settings, string filePath, string seperator) : base(settings, filePath, seperator)
        {
        }

        public override void Format(Settings input)
        {
            var csv = new StringBuilder();

            csv.AppendLine($"Name{Seperator}Value{Seperator}");

            csv.AppendLine($"MinGroupSize{Seperator}{input.MinGroupSize}{Seperator}");
            csv.AppendLine($"MaxGroupSize{Seperator}{input.MaxGroupSize}{Seperator}");
            csv.AppendLine($"NumberOfGroups{Seperator}{input.NumberOfGroups}{Seperator}");
            csv.AppendLine($"OptimiseType{Seperator}{input.OptimiseType}{Seperator}");
            csv.AppendLine($"ShowAll{Seperator}{input.ShowAll}{Seperator}");
            csv.AppendLine($"Runs{Seperator}{input.Runs}{Seperator}");

            WriteToFile(csv.ToString());
        }
    }
}
