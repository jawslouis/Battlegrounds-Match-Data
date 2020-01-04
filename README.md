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

1. Download the latest zip file from the [releases page](https://github.com/jawslouis/battlegrounds-stats/releases).  
2. Unblock the zip file before unzipping, by right-clicking it and choosing properties:
![Unblock](Images/unblock.png?raw=true)  
3. Unzip the archive to `%AppData%/HearthstoneDeckTracker/Plugins` To find this directory, you can click the following button in the Hearthstone Deck Tracker options menu: `Options -> Tracker -> Plugins -> Plugins Folder`
4. Inside the `Hearthstone Battlegrounds Stats` directory, there should be a bunch of files, including a file called `Hearthstone Battlegrounds Stats.dll`.  
5. Launch Hearthstone Deck Tracker. Enable the plugin in `Options -> Tracker -> Plugins`.  

## Usage Notes

1. By default, the CSV will be saved on your desktop. To change the save location, go to the Hearthstone Deck Tracker and navigate to `Plugins -> Battlegrounds Stats -> Set CSV Location`.
2. Do not have the CSV open when a match has ended. Otherwise, the plugin will not be able to write the match data to the CSV.
