# AlvinSoft Unturned Announcements
An OpenMod Unturned plugin for join/leave/death/ban messages and announcements. Allows multiple strings per announcement/message type, and selects randomly.

## Config
Use `config.yaml` to enable which announcements you want to use.

### Join announcements
Set `join_announcements` to true, to enable announcements when a player has successfully joined the server. Reload the plugin if the server is running.

### Leave announcements
Set `leave_announcements` to true, to enable announcements when a player has successfully left the server. Reload the plugin if the server is running.

### Death announcements
Set `death_announcements` to true, to enable announcements when a player has died. Reload the plugin if the server is running.

### Ban announcements
You can enable ban announcements to include nothing, the reason, the duration, or both. The duration (given in seconds) is humanized using the nuget package `Humanizer`. Installing `Humanizer` always throws when the plugin loads, because there are locales included that Mono doesn't support. Instead, install `Humanizer.Core` which includes the 'en-US' locale. If you need additional locales, install `Humanizer.Core.xx` (e.g. `Humanizer.Core.fr`), and specify it in the config under `locale`.


## Translations
Most announcement types can have as many strings as desired. Number the strings starting with 0 (0-indexed) and don't leave out any numbers.

**DO** do this:
```
join_announcements:
    0: "{Player} has joined the server!"
    1: "{Player} is now online!"
    2: "{Player} has entered the game!"
```
**DON'T** do this:
```
join_announcements:
    1: "{Player} has joined the server!"
    2: "{Player} is now online!"
    3: "{Player} has entered the game!"
```
**ALSO DON'T** do this:
```
join_announcements:
    1: "{Player} has joined the server!"
    3: "{Player} is now online!"
    4: "{Player} has entered the game!"
```

A wrong numbering will *not* cause the plugin to throw, but in the worst case will cause the plugin to ignore all strings. The only thing that will cause the plugin to throw, is enabling an announcement type and not having provided a translation for an event, when it happens. Enabling the announcement type assumes you want to use it.

`join_announcements` and `leave_announcements` only allow the `{Player}` placeholder, which is replaced with the player's name. `death_announcements` allows `{Player}` and `{Killer}`, which are replaced with the player's name and the killer's name respectively. `ban_announcements` allows `{Player}`, `{Reason}`, and `{Duration}`, which are replaced with the player's name, the reason for the ban, and the duration of the ban respectively.

## Contact
I know Unturned plugins can be a 'pain in the ahh' to get working, especially because of the missing/half-ahhed documentation. So feel free to contact me anytime on Discord if you have any questions or suggestions. My Discord is `m0ment` and my email is `alvin.szoke@gmail.com`.