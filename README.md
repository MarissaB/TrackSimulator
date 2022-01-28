# Track Simulator 1.0

I've worked as the tower operator at my local drag strip for over half a decade. I'm very familiar with the computer system and calculations that go on behind the scenes from when the cars pull up to the moment the winner light turns on. Unfortunately a lot of companies that make track timing software have cripplingly poor UI/UX, outdated data management, and error-prone systems. Out of frustration (and a little spite) I want to see if I can make my own UWP app to run a drag strip tower system from start to finish.

# Goals
My goals are to have management for driver lists, race classes, searching of results, and time slip printing as well as creating competition ladders and qualifying brackets. The major pain points in the software we use are the non-intuitive interface and poor data organization.

- In some drag races, both cars leave the starting line at the same time and the winner is whoever crosses the finish line first. In other races, the cars leave with a staggered start (meaning one is given the green light before the other one) and the winner is whoever runs closest to their target time. In time trials, there's no winner at all. This app will support all three types.

## Glossary

- A **Driver** refers to the human behind the wheel. Drivers are designated with an alphanumeric number, which is optionally registered with the NHRA (meaning it's regionally unique to them).
- The **Category** is the class the Driver is in. They can enter multiple categories in the same day but cannot enter the same category twice since one person can't drive two vehicles at a time. Different categories use different settings for determining tournament laddering, starting lights, and finish lines.
- A **Round** refers to the number of times the entire Category has finished. We start with everyone in Round 1, the winners of that in Round 2, and so on.
- A **Bye** in competition is when a competitior does not have an opponent and is allowed to advance to the next Round automatically. This usually happens if a vehicle breaks or there's an odd number of competitors in a Round.
- A **Run** is a pass down the track with from start to finish. If a vehicle breaks or wrecks before reaching the finish, the Run is considered incomplete but still saved.
- A **Race** is a pair of Runs (one each in the left and right lanes) made down the track. A Race can be in a competitive or time trial Category, and one or both lanes can be used. If a lane is empty for a Race, it will be saved as an empty Run.
- A **Timeslip** is a printout that includes the speed and time readings at certain lengths down the track for each lane, the Category for the Race, and the Driver in each lane. At the track we have a tiny dot-matrix printer. This app will generate a txt file instead.


## Feature List

I've broken down my features list based on what I do on a day-to-day basis for a race weekend.

1. Enter/update driver info from paper tech cards.
- [x] Add, edit, and delete Drivers

2. Set the race categories for the weekend. This doesn't often change but we adjust finish lines regularly, for example.
- [x] Add, edit, and delete Categories

3. Announcer calls up a category and drivers start pulling forward in two lanes. This is when I enter their numbers to queue up pairs while they do burnouts.
- [ ] Select a Category for a Race and display the Race screen for entering vehicles

4. The drivers finish roasting tires and line up at the starting line. I signal the system to be "tower ready" and the timing software triggers the starting lights. Away they go!
- [ ] Generate and save Runs for each lane to advance the queue
- [ ] Generate a formatted Timeslip (txt file) when a Race is saved
- [ ] Display the results of the Race on a separate Announcer screen
- [ ] Allow the user to terminate a Run in progress as incomplete (i.e. a vehicle broke mid-race)

5. After time trials, eliminations start. After every round I create a new ladder sheet for each competition Category.
- [ ] Generate a ladder sheet (txt file) for a Category's next Round
- [ ] Customize the ladder generation in the Settings for how to pick the Bye competitor in a Round with an odd number of competitors

6. Sometimes we need to pull reports over the course of a season to determine yearly winners or vehicle performance.
- [ ] Generate a report (CSV file) with all of a Driver's Runs for the year
- [ ] Generate a report with a Driver's current stats for the year
- [ ] Generate a report with all of the competitors for a Category for the year

# Limitations
I don't have a live data feed from timing sensors to hook into with this app (unless my track upgrades, then it's game on!). I want to spin up a simulator function that will create an entire race day's worth of runs (350+).

The tower I work in is air-gapped and the track sensors are hooked into a closed system. There's no internet connection to run the system, so my app will be made with that in mind.

# Bonus Game
For kicks and giggles I may add a reaction timer too - it's fun to see how good or bad your own reaction times are whether you're looking at a screen or sitting in a vehicle.
