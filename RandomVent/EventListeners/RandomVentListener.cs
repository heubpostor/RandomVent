using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impostor.Api.Events;
using Impostor.Api.Events.Player;
using Impostor.Api.Innersloth;
using Impostor.Api.Innersloth.Customization;
using Impostor.Api.Net.Inner.Objects;
using Microsoft.Extensions.Logging;

namespace RandomVent.EventListeners
{
    public class RandomVentListener : IEventListener
    {
        private readonly ILogger _logger;
        private readonly Random _random;

        public bool PluginActive { get; private set; }

        public RandomVentListener(ILogger<RandomVentListener> logger)
        {
            _logger = logger;
            _random = new Random();
            PluginActive = false;
        }

        [EventListener]
        public async void OnLobbyCreate(IPlayerSpawnedEvent spawnedPlayer)
        {
            if (spawnedPlayer.ClientPlayer.IsHost)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                await SendMessage(spawnedPlayer.PlayerControl, "RandomVent plugin is loaded!");
                await SendMessage(spawnedPlayer.PlayerControl, "Every impostor that vents will be ported to a random vent! :D");
                await SendMessage(spawnedPlayer.PlayerControl, "You can toggle the plugin state with the command /randomvents on/off");
            }
        }

        [EventListener]
        public async void OnChatMessage(IPlayerChatEvent chatEvent)
        {
            var message = chatEvent.Message;
            var player = chatEvent.PlayerControl;
            if (message.StartsWith("/randomvents"))
            {
                // Check if player is allowed to enable/disable plugin
                if (!chatEvent.ClientPlayer.IsHost)
                {
                    await SendMessage(player, "You are not allowed to do that! Please ask your lobby host.");
                    return;
                }

                var option = message.Split(' ')[1];
                switch (option)
                {
                    case "on":
                        PluginActive = true;
                        await SendMessage(player, "RandomVents was successfully activated! Watch out when venting!");
                        break;
                    case "off":
                        PluginActive = false;
                        await SendMessage(player, "RandomVents was successfully disabled! Vents are now the impostors safe space!");
                        break;
                    default:
                        await SendMessage(player, $"You cannot use the option '{option}'! Please try again with on/off!");
                        return;
                }
            }
        }

        [EventListener]
        public async void OnImpostorVented(IPlayerVentEvent playerVentEvent)
        {
            if (PluginActive && playerVentEvent.PlayerControl.PlayerInfo.IsImpostor)
            {
                VentLocation randomVent;

                switch (playerVentEvent.Game.Options.Map)
                {
                    case MapTypes.Skeld:
                        randomVent = VentLocation.SkeldAdmin + (uint)_random.Next(0, 13);
                        break;
                    case MapTypes.MiraHQ:
                        randomVent = VentLocation.MiraBalcony + (uint)_random.Next(0, 10);
                        break;
                    case MapTypes.Polus:
                        randomVent = VentLocation.PolusSecurity + (uint)_random.Next(0, 11);
                        break;
                    default:
                        _logger?.LogError("Map type cannot be out of MapTypes enum!");
                        return;
                }

                _logger?.LogDebug(
                    $"Player {playerVentEvent.ClientPlayer.Character.PlayerInfo.PlayerName} was entering vent {playerVentEvent.VentId}, moving him to vent {randomVent}!"
                    );

                await playerVentEvent.PlayerControl.SetVentAsync(randomVent);
                await playerVentEvent.PlayerControl.ExitVentAsync();
            }
        }

        private async ValueTask<int> SendMessage(IInnerPlayerControl player, string message)
        {
            string name = player.PlayerInfo.PlayerName;
            byte color = player.PlayerInfo.ColorId;
            await player.SetNameAsync("[00FF00FF]RandomVent Plugin");
            await player.SetColorAsync((byte)ColorType.Lime);
            await player.SendChatAsync(message);
            await player.SetNameAsync(name);
            await player.SetColorAsync(color);
            return 0;
        }
    }
}
