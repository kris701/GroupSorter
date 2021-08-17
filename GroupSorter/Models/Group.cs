using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter
{
    public class Group : ICloneable<Group>
    {
        public Group(Settings settings) {
            Settings = settings;
        }

        public Group(Dictionary<int, Person> groupMembers, int groupSatisfaction, Settings settings) {
            GroupMembers = groupMembers;
            GroupSatisfaction = groupSatisfaction;
            Settings = settings;
        }

        public Settings Settings { get; internal set; }
        public Dictionary<int, Person> GroupMembers { get; set; } = new Dictionary<int, Person>();
        public int GroupSize { get => GroupMembers.Count; }
        public int GroupSatisfaction { get; internal set; }
        public bool Changed { get; set; }
        public int CalculateGroupSatisfaction()
        {
            if (Changed)
            {
                GroupSatisfaction = 0;
                foreach (Person person in GroupMembers.Values)
                {
                    person.GotGroupMembersCount = 0;
                    if (GroupMembers.ContainsKey(person.Preference1ID))
                    {
                        GroupSatisfaction += 3;
                        person.GotGroupMembersCount++;
                    }
                    if (GroupMembers.ContainsKey(person.Preference2ID))
                    {
                        GroupSatisfaction += 2;
                        person.GotGroupMembersCount++;
                    }
                    if (GroupMembers.ContainsKey(person.Preference3ID))
                    {
                        GroupSatisfaction += 1;
                        person.GotGroupMembersCount++;
                    }
                }
                GroupSatisfaction = ReduceByOptimalGroupSize(GroupSatisfaction, GroupMembers.Count);
                Changed = false;
            }
            return GroupSatisfaction;
        }

        private int ReduceByOptimalGroupSize(int groupSatisfaction, int memeberCount)
        {
            switch (Settings.OptimiseType)
            {
                case Settings.OptimiseTypes.EqualiseGroups: return (int)((float)groupSatisfaction * ((float)memeberCount / (float)Settings.OptimalGroupSize));
            }
            return groupSatisfaction;
        }

        public Group Clone()
        {
            Dictionary<int, Person> newDict = new Dictionary<int, Person>();
            foreach (Person person in GroupMembers.Values)
                newDict.Add(person.ID, person);
            return new Group(newDict, GroupSatisfaction, Settings);
        }
    }
}
