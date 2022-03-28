# Logviewer
[![CodeFactor grade](https://img.shields.io/codefactor/grade/github/asdfcyber/logviewer)](https://www.codefactor.io/repository/github/asdfcyber/logviewer) [![Latest release](https://img.shields.io/github/v/release/asdfcyber/logviewer)](https://github.com/asdfcyber/logviewer/releases/latest) ![Platforms](https://img.shields.io/badge/platform-windows%20%7C%20macos%20%7C%20linux-blue) <br/><br/>


## Description
A mod for [Rail Route](https://railroute.bitrich.info/) which shows debug logs in-game. Logs are filterable with regex patterns, and can be saved to review them later.<br/><br/>


## How to install
Download the latest version for your platform, and unpack the zip in the right directory depending on your operating system:

**Windows:** `%USERPROFILE%\AppData\LocalLow\bitrich\Rail Route\mods`  
**Linux:** `$HOME/.config/unity3d/bitrich/Rail Route/mods`  
**macOS:** `~/Library/Application Support/Rail Route/mods`

If you did it right, you should have the following file structure:
```
mods
|
| - 000_Logviewer
|   |
|   | - Logviewer.dll and other files
```

You can uninstall the mod by deleting the Logviewer folder and its contents. This won't break your game or corrupt savegames. If you want to temporarily disable Logviewer without deleting anything for whatever reason, you can change the file extension of `Logviewer.dll` or move it somewhere else.<br/><br/>


## How to use
Press F3 to toggle the Logviewer panel. The filter inputfield allows you to filter the messages with regular expressions, and the bottom inputfield lets you log custom messages. If you want the currently shown logs to be saved, pressing the save button will save those to a file in `000_Logviewer/logs`.<br/><br/>


## Reporting issues
If you find a bug, please start a new GitHub issue or message me on the Rail Route discord. Don't forget to send your `Player.log` file! You can find it one folder up from the mod folder. A screenshot or savefile might be helpful too. If you think you found a bug in Rail Route and you're using Logviewer, first remove/disable the mod to make sure it isn't actually a Logviewer issue.
