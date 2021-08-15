using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter.FileHandling.Input
{
    public class SettingsFormatter : InputFileHandler<Settings>
    {
        public SettingsFormatter(Settings settings, string filePath, string seperator) : base(settings, filePath, seperator)
        {
        }

        public override Settings Format()
        {
            Settings newSettings = new Settings();

            try
            {
                int ID = 0;
                foreach (string[] s in GetCSVFile())
                {
                    if (ID > 0)
                    {
                        if (s[0].ToUpper() == "MINGROUPSIZE")
                            newSettings.MinGroupSize = Int32.Parse(s[1]);
                        if (s[0].ToUpper() == "MAXGROUPSIZE")
                            newSettings.MaxGroupSize = Int32.Parse(s[1]);
                        if (s[0].ToUpper() == "NUMBEROFGROUPS")
                            newSettings.NumberOfGroups = Int32.Parse(s[1]);
                        if (s[0].ToUpper() == "OPTIMALGROUPSIZE")
                            newSettings.OptimalGroupSize = Int32.Parse(s[1]);
                        if (s[0].ToUpper() == "OPTIMISETYPE")
                        {
                            switch (s[1].ToUpper())
                            {
                                case "EQUALISEGROUPS": newSettings.OptimiseType = Settings.OptimiseTypes.EqualiseGroups; break;
                                default: newSettings.OptimiseType = Settings.OptimiseTypes.None; break;
                            }
                        }
                        if (s[0].ToUpper() == "SHOWALL")
                        {
                            if (s[1].ToUpper() == "TRUE")
                                newSettings.ShowAll = true;
                            else
                                newSettings.ShowAll = false;
                        }
                    }
                    ID++;
                }
            }
            catch
            {
                ConsoleHelper.WriteColorLine("Error in processing the settings CSV file data! Some data in the file might be incorrect or corrupt!", ConsoleColor.Red);
                ConsoleHelper.WriteColorLine("\t Detailed error:", ConsoleColor.Red);
                throw;
            }

            return newSettings;
        }
    }

}
