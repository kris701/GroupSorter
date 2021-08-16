# GroupSorter

This little program can be used to sort a bunch of people into groups, based on the peoples own preferences.
A person can have 3 preferences, arranges as 1st, 2nd and 3rd priority.
Its really simple to use, since all you need to do is edit some CSV files.

![image](https://user-images.githubusercontent.com/22596587/129479565-b6a2de2a-f838-4dec-8b8e-2f442ede7ce7.png)

## How to use
Here is a little guide to how the program works.

### Step 1
Either compile the program or take the one in the latest release (It will make some default csv files if none exist)

### Step 2
Take a look in the `settings.csv` file next to the executable.
It contains some general settings to limit the sorting algorithm a bit:
* MinGroupSize: Is the minimum size a group is allowed to be
* MaxGroupSize: Is the maximum size a group is allowed to be
* NumberOfGroups: Is the amount of groups the algorithm should split the people into
* OptimiseType: Is a litte addition to the satisfaction level of a group. The choices are:
  * None: Dont do anything
  * EqualiseGroups: Adjusting the satisfaction of a group depending on how equal their size is. This will make more "equal" sized groups.
* ShowAll: Shows all the itterations if true, instead of just the possible ones that was better than the last one.

### Step 3
Take a look in the `input.csv` file, also next to the executable.
Here you can add the people to be sorted, and their preferences.
* The ID should start from 0 and go up by each row
* The name is just there for you to know who is who
* Preference1 is the first preference for the person. This is a ID to another person that they wanna be in group with.
* Preference2 is the second preference for the person. This is a ID to another person that they wanna be in group with.
* Preference3 is the third preference for the person. This is a ID to another person that they wanna be in group with.

### Step 4
Run the `GroupSorter.exe` and wait for it to finish. The output best group will be put in the `output.csv` file.
