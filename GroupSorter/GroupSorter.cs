using GroupSorter.FileHandling;
using GroupSorter.FileHandling.Output;
using GroupSorter.FileHandling.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GroupSorter
{
    public class GroupSorter
    {
        public Dictionary<int, Person> ListOfPeople { get; internal set; }
        public List<Group> BestCombination = new List<Group>();
        public int BestSatisfaction { get; set; } = 0;
        public bool IsAnyFound { get; internal set; }
        public int Iterations { get; internal set; }

        public string CSVInputFilePath { get; } = "input.csv";
        public string CSVOutputFilePath { get; } = "output.csv";
        public string SettingsFilePath { get; } = "settings.csv";
        public Settings Settings { get; set; }

        public GroupSorter()
        {
            if (!File.Exists(SettingsFilePath))
            {
                ConsoleHelper.WriteColorLine("Settigns file not found! Making default", ConsoleColor.Yellow);
                DefaultSettingsFormatter defaultSettingsFormatter = new DefaultSettingsFormatter(Settings, SettingsFilePath, ";");
                Settings = new Settings();
                defaultSettingsFormatter.Format(Settings);
            }
            if (!File.Exists(CSVInputFilePath))
            {
                ConsoleHelper.WriteColorLine("Input file not found! Making default", ConsoleColor.Yellow);
                DefaultInputCSVFormatterr defaultInput = new DefaultInputCSVFormatterr(Settings, CSVInputFilePath, ";");
                defaultInput.Format();
            }
            SettingsFormatter settingsFormatter = new SettingsFormatter(Settings, SettingsFilePath, ";");
            Settings = settingsFormatter.Format();
            PersonFormatter personFormatter = new PersonFormatter(Settings, CSVInputFilePath, ";");
            ListOfPeople = personFormatter.Format();
        }

        public void Run()
        {
            Console.WriteLine(" === All possible combinations === ");

            SplitGroups();

            GetSatisfaction(BestCombination);

            BestGroupOutputFormatter bestGroupOutputFormatter = new BestGroupOutputFormatter(Settings, CSVOutputFilePath, ";");
            bestGroupOutputFormatter.Format(BestCombination);

            Console.WriteLine();

            if (IsAnyFound)
            {
                Console.WriteLine($" === Best combination possible out of {Iterations} iterations === ");
                PrintLists(BestCombination, 0);
            }
            else
                Console.WriteLine("No possible combination found! Either adjust your settings or people list!");
        }

        private void SplitGroups()
        {
            var FPSwatch = System.Diagnostics.Stopwatch.StartNew();
            int FPSNumberOfIterations = 1;

            for (int i = 0; i < Settings.Runs; i++)
            {
                Random rand = new Random();
                ListOfPeople = ListOfPeople.OrderBy(x => rand.Next())
                  .ToDictionary(item => item.Key, item => item.Value);

                List<Group> baseGroup = GenerateLeftMostGroup();
                int groupIndex = baseGroup.Count - 1;

                while (baseGroup[0].GroupMembers.Count != ListOfPeople.Count - Settings.NumberOfGroups + 1)
                {
                    // Move to the group index with more than 1 member in left
                    // And that the group below has more than 1 member left
                    while (baseGroup[groupIndex].GroupMembers.Count == 1 && baseGroup[groupIndex - 1].GroupMembers.Count == 1)
                        groupIndex--;

                    // Check if current indexed group has more than one member
                    // If it does, move that member down, and run next itteration
                    if (baseGroup[groupIndex].GroupMembers.Count != 1)
                    {
                        MovePersonToGroup(baseGroup[groupIndex], baseGroup[groupIndex - 1], baseGroup[groupIndex].GroupMembers.Keys.First());
                    }
                    else
                    {
                        // If then there is only 1 member left in the group, regroup by moving one member down and the rest to the first group
                        MovePersonToGroup(baseGroup[groupIndex - 1], baseGroup[groupIndex - 2], baseGroup[groupIndex - 1].GroupMembers.Keys.First());
                        while (groupIndex != baseGroup.Count)
                        {
                            while (baseGroup[groupIndex - 1].GroupMembers.Count != 1)
                                MovePersonToGroup(baseGroup[groupIndex - 1], baseGroup[groupIndex], baseGroup[groupIndex - 1].GroupMembers.Keys.Last());
                            groupIndex++;
                        }
                    }
                    // Reset the group index
                    groupIndex = baseGroup.Count - 1;

                    // Set best group if this current configuration is better than the one that is saved
                    CheckAndSetBestGroup(baseGroup);

                    if (Settings.ShowAll)
                        PrintLists(baseGroup, Iterations);

                    Iterations++;

                    if (FPSwatch.ElapsedMilliseconds >= 1000)
                    {
                        Console.Title = "Frames per Second: " + (Iterations - FPSNumberOfIterations);
                        FPSNumberOfIterations = Iterations;
                        FPSwatch.Restart();
                    }
                }
            }
        }

        private List<Group> GenerateLeftMostGroup() {
            Dictionary<int, Person> keyValuePairs = new Dictionary<int, Person>(ListOfPeople);
            List<Group> baseGroup = new List<Group>();
            for (int i = 0; i < Settings.NumberOfGroups; i++)
            {
                Group newGroup = new Group();

                newGroup.GroupMembers.Add(keyValuePairs.First().Value.ID, keyValuePairs.First().Value);
                keyValuePairs.Remove(keyValuePairs.First().Value.ID);
                baseGroup.Add(newGroup);
            }
            for (int i = Settings.NumberOfGroups; i < ListOfPeople.Count; i++)
            {
                baseGroup[Settings.NumberOfGroups - 1].GroupMembers.Add(keyValuePairs.First().Value.ID, keyValuePairs.First().Value);
                keyValuePairs.Remove(keyValuePairs.First().Value.ID);
            }

            return baseGroup;
        }

        private void CheckAndSetBestGroup(List<Group> baseGroup)
        {
            if (GetSatisfaction(baseGroup) > BestSatisfaction)
            {
                if (IsGroupsAccepted(baseGroup))
                {
                    BestCombination = baseGroup.ConvertAll(x => x.Clone());
                    BestSatisfaction = GetSatisfaction(BestCombination);
                    if (!Settings.ShowAll)
                        PrintLists(baseGroup, Iterations);
                    IsAnyFound = true;
                }
            }
        }

        private void MovePersonToGroup(Group fromGroup, Group toGroup, int personID)
        {
            toGroup.GroupMembers.Add(personID, fromGroup.GroupMembers[personID]);
            fromGroup.GroupMembers.Remove(personID);
        }

        private int GetSatisfaction(List<Group> groups)
        {
            int returnValue = 0;

            foreach (Group group in groups)
            {
                group.CalculateGroupSatisfaction();
                returnValue += ReduceByOptimalGroupSize(group);
            }

            return returnValue;
        }

        private int ReduceByOptimalGroupSize(Group group)
        {
            switch (Settings.OptimiseType)
            {
                case Settings.OptimiseTypes.EqualiseGroups: return (int)((float)group.GroupSatisfaction * ((float)group.GroupMembers.Count / (float)Settings.OptimalGroupSize));
            }
            return group.GroupSatisfaction;
        }

        private bool IsGroupsAccepted(List<Group> groups)
        {
            foreach (Group group in groups)
            {
                if (group.GroupMembers.Count < Settings.MinGroupSize)
                    return false;
                if (group.GroupMembers.Count > Settings.MaxGroupSize)
                    return false;
            }
            return true;
        }

        private void PrintLists(List<Group> Groups, int Itteration)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Itteration}\t\t");

            foreach (Group InnerList in Groups)
            {
                sb.Append("[ ");
                foreach (Person p in InnerList.GroupMembers.Values)
                {
                    sb.Append($"{p.ID}, ");
                }
                sb.Append("] ");
            }
            ConsoleHelper.WriteColorLine(sb.ToString(), ConsoleColor.DarkGray);

            Console.Write($"\t\tPossible?: ");
            if (IsGroupsAccepted(Groups))
                ConsoleHelper.WriteColor("TRUE ", ConsoleColor.Green);
            else
                ConsoleHelper.WriteColor("FALSE ", ConsoleColor.Red);

            Console.Write($"Satisfaction: ");
            ConsoleHelper.WriteColor($"{GetSatisfaction(Groups)} ", ConsoleColor.Blue);

            Console.Write($"Total People that got wishes granted (out of): ");
            int totalSatesfied = 0;
            int totalwishes = 0;
            foreach (Group group in Groups)
            {
                foreach (Person person in group.GroupMembers.Values)
                {
                    totalSatesfied += person.GotGroupMembersCount;
                    totalwishes += person.PreferedGroupMembersCount;
                }
            }
            ConsoleHelper.WriteColorLine($"{totalSatesfied} ({totalwishes})", ConsoleColor.Blue);
        }
    }
}
