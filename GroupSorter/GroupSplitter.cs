using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupSorter
{
    public class GroupSplitter
    {
        public Dictionary<int, Person> ListOfPeople { get; internal set; }
        public Settings Settings { get; set; }
        public int Iterations { get; set; }
        public List<Group> BestCombination = new List<Group>();
        public int BestSatisfaction { get; set; } = 0;
        public bool IsAnyFound { get; internal set; }

        public GroupSplitter(Dictionary<int, Person> listOfPeople, Settings settings)
        {
            ListOfPeople = listOfPeople;
            Settings = settings;
        }

        public void SplitGroups()
        {
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

                Iterations++;
            }

            foreach (Group group in BestCombination)
                group.Changed = true;
            GetSatisfaction(BestCombination);
        }

        private List<Group> GenerateLeftMostGroup()
        {
            Dictionary<int, Person> keyValuePairs = new Dictionary<int, Person>(ListOfPeople);
            List<Group> baseGroup = new List<Group>();
            for (int i = 0; i < Settings.NumberOfGroups; i++)
            {
                Group newGroup = new Group(Settings);

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
                    IsAnyFound = true;
                }
            }
        }

        private void MovePersonToGroup(Group fromGroup, Group toGroup, int personID)
        {
            fromGroup.Changed = true;
            toGroup.Changed = true;
            toGroup.GroupMembers.Add(personID, fromGroup.GroupMembers[personID]);
            fromGroup.GroupMembers.Remove(personID);
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

        private int GetSatisfaction(List<Group> groups)
        {
            int returnValue = 0;
            foreach (Group group in groups)
                returnValue += group.CalculateGroupSatisfaction();

            return returnValue;
        }
    }
}
