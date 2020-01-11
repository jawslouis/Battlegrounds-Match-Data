# Battlegrounds-Match-Data
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
1. Download the latest `BattlegroundsMatchData.zip` file from the [releases page](https://github.com/jawslouis/battlegrounds-stats/releases)
2. Launch Hearthstone Deck Tracker. Go to `Options -> Tracker -> Plugins`
3. Drag and drop the zip file onto the Plugins window. Enable the plugin.

## Usage Notes
1. Do not have the CSV open when a match has ended. Otherwise, the plugin will not be able to write the match data to the CSV.
2. By default, the CSV will be saved in the Hearthstone Deck Tracker folder. To change the save location, go to `Plugins -> Battlegrounds Stats -> Set CSV Location` in the Hearthstone Deck Tracker.

## Auto-upload to Google Spreadsheet
Auto-uploading is an optional feature that is disabled by default. The [set-up instructions](../../wiki/Auto-upload-to-Google-Spreadsheet/) are in the wiki - this will take about 20 mins. 

[See an example](https://docs.google.com/spreadsheets/d/1N8MS8fNeE3JBLAqDyqQOefSLUM0OIyfHSDDwipP5UCU/edit?usp=sharing) of statistics that can be automatically tracked when auto-uploading is enabled.

![statistics](Images/statistics-1.PNG?raw=true)
![statistics](Images/statistics-2.PNG?raw=true)
![statistics](Images/statistics-3.PNG?raw=true)
![spreadsheet](Images/spreadsheet.PNG?raw=true)
