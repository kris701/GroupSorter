using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter
{
    public class Group : ICloneable<Group>
    {
        public Group() { }

        public Group(Dictionary<int, Person> groupMembers, int groupSatisfaction)
        {
            GroupMembers = groupMembers;
            GroupSatisfaction = groupSatisfaction;
        }

        public Dictionary<int, Person> GroupMembers { get; set; } = new Dictionary<int, Person>();
        public int GroupSize { get => GroupMembers.Count; }
        public int GroupSatisfaction { get; internal set; }
        public int CalculateGroupSatisfaction()
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
                    GroupSatisfaction += 2;
                    person.GotGroupMembersCount++;
                }
            }
            return GroupSatisfaction;
        }

        public Group Clone()
        {
            Dictionary<int, Person> newDict = new Dictionary<int, Person>();
            foreach (Person person in GroupMembers.Values)
                newDict.Add(person.ID, person);
            return new Group(newDict, GroupSatisfaction);
        }
    }
}
