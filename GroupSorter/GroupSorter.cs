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
        public List<Task> TaskList { get; set; } = new List<Task>();
        public Dictionary<int, Person> ListOfPeople { get; internal set; }
        public List<Group> BestCombination = new List<Group>();
        public int BestSatisfaction { get; set; } = 0;
        public bool IsAnyFound { get; internal set; }
        public int Iterations { get; internal set; }
        public int TotalFinished { get; internal set; }
        public List<int> LatestFPS { get; internal set; } = new List<int>(new int[10]);
        public int SecondsPassed { get; internal set; }

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
            var FPSwatch = System.Diagnostics.Stopwatch.StartNew();
            int FPSNumberOfIterations = 1;

            Console.Title = "Group Sorter v. 1.1";
            Console.WriteLine(" === All possible combinations === ");

            for (int i = 0; i < Settings.Threads; i++)
            {
                TaskList.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < Settings.Runs; j++)
                    {
                        Dictionary<int, Person> keyValuePairs = new Dictionary<int, Person>(ListOfPeople);

                        Random rand = new Random();
                        keyValuePairs = keyValuePairs.OrderBy(x => rand.Next()).ToDictionary(item => item.Key, item => item.Value);
                        GroupSplitter newSplitter = new GroupSplitter(keyValuePairs, Settings);
                        newSplitter.SplitGroups();
                        Iterations += newSplitter.Iterations;
                        if (newSplitter.BestSatisfaction > BestSatisfaction)
                        {
                            IsAnyFound = true;
                            BestSatisfaction = newSplitter.BestSatisfaction;
                            BestCombination = newSplitter.BestCombination;
                            ConsoleHelper.PrintLists(BestCombination, Iterations, true, BestSatisfaction);
                        }
                        TotalFinished++;
                    }
                }));
            }

            while(TaskList.Any(x => !x.IsCompleted))
            {
                if (FPSwatch.ElapsedMilliseconds >= 1000)
                {
                    Console.Title = $"Average FPS: {AvrOfList(LatestFPS)}, Finished: {((float)TotalFinished / (float)(Settings.Runs * Settings.Threads)) * 100}%";
                    LatestFPS[SecondsPassed % 10] = ((Iterations - FPSNumberOfIterations));
                    FPSNumberOfIterations = Iterations;
                    FPSwatch.Restart();
                    SecondsPassed++;
                }
            }

            Console.Title = "Group Sorter v. 1.1";

            //GetSatisfaction(BestCombination);

            BestGroupOutputFormatter bestGroupOutputFormatter = new BestGroupOutputFormatter(Settings, CSVOutputFilePath, ";");
            bestGroupOutputFormatter.Format(BestCombination);

            Console.WriteLine();

            if (IsAnyFound)
            {
                Console.WriteLine($" === Best combination possible out of {Iterations} iterations === ");
                ConsoleHelper.PrintLists(BestCombination, 0, true, BestSatisfaction);
            }
            else
                Console.WriteLine("No possible combination found! Either adjust your settings or people list!");
        }

        private int AvrOfList(List<int> list)
        {
            int total = 0;
            foreach (int i in list)
                total += i;
            return total / list.Count;
        }
    }
}
