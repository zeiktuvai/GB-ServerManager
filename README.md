# Ground Branch Server Manager
---
GB Server Manager was is a WPF desktop app published under the GNU Public License intended as a replacement for the script that currently runs and manages ground branch servers running on Windows server.

![image](https://user-images.githubusercontent.com/12722354/186228204-356426d6-0fbd-4aa6-b877-37d8e1608f9c.png)

It currently has the ability to import and manage servers that already exist, Monitoring and re-starting the server after time out. As this is in active development, there may be some bugs or eccentricites that need resolved.  Please feel free to reach out or open an issue if you encounter one.

## PLEASE NOTE: In order for the application to be able to stop the server process properly, you must run this application as administrator.

### Features
- Add existing installed servers to manage
- Set launch options for the server
- Run and open the log window right from the app
- A timer runs that will check player counts and if the server is up every 5 minutes (May make this a setting that can be changed later)
- More to come..

### In Development
- Ability to edit ini files
- Ability to download and create servers right from the app
- Instituing a separate timer for each server so that if it dies in that 5 minute window you don't have to wait for the next tick for it to come back up.
- Maybe RCON support for managing the server from the app (if implemented by the devs)
- Ability to delete servers (including their files)
- E-mail server status notifications
- Possibly a doscord bot for server status and notifications (would require me to figure out how to host that bot though)
- And more as it arises..

