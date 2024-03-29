# Ground Branch Server Manager
---
GB Server Manager is a WPF desktop app published under the GNU Public License intended as a replacement for the script that currently runs and manages ground branch servers running on Windows server.

![image](https://user-images.githubusercontent.com/12722354/186763035-524be1d8-99b8-4412-9d34-c940c3f12dbf.png)

It currently has the ability to import and manage servers that already exist, Monitoring and re-starting the server after time out. As this is in active development, there may be some bugs or eccentricites that need resolved.  Please feel free to reach out or open an issue if you encounter one.

Technical Note: It is recommended to run this application as administrator, however it can be ran under a standard user account.  Be warned though that occasionally the app may not be able to close the server process due to access being denied (even though the same user account started it).  In this instance you will have to close the window manually because.. windows...

### Features
- Add existing installed servers to manage
- Create a new server with a few clicks and it will download the Ground Branch server to a new folder and set the Server.Ini with your settings.
- Checks for updates each time the server is started or restarted (after the timeout).
- Set launch options for the server
- Run and open the log window right from the app
- The app will check to see if the servers are running every 5 minutes, and update the status of the running servers in the UI every 30 seconds.
- Can download and extract steamCMD for you.
- More to come..

### In Development

- Server PID persistance (So you can close the application to update it without closing the servers)
- Ability to delete servers (including their files)
- E-mail server status notifications
- Maybe RCON support for managing the server from the app (if implemented by the devs)
- Possibly a discord bot for server status and notifications (would require me to figure out how to host that bot though)
- And more as it arises..

[Server Creation Instructions](Instructions.md)
