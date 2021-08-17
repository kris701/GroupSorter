using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter
{
    public static class ConsoleHelper
    {
        public static void WriteColorLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void WriteColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void PrintLists(List<Group> Groups, int Iteration, bool isAccepted, int satisfactions)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Iteration}\t\t");

            foreach (Group InnerList in Groups)
            {
                sb.Append("[ ");
                foreach (Person p in InnerList.GroupMembers.Values)
                {
                    sb.Append($"{p.ID}, ");
                }
                sb.Append("] ");
            }
            WriteColorLine(sb.ToString(), ConsoleColor.DarkGray);

            Console.Write($"\t\tPossible?: ");
            if (isAccepted)
                WriteColor("TRUE ", ConsoleColor.Green);
            else
                WriteColor("FALSE ", ConsoleColor.Red);

            Console.Write($"Satisfaction: ");
            WriteColor($"{satisfactions} ", ConsoleColor.Blue);

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
            WriteColorLine($"{totalSatesfied} ({totalwishes})", ConsoleColor.Blue);
        }

    }
}
