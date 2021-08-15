using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Output
{
    public class BestGroupOutputFormatter : OutputFileHandler<List<Group>>
    {
        public BestGroupOutputFormatter(Settings settings, string filePath, string seperator) : base(settings, filePath, seperator)
        {
        }

        public override void Format(List<Group> input)
        {
            var csv = new StringBuilder();

            int groupNr = 0;
            csv.AppendLine($"GroupNumber{Seperator}Name{Seperator}PreferedGroupMembers{Seperator}GotGroupMembers{Seperator}");

            foreach (Group group in input)
            {
                foreach (Person person in group.GroupMembers.Values)
                {
                    csv.AppendLine($"{groupNr}{Seperator}{person.Name}{Seperator}{person.PreferedGroupMembersCount}{Seperator}{person.GotGroupMembersCount}{Seperator}");
                }
                csv.AppendLine("");
                groupNr++;
            }

            WriteToFile(csv.ToString());
        }
    }
}
