using System;
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
using static System.Collections.Specialized.BitVector32;
using System.Linq;
using SDG.Unturned;
using System.Collections.Generic;
using NuGet.Protocol.Plugins;

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
            IUserManager userManager) : base(serviceProvider)
        {

            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
            m_EventBus = eventBus;
            m_UserManager = userManager;
        }


        protected override async UniTask OnLoadAsync() {

            m_Logger.LogInformation(m_StringLocalizer.GetString("plugin_events:plugin_start").Value);

            await UniTask.SwitchToMainThread();

            if (m_Configuration.GetValue<bool>("join_messages") == true) {
                m_EventBus.Subscribe<IUserConnectedEvent>(this, async (sender, obj, ev) => {

                    string message = m_StringLocalizer.GetStrings("join:join_messages", new { Player = ev.User.DisplayName }).RandomIndex();

                    await ev.User.PrintMessageAsync(message, System.Drawing.Color.White);

                });
            }

            if (m_Configuration.GetValue<bool>("join_announcements") == true) {
                m_EventBus.Subscribe<IUserConnectedEvent>(this, async (sender, obj, ev) => {

                    string message = m_StringLocalizer.GetStrings("join:join_announcements", new { Player = ev.User.DisplayName }).RandomIndex();

                    await m_UserManager.BroadcastAsync(message, System.Drawing.Color.White);

                });
            }

            if (m_Configuration.GetValue<bool>("leave_messages") == true) {
                m_EventBus.Subscribe<IUserDisconnectedEvent>(this, async (sender, obj, ev) => {

                    string message = m_StringLocalizer.GetStrings("leave:leave_messages", new { Player = ev.User.DisplayName }).RandomIndex();

                    await ev.User.PrintMessageAsync(message, System.Drawing.Color.White);

                });
            }

            if (m_Configuration.GetValue<bool>("leave_announcements") == true) {
                m_EventBus.Subscribe<IUserDisconnectedEvent>(this, async (sender, obj, ev) => {

                    string message = m_StringLocalizer.GetStrings("leave:leave_announcements", new { Player = ev.User.DisplayName }).RandomIndex();

                    await m_UserManager.BroadcastAsync(message, System.Drawing.Color.White);

                });
            }

            if (m_Configuration.GetValue<bool>("death_announcements") == true) {
                m_EventBus.Subscribe<UnturnedPlayerDeathEvent>(this, async (sender, obj, ev) => {

                    string message = ev.DeathCause switch {
                        EDeathCause.BLEEDING => m_StringLocalizer.GetStrings("death:bleeding", new {  Player = ev.Player. }).RandomIndex(),
                        EDeathCause.BONES => m_StringLocalizer.GetStrings("death:bones", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.FREEZING => m_StringLocalizer.GetStrings("death:freezing", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.BURNING => m_StringLocalizer.GetStrings("death:burning", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.FOOD => m_StringLocalizer.GetStrings("death:food", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.WATER => m_StringLocalizer.GetStrings("death:water", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.GUN => m_StringLocalizer.GetStrings("death:gun", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.MELEE => m_StringLocalizer.GetStrings("death:melee", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.ZOMBIE => m_StringLocalizer.GetStrings("death:zombie", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.ANIMAL => m_StringLocalizer.GetStrings("death:animal", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.SUICIDE => m_StringLocalizer.GetStrings("death:suicide", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.KILL => m_StringLocalizer.GetStrings("death:kill", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.INFECTION => m_StringLocalizer.GetStrings("death:infection", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.PUNCH => m_StringLocalizer.GetStrings("death:punch", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.BREATH => m_StringLocalizer.GetStrings("death:breath", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.ROADKILL => m_StringLocalizer.GetStrings("death:roadkill", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.VEHICLE => m_StringLocalizer.GetStrings("death:vehicle", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.GRENADE => m_StringLocalizer.GetStrings("death:grenade", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.SHRED => m_StringLocalizer.GetStrings("death:shred", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.LANDMINE => m_StringLocalizer.GetStrings("death:landmine", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.ARENA => m_StringLocalizer.GetStrings("death:arena", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.MISSILE => m_StringLocalizer.GetStrings("death:missile", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.CHARGE => m_StringLocalizer.GetStrings("death:charge", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.SPLASH => m_StringLocalizer.GetStrings("death:splash", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.SENTRY => m_StringLocalizer.GetStrings("death:sentry", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.ACID => m_StringLocalizer.GetStrings("death:acid", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.BOULDER => m_StringLocalizer.GetStrings("death:boulder", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.BURNER => m_StringLocalizer.GetStrings("death:burner", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.SPIT => m_StringLocalizer.GetStrings("death:spit", new { Player = ev.Player. }).RandomIndex(),
                        EDeathCause.SPARK => m_StringLocalizer.GetStrings("death:spark", new { Player = ev.Player. }).RandomIndex(),
                        _ => m_StringLocalizer.GetStrings("death:default", new { Player = ev.Player. }).RandomIndex(),
                    };

                    // 

                    await m_UserManager.BroadcastAsync(message, System.Drawing.Color.Red);

                });
            }

            if (m_Configuration.GetValue<bool>("ban_announcements:enable") == true) {
                m_EventBus.Subscribe<UnturnedPlayerBannedEvent>(this, async (sender, obj, ev) => {

                    string message = m_StringLocalizer.GetStrings("ban").RandomIndex();

                    await m_UserManager.BroadcastAsync(message, System.Drawing.Color.Red);

                });
            }

        }

        protected override async UniTask OnUnloadAsync() {
            m_Logger.LogInformation(m_StringLocalizer.GetString("plugin_events:plugin_stop"));
        }

    }

    public static class Extensions {
        public static string[] GetStrings(this IStringLocalizer stringRoot, string key, params object[] arguments) {
            List<string> strings = new List<string>();

            int i = 0;
            while (true) {
                string str = stringRoot[$"{key}:{i}", arguments];

                if (string.IsNullOrEmpty(str))
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
