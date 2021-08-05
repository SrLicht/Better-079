using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Better079.System;
using Exiled.API.Features;
using Better079.Handlers;
using Better079.System;
using UnityEngine;
using Exiled.API.Extensions;

namespace Better079.Command
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Better079Command : ICommand
    {
        public string Command => "079";

        public string[] Aliases => new string[] { Plugin.Instance.Config.CommandPrefix };

        public string Description => "\n<color=yellow>| Habilidades del SCP-079 |</color>";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender != null)
            {
                try
                {
                    Player player = Player.Get((sender as CommandSender).SenderId);
                    if (arguments.IsEmpty())
                    {
                        string msg;
                        msg = "\n<color=yellow>El comando es .079 [Numero de habilidad o prefix de habilidad]</color>\n<color=red>Las habilidades que tienes son:</color>\n" +
                            $"<color=red>A1 | 1:</color> <color=yellow>Habilidad que recarga por completo tu energia, a nivel tier 5 te permite tener 999 de energia por {Plugin.Instance.Config.Abilitys.A1.OverloadDuration} tiene un cooldown de {Plugin.Instance.Config.Abilitys.A1.OverloadCoolDown} segundos y no usa energia.</color>\n" +
                            $"<color=red>A2 | 2:</color> <color=yellow>Habilidad que te permite envenenar a los enemigos en la sala donde este tu camara actual al tirar el comando, tiene un cooldown de {Plugin.Instance.Config.Abilitys.A2.PoisonCooldown} segundos y usa {Plugin.Instance.Config.Abilitys.A2.PoisonEnergyCost} de energia.</color>\n" +
                            $"<color=red>A3 | 3:</color> <color=yellow>Habilidad que te permite tirar una granada flash en donde este la camara actualmente, tiene un cooldown de {Plugin.Instance.Config.Abilitys.A3.FlashCooldown} segundos y usa {Plugin.Instance.Config.Abilitys.A3.FlashEnergyCost} de energia.</color>\n" +
                            $"<color=red>A4 | 4:</color> <color=yellow>Habilidad que te permite curar a los SCPs que se encuentren en la habitacion donde estan, no es necesario que se queden en dicha habitacion para mantener la curacion, tiene un cooldown de {Plugin.Instance.Config.Abilitys.A4.CureCooldown} y usa {Plugin.Instance.Config.Abilitys.A4.CureEnergyCost} de energia.</color>\n";
                        response = msg;
                        return false;
                    }
                    else if (player.Role != RoleType.Scp079)
                    {
                        response = "Tienes que ser SCP-079 para usar estos comandos";
                        return false;
                    }
                    switch (arguments.At(0).ToLower())
                    {
                        // 0 = tier 1 | 1 = tier 2| 2 = tier 3 | 3 = tier 4  | 4 = tier 5;
                        case "a1":
                        case "1":
                            {
                                if (Time.time < Mainhandler.a1cooldown)
                                {
                                    response = $"Tienes que esperar <color=red>{(int)Math.Round(Mainhandler.a1cooldown - Time.time)}</color> segundos para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                if (player.Level < 4)
                                {
                                    player.Energy = player.MaxEnergy;
                                    player.Experience += 25;
                                    Mainhandler.a1cooldown = Time.time + Plugin.Instance.Config.Abilitys.A1.OverloadCoolDown;
                                    response = "<color=#25AEE6>Robaste energia</color> de los generadores, carga completa!";
                                    player.ShowHint(response);
                                    return true;
                                }
                                Mainhandler.handler.A1Overload(player);
                                response = "<color=#E62586>Tienes energia infinita por 15 segundos!!</color>";
                                player.ShowHint(response);
                                return true;
                            }
                        case "a2":
                        case "2":
                            {
                                if (Time.time < Mainhandler.a2cooldown)
                                {
                                    response = $"Tienes que esperar <color=red>{(int)Math.Round(Mainhandler.a2cooldown - Time.time)}</color> segundos para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                else if (player.Level < 2)
                                {
                                    response = "Necesitas ser <color=#25BAE6>Tier 3</color> para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                else if (player.Energy < Plugin.Instance.Config.Abilitys.A2.PoisonEnergyCost)
                                {
                                    response = "<color=red>No</color> tienes la energia necesaria para usar este comando";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                player.Experience += 30;
                                Mainhandler.handler.A2Poison(player.Camera.Room(), player);
                                player.Energy -= Plugin.Instance.Config.Abilitys.A2.PoisonEnergyCost;
                                response = "Gaseando habitacion con <color=green>veneno</color>, no afectara a tus alidos<color=red> >:) </color>";
                                player.ShowHint(response, 5);
                                return true;
                            }
                        case "a3":
                        case "3":
                            {
                                if (Time.time < Mainhandler.a3cooldown)
                                {
                                    response = $"\nTienes que esperar <color=red>{(int)Math.Round(Mainhandler.a3cooldown - Time.time)}</color> segundos para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                else if (player.Level < 3)
                                {
                                    response = "\nNecesitas ser <color=#25BAE6>Tier 4</color> para usar esta habilidad";
                                    player.ShowHint(response);
                                    return false;
                                }
                                else if (player.Energy < Plugin.Instance.Config.Abilitys.A3.FlashEnergyCost)
                                {
                                    response = "\n<color=red>No</color> tienes la energia necesaria para tirar esta habilidad";
                                    player.ShowHint(response, 4);
                                    return false;
                                }
                                player.Experience += 54;
                                Mainhandler.handler.A3Flash(player.ReferenceHub);
                                player.Energy -= Plugin.Instance.Config.Abilitys.A3.FlashEnergyCost;
                                response = "\nLanzando <color=#25E6E0>Flash</color><color=#E68325>.</color><color=#E6A025>.</color><color=#D7E625>.</color>";
                                player.ShowHint(response);
                                return true;
                            }
                        case "a4":
                        case "4":
                            {
                                if (Time.time < Mainhandler.a4cooldown)
                                {
                                    response = $"\nTienes que esperar <color=red>{(int)Math.Round(Mainhandler.a4cooldown - Time.time)}</color> segundos para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                else if (player.Energy < Plugin.Instance.Config.Abilitys.A4.CureEnergyCost)
                                {
                                    response = "\n<color=red>No</color> tienes la energia necesaria para usar este comando";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                else if (player.Level < 4)
                                {
                                    response = "\nNecesitas ser <color=#25BAE6>Tier 5</color> para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                Mainhandler.handler.A4Cure(player.Camera.Room(), player);
                                response = "\nLanzando <color=green>Nano-regeneracion</color> a los <color=red>SCPs</color> en el cuarto actual.";
                                player.ShowHint(response, 4);
                                return true;
                            }
                        case "a5":
                        case "5":
                            {
                                if (Time.time < Mainhandler.a5cooldown)
                                {
                                    response = $"\nTienes que esperar <color=red>{(int)Math.Round(Mainhandler.a5cooldown - Time.time)}</color> segundos para usar esta habilidad.";
                                    player.ShowHint(response, 5f);
                                    return false;
                                }
                                Mainhandler.handler.A5Find(player);
                                response = "\nCambiando a camara mas cercana a un SCP";
                                player.ShowHint(response);
                                return true;
                            }
                        default:
                            response = "\n<color=yellow>Usa el comando .079 para saber que habilidades tienes</color>";
                            return false;
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"{e.TargetSite} {e.Message}\n{e.StackTrace}");
                    throw;
                }
            }
            else
            {
                response = "Sender Null";
                return false;
            }
        }
    }
}
