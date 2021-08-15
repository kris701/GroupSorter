using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Input
{
    public class PersonFormatter : InputFileHandler<Dictionary<int, Person>>
    {
        public PersonFormatter(Settings settings, string filePath, string seperator) : base(settings, filePath, seperator)
        {
        }

        public override Dictionary<int, Person> Format()
        {
            Dictionary<int, Person> outList = new Dictionary<int, Person>();

            try { 
                int ID = 0;
                foreach (string[] s in GetCSVFile())
                {
                    if (ID > 0)
                    {
                        Person newPerson = new Person();
                        newPerson.ID = Int32.Parse(s[0]);
                        newPerson.Name = s[1];
                        if (IsDigitsOnly(s[2]))
                        {
                            newPerson.Preference1ID = Int32.Parse(s[2]);
                            newPerson.PreferedGroupMembersCount++;
                        }
                        if (IsDigitsOnly(s[3]))
                        {
                            newPerson.Preference2ID = Int32.Parse(s[3]);
                            newPerson.PreferedGroupMembersCount++;
                        }
                        if (IsDigitsOnly(s[4]))
                        {
                            newPerson.Preference3ID = Int32.Parse(s[4]);
                            newPerson.PreferedGroupMembersCount++;
                        }

                        outList.Add(newPerson.ID, newPerson);
                    }
                    ID++;
                }
            }
            catch
            {
                ConsoleHelper.WriteColorLine("Error in processing the input CSV file data! Some data in the file might be incorrect or corrupt!", ConsoleColor.Red);
                ConsoleHelper.WriteColorLine("\t Detailed error:", ConsoleColor.Red);
                throw;
            }

            return outList;
        }
    }
}
