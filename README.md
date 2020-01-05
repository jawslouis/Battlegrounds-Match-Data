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
Auto-uploading is an optional feature that is disabled by default. Setting this up will take about 20 mins. 

#### Setting up Google API
1. Go to [Google Developers Console](https://console.developers.google.com/) and create a new API project. It will take a few seconds to get ready.
2. Go to "Credentials" on the sidebar, click “Create credentials” and select "Service Account Key". After creation, you will automatically download a credential JSON file. Copy the file to the plugins folder (or any other convenient location).
![Create Credentials](Images/create-credential.PNG?raw=true)  
3. Back at the Credentials page, select "Manage Service Account" (see below). Copy the email address for the service account and save it somewhere. 
![Manage Account](Images/manage-account.PNG?raw=true)  
4. Go to [Google Sheets API](https://console.developers.google.com/apis/library/sheets.googleapis.com) and enable the API.
5. Create a blank Google Spreadsheet. Add the service account as an editor, using the email copied in step 3. Copy the Spreadsheet ID, which is the jumble of letters in the URL. Example: 
docs.google.com/spreadsheets/d/**16pcIzB7AsjM4KjJgHTMm3fJdo-4n2eK4PGN8qEJVZh0**/edit#gid=0

#### Plugin Setup
6. Go to the plugin folder. It can be found at `Options -> Tracker -> Plugins -> Plugins Folder`. Open `BattlegroundsMatchData.config` using a text editor.
7. Set `UploadEnabled` to true, and fill in your Spreadsheet ID and the file location for the JSON credential. Your config file should look like below. Make sure the file path uses double slashes:
```
{  
  "UploadEnabled": true,
  "TestUpload": false,
  "SpreadsheetId": "16pcIzB7AsjM4KjJgHTMm3fJdo-4n2eK4PGN8qEJVZh0",
  "CredentialLocation": "C:\\Users\\<User>\\AppData\\Roaming\\HearthstoneDeckTracker\\Plugins\\BattlegroundsMatchData\\MyCredential.json"
}
```
8. That's it. You're finally done! Restart Hearthstone Deck Tracker and make sure the plugin is enabled. Your games will now be both saved in a local CSV, and uploaded to your Google spreadsheet.

You can disable uploading at any point by going to `Plugins -> Battlegrounds Stats -> Enable Google Spreadsheet Upload`

![Manage Upload](Images/set-upload.png?raw=true)  

