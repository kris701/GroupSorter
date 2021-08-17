using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter
{
    public class Settings
    {
        public enum OptimiseTypes { None, EqualiseGroups };

        public int NumberOfGroups { get; set; } = 4;
        public int MinGroupSize { get; set; } = 2;
        public int MaxGroupSize { get; set; } = 7;
        public int OptimalGroupSize { get; set; } = 5;
        public OptimiseTypes OptimiseType { get; set; } = OptimiseTypes.EqualiseGroups;
        public int Runs { get; set; } = 10;
        public int Threads { get; set; } = 8;
    }
}
