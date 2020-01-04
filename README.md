# Battlegrounds-Stats
A plugin for [Hearthstone Deck Tracker](https://github.com/HearthSim/Hearthstone-Deck-Tracker) to save Battlegrounds match data in a CSV.

The following data will be captured for each match:
- Hero
- Finishing position
- MMR after match
- Ending minions
- Turns taken to reach each tavern tier

Sample output of the CSV, when viewed in a spreadsheet (e.g. Excel):

![CSVFormat](Images/csvformat.png?raw=true)  


## Installation

1. Download the latest `Battlegrounds.Match.Data.dll` file from the [releases page](https://github.com/jawslouis/battlegrounds-stats/releases)
2. Launch Hearthstone Deck Tracker. Go to `Options -> Tracker -> Plugins`
3. Drag and drop `Battlegrounds.Match.Data.dll` onto the Plugins window. Enable the plugin.

## Usage Notes

1. By default, the CSV will be saved in the Hearthstone Deck Tracker folder. To change the save location, go to `Plugins -> Battlegrounds Stats -> Set CSV Location` in the Hearthstone Deck Tracker.
2. Do not have the CSV open when a match has ended. Otherwise, the plugin will not be able to write the match data to the CSV.
