using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impostor.Api.Events;
using Impostor.Api.Events.Player;
using Impostor.Api.Innersloth;

namespace RandomVent.EventListeners
{
    public class RandomVentListener : IEventListener
    {
        [EventListener]
        public async void OnImpostorVented(IPlayerVentEvent playerVentEvent)
        {
            if (playerVentEvent.PlayerControl.PlayerInfo.IsImpostor)
            {
                await playerVentEvent.PlayerControl.SetVentAsync(VentLocation.PolusO2);
                await playerVentEvent.PlayerControl.ExitVentAsync();
            }
        }
    }
}
