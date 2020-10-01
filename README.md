# Battlegrounds-Match-Data
A plugin for [Hearthstone Deck Tracker](https://github.com/HearthSim/Hearthstone-Deck-Tracker) to save Battlegrounds match data to a CSV, Google Spreadsheet, or an [online dashboard](https://bgstats.cintrest.com).

The below data will be captured for each game:
- Hero
- Finishing position
- MMR after match
- Turns taken to reach each tavern tier

For each battle:
- Minion stats & keywords (e.g. divine shield, poison) for yourself and the opponent
- Opponent's hero
- Turn number
- Combat result

Your minions' average & total stats (attack & health) will also be shown on an overlay, just under the turn timer:

![Overlay](Images/overlay.PNG?raw=true)

## Installation
1. Download the latest `BattlegroundsMatchData.zip` file from the [releases page](https://github.com/jawslouis/battlegrounds-stats/releases)
2. Launch Hearthstone Deck Tracker. Go to `Options -> Tracker -> Plugins`
3. Drag and drop the zip file onto the Plugins window. Enable the plugin.
4. From the toolbar, go to `Plugins -> Battlegrounds Match Data Settings`. This will open the settings menu. 

![Settings](Images/settings.PNG?raw=true)  

Follow the setup instructions below for your preferred save method:
- [Upload to BgStats Dashboard](#upload-to-bgstats-dashboard)
- [Auto-upload to Google Spreadsheet](#auto-upload-to-google-spreadsheet)
- [CSV](#csv)

## Upload to BgStats Dashboard

Simply go to the settings menu and enable upload to BgStats Dashboard.
There will also be a link to visit your dashboard on the site, which will be at `http://bgstats.cintrest.com/<user>-<id>`. 

This is a sample view of the dashboard, after you've logged some games:

![Sample Dashboard](Images/sample-dash.PNG?raw=true)  

## Auto-upload to Google Spreadsheet
Auto-uploading is an optional feature that is disabled by default. If enabled, you can also track the minions for yourself & your opponent for each turn.
The [set-up instructions](../../wiki/Auto-upload-to-Google-Spreadsheet/) are in the wiki - this will take about 20 mins.

Here is an [example spreadsheet](https://docs.google.com/spreadsheets/d/1jlK08xcHi83u85V2YMT7UhQCsLeY8iENztPi7BFnR58/edit?usp=sharing)  showing statistics that can be automatically tracked when auto-uploading is enabled.

![statistics](Images/statistics-3.PNG?raw=true)

![statistics](Images/statistics-2.PNG?raw=true)

![statistics](Images/statistics-1.PNG?raw=true)

![spreadsheet](Images/spreadsheet.PNG?raw=true)

![spreadsheet](Images/allboards.PNG?raw=true)

## CSV
By default, the CSV will be saved in the Hearthstone Deck Tracker folder. To change the save location, modify the file location in the settings menu. 

This is a sample output of the CSV, when viewed in a spreadsheet (e.g. Excel):

![CSVFormat](Images/csvformat.png?raw=true)

Do not have the CSV open when a match has ended. Otherwise, the plugin will not be able to write the match data to the CSV.
