using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using OpenMod.API.Eventing;
using OpenMod.API.Users;
using OpenMod.Core.Users.Events;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Players.Bans.Events;
using SDG.Unturned;
using Steamworks;
using Humanizer;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html
[assembly: PluginMetadata("AlvinSoft.UnturnedAnnouncements", DisplayName = "AlvinSoft Unturned Announcements")]
namespace AlvinSoft {
    public class UnturnedAnnouncements : OpenModUnturnedPlugin {

        private readonly IConfiguration m_Configuration; //config
        private readonly IStringLocalizer m_StringLocalizer; //translations
        private readonly ILogger<UnturnedAnnouncements> m_Logger; //logger
        private readonly IEventBus m_EventBus; //events
        private readonly IUserManager m_UserManager; //users

        public UnturnedAnnouncements(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<UnturnedAnnouncements> logger,
            IServiceProvider serviceProvider,
            IEventBus eventBus,
            IUserManager userManager) : base(serviceProvider) {


            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
            m_EventBus = eventBus;
            m_UserManager = userManager;

            string locale = m_Configuration.GetValue<string>("locale") ?? "en-US";
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo(locale);
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(locale);

        }


        protected override async UniTask OnLoadAsync() {

            m_Logger.LogInformation(m_StringLocalizer.GetString("plugin_events:plugin_start").Value);

            if (m_Configuration.GetValue<bool>("join_announcements") == true) {
                m_EventBus.Subscribe<IUserConnectedEvent>(this, OnUserConnectedEvent);
            }

            if (m_Configuration.GetValue<bool>("leave_announcements") == true) {
                m_EventBus.Subscribe<IUserDisconnectedEvent>(this, OnUserDisconnectedEvent);
            }

            if (m_Configuration.GetValue<bool>("death_announcements") == true) {

                m_EventBus.Subscribe<UnturnedPlayerDeathEvent>(this, OnUnturnedPlayerDeathEvent);
            }

            if (m_Configuration.GetValue<bool>("ban_announcements:enable") == true) {
                m_EventBus.Subscribe<UnturnedPlayerBannedEvent>(this, OnUnturnedPlayerBannedEvent);
            }

        }

        protected override async UniTask OnUnloadAsync() {

            m_Logger.LogInformation(m_StringLocalizer.GetString("plugin_events:plugin_stop"));

        }

        public async Task OnUserConnectedEvent(IServiceProvider sender, object? obj, IUserConnectedEvent ev) {

            string message = m_StringLocalizer.GetStrings("join_announcements", new { Player = ev.User.DisplayName }).RandomIndex();

            await m_UserManager.BroadcastAsync(message, System.Drawing.Color.White);

        }
        public async Task OnUserDisconnectedEvent(IServiceProvider sender, object? obj, IUserDisconnectedEvent ev) {

            string message = m_StringLocalizer.GetStrings("leave_announcements", new { Player = ev.User.DisplayName }).RandomIndex();

            await m_UserManager.BroadcastAsync(message, System.Drawing.Color.White);

        }
        public async Task OnUnturnedPlayerDeathEvent(IServiceProvider sender, object? obj, UnturnedPlayerDeathEvent ev) {

            string VictimName = ev.Player.SteamPlayer.playerID.playerName;

            string? KillerName = null;
            if (ev.Instigator != CSteamID.Nil) {
                var killerPlayer = PlayerTool.getPlayer(ev.Instigator);
                if (killerPlayer != null) {
                    KillerName = killerPlayer.channel.owner.playerID.playerName;
                }
            }
            KillerName ??= m_StringLocalizer["unknown_player"];

            string message = ev.DeathCause switch {
                EDeathCause.BLEEDING => m_StringLocalizer.GetStrings("death:bleeding", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.BONES => m_StringLocalizer.GetStrings("death:bones", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.FREEZING => m_StringLocalizer.GetStrings("death:freezing", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.BURNING => m_StringLocalizer.GetStrings("death:burning", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.FOOD => m_StringLocalizer.GetStrings("death:food", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.WATER => m_StringLocalizer.GetStrings("death:water", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.GUN => m_StringLocalizer.GetStrings("death:gun", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.MELEE => m_StringLocalizer.GetStrings("death:melee", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.ZOMBIE => m_StringLocalizer.GetStrings("death:zombie", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.ANIMAL => m_StringLocalizer.GetStrings("death:animal", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.SUICIDE => m_StringLocalizer.GetStrings("death:suicide", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.KILL => m_StringLocalizer.GetStrings("death:kill", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.INFECTION => m_StringLocalizer.GetStrings("death:infection", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.PUNCH => m_StringLocalizer.GetStrings("death:punch", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.BREATH => m_StringLocalizer.GetStrings("death:breath", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.ROADKILL => m_StringLocalizer.GetStrings("death:roadkill", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.VEHICLE => m_StringLocalizer.GetStrings("death:vehicle", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.GRENADE => m_StringLocalizer.GetStrings("death:grenade", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.SHRED => m_StringLocalizer.GetStrings("death:shred", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.LANDMINE => m_StringLocalizer.GetStrings("death:landmine", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.ARENA => m_StringLocalizer.GetStrings("death:arena", new {  VictimName, KillerName }).RandomIndex(),
                EDeathCause.MISSILE => m_StringLocalizer.GetStrings("death:missile", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.CHARGE => m_StringLocalizer.GetStrings("death:charge", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.SPLASH => m_StringLocalizer.GetStrings("death:splash", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.SENTRY => m_StringLocalizer.GetStrings("death:sentry", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.ACID => m_StringLocalizer.GetStrings("death:acid", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.BOULDER => m_StringLocalizer.GetStrings("death:boulder", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.BURNER => m_StringLocalizer.GetStrings("death:burner", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.SPIT => m_StringLocalizer.GetStrings("death:spit", new { VictimName, KillerName }).RandomIndex(),
                EDeathCause.SPARK => m_StringLocalizer.GetStrings("death:spark", new { VictimName, KillerName }).RandomIndex(),
                _ => m_StringLocalizer.GetStrings("death:default", new { VictimName, KillerName }).RandomIndex(),
            };

            await m_UserManager.BroadcastAsync(message, System.Drawing.Color.Red);

        }
        public async Task OnUnturnedPlayerBannedEvent(IServiceProvider sender, object? obj, UnturnedPlayerBannedEvent ev) {

            bool includeDuration = m_Configuration.GetValue<bool>("ban_announcements:include_duration");
            bool includeReason = m_Configuration.GetValue<bool>("ban_announcements:include_reason");

            var playerObject = PlayerTool.getPlayer(ev.BannedPlayer);
            string Player;
            if (playerObject != null) {
                Player = playerObject.channel.owner.playerID.characterName;
            } else {
                Player = m_StringLocalizer["unknown_player"];
            }
            string Duration = ev.Duration == 0 ? m_StringLocalizer.GetStrings("ban:permanent_duration_string").RandomIndex() : TimeSpan.FromSeconds(ev.Duration).Humanize();
            string Reason = string.IsNullOrEmpty(ev.Reason) ? m_StringLocalizer["ban:no_reason_string"] : ev.Reason;
            string message;
            if (includeDuration) {

                if (includeReason) {

                    message = m_StringLocalizer.GetStrings("ban:with_both", new { Player, Duration, Reason }).RandomIndex();

                } else {

                    message = m_StringLocalizer.GetStrings("ban:with_duration", new { Player, Duration }).RandomIndex();

                }

            } else if (includeReason) {

                message = m_StringLocalizer.GetStrings("ban:with_reason", new { Player, Reason }).RandomIndex();

            } else {

                message = m_StringLocalizer.GetStrings("ban:without_info", new { Player }).RandomIndex();

            }

            await m_UserManager.BroadcastAsync(message, System.Drawing.Color.Red);

        }
    }

    public static class Extensions {
        public static string[] GetStrings(this IStringLocalizer stringRoot, string key, params object[] arguments) {
            List<string> strings = new List<string>();

            int i = 0;
            while (true) {

                string name = $"{key}:{i}";
                string str = stringRoot[name, arguments];

                if (str == name || string.IsNullOrEmpty(str))
                    break;

                strings.Add(str);
                i++;
            }

            return strings.ToArray();
        }

        public static string RandomIndex(this string[] array) {

            if (array == null || array.Length == 0)
                return string.Empty;

            return array[UnityEngine.Random.Range(0, array.Length)];

        }
    }
}
