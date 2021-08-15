using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter
{
    public class Person : ICloneable<Person>
    {
        public Person() { }
        public Person(int iD, string name, int preference1ID, int preference2ID, int preference3ID)
        {
            ID = iD;
            Name = name;
            Preference1ID = preference1ID;
            Preference2ID = preference2ID;
            Preference3ID = preference3ID;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Preference1ID { get; set; } = -1;
        public int Preference2ID { get; set; } = -1;
        public int Preference3ID { get; set; } = -1;
        public int PreferedGroupMembersCount { get; set; }
        public int GotGroupMembersCount { get; set; }

        public Person Clone()
        {
            return new Person(ID, Name, Preference1ID, Preference2ID, Preference3ID);
        }
    }
}
