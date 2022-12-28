using System;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Unturned;
using System.Collections;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnityEngine;
using Rocket.Core.Logging;

namespace gitankillfeed
{
    public class gitankillfeed : RocketPlugin<gitankillfeedConfiguration>
    {
        public static gitankillfeed Instance { get; private set; }
        public int nombreUi = 0;
        public List<int> liste = new List<int>() { 0, 0, 0, 0, 0, 0 };
        protected override void Load()
        {
            UnturnedPlayerEvents.OnPlayerDeath += tellPlayer;
            U.Events.OnPlayerConnected += giveUI;
            Rocket.Core.Logging.Logger.Log("Event plugin charger !");

        }
        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerDeath -= tellPlayer;
            U.Events.OnPlayerConnected -= giveUI;
            Rocket.Core.Logging.Logger.Log("plugin decharger !");
        }
        private void giveUI(UnturnedPlayer player)
        {
            EffectManager.sendUIEffect(32045, 263, player.SteamPlayer().transportConnection, true);
        }

        private IEnumerator HideUIDelayed(int id)
        {
            int cooldown = (int)Configuration.Instance.cooldown;
            if(cooldown != 0)
            {
                yield return new WaitForSeconds(cooldown);
            }
            else
            {
                Rocket.Core.Logging.Logger.Log("cooldown marche pas fdp !");
                yield return new WaitForSeconds(8);
            }
            nombreUi -= 1;
            foreach (var transportCon in Provider.EnumerateClients())
            {
                if (nombreUi == 0 & liste.Exists(x => x == 1) == false)
                {
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, "text", false);
                }
                else if (id - 1 == 0)
                {
                    EffectManager.sendUIEffectText(263, transportCon, true, "murderer", "");
                    EffectManager.sendUIEffectText(263, transportCon, true, "body", "");
                    EffectManager.sendUIEffectText(263, transportCon, true, "victim", "");
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, "kill", false);
                }
                else
                {
                    EffectManager.sendUIEffectText(263, transportCon, true, $"murderer{id - 1}", "");
                    EffectManager.sendUIEffectText(263, transportCon, true, $"body{id - 1}", "");
                    EffectManager.sendUIEffectText(263, transportCon, true, $"victim{id - 1}", "");
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, $"kill{id - 1}", false);
                }
            }
            liste[id - 1] = 0;
        }
        private void tellPlayer(UnturnedPlayer player, EDeathCause cause, ELimb limb, Steamworks.CSteamID murderer)
        {
            UnturnedPlayer murdererPlayer = UnturnedPlayer.FromCSteamID(murderer);
            nombreUi += 1;
            if (nombreUi > 6)
            {
                nombreUi = 1;
            }
            int FreeId = liste.FindIndex(x => x == 0) + 1;
            Rocket.Core.Logging.Logger.Log("FreeId : " + FreeId.ToString());
            liste[FreeId - 1] = 1;
            foreach (var transportCon in Provider.EnumerateClients())
            {
                if (FreeId == 1)
                {
                    EffectManager.sendUIEffectText(263, transportCon, true, "murderer", murdererPlayer.CharacterName.ToString());
                    EffectManager.sendUIEffectText(263, transportCon, true, "body", limb.ToString());
                    EffectManager.sendUIEffectText(263, transportCon, true, "victim", player.CharacterName.ToString());
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, "kill", true);
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, "text", true);
                }
                else
                {
                    EffectManager.sendUIEffectText(263, transportCon, true, $"murderer{FreeId - 1}", murdererPlayer.CharacterName.ToString());
                    EffectManager.sendUIEffectText(263, transportCon, true, $"body{FreeId - 1}", limb.ToString());
                    EffectManager.sendUIEffectText(263, transportCon, true, $"victim{FreeId - 1}", player.CharacterName.ToString());
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, $"kill{FreeId - 1}", true);
                    EffectManager.sendUIEffectVisibility(263, transportCon, true, "text", true);
                }
            }
            StartCoroutine(HideUIDelayed(FreeId));
        }
    }
}
