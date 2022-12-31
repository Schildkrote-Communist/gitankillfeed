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
            EffectManager.sendUIEffect(32045, 843, player.SteamPlayer().transportConnection, true);
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
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, "text", false);
                }
                else if (id - 1 == 0)
                {
                    EffectManager.sendUIEffectText(843, transportCon, true, "murderer", "");
                    EffectManager.sendUIEffectText(843, transportCon, true, "body", "");
                    EffectManager.sendUIEffectText(843, transportCon, true, "victim", "");
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, "kill", false);
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, "zombie", false);
                }
                else
                {
                    EffectManager.sendUIEffectText(843, transportCon, true, $"murderer{id - 1}", "");
                    EffectManager.sendUIEffectText(843, transportCon, true, $"body{id - 1}", "");
                    EffectManager.sendUIEffectText(843, transportCon, true, $"victim{id - 1}", "");
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, $"kill{id - 1}", false);
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, $"zombie{id - 1}", false);
                }
            }
            liste[id - 1] = 0;
        }
        private void tellPlayer(UnturnedPlayer player, EDeathCause cause, ELimb limb, Steamworks.CSteamID murderer)
        {
            UnturnedPlayer murdererPlayer;
            string configColor = Configuration.Instance.LinesColor;
            string characterName;
            try {
                murdererPlayer = UnturnedPlayer.FromCSteamID(murderer);
                characterName = $"<color={configColor}>" + murdererPlayer.CharacterName.ToString() + $" [{murdererPlayer.Player.life.health} HP]" + "</color>";
            }
            catch {
                murdererPlayer = null;
                characterName = null;
            }
            nombreUi += 1;
            if (nombreUi > 6)
            {
                nombreUi = 1;
            }
            int FreeId = liste.FindIndex(x => x == 0) + 1;
            liste[FreeId - 1] = 1;
            foreach (var transportCon in Provider.EnumerateClients())
            {
                if (FreeId == 1)
                {
                    if(characterName != null)
                    {
                        EffectManager.sendUIEffectText(843, transportCon, true, "murderer", characterName);
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, "kill", true);
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, "zombie", false);
                    }
                    else
                    {
                        EffectManager.sendUIEffectText(843, transportCon, true, "murderer", "");
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, "kill", false);
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, "zombie", true);
                    }               
                    EffectManager.sendUIEffectText(843, transportCon, true, "body", $"<color={configColor}>" + limb.ToString() + "</color>");
                    EffectManager.sendUIEffectText(843, transportCon, true, "victim", $"<color={configColor}>" + player.CharacterName.ToString() + "</color>");
                    
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, "text", true);
                }
                else
                {
                    if (characterName != null)
                    {
                        EffectManager.sendUIEffectText(843, transportCon, true, $"murderer{FreeId - 1}", characterName);
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, $"kill{FreeId - 1}", true);
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, $"zombie{FreeId - 1}", false);
                    }
                    else
                    {
                        EffectManager.sendUIEffectText(843, transportCon, true, $"murderer{FreeId - 1}", "");
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, $"kill{FreeId - 1}", false);
                        EffectManager.sendUIEffectVisibility(843, transportCon, true, $"zombie{FreeId - 1}", true);
                    }        
                    EffectManager.sendUIEffectText(843, transportCon, true, $"body{FreeId - 1}", $"<color={configColor}>" + limb.ToString() + "</color>");
                    EffectManager.sendUIEffectText(843, transportCon, true, $"victim{FreeId - 1}", $"<color={configColor}>" + player.CharacterName.ToString() + "</color>");
                    
                    EffectManager.sendUIEffectVisibility(843, transportCon, true, "text", true);
                }
            }
            StartCoroutine(HideUIDelayed(FreeId));
        }
    }
}
