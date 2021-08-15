using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GroupSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            GroupSorter groupSorter = new GroupSorter();
            groupSorter.Run();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
