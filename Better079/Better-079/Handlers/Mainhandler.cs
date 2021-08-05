using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Grenades;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Better079.Handlers
{
    public class Mainhandler : Base.Handler
    {
        public static Mainhandler handler;
        internal static float a1cooldown = 0f;
        internal static float a2cooldown = 0f;
        internal static float a3cooldown = 0f;
        internal static float a4cooldown = 0f;
        internal static float a5cooldown = 0f;
        public static List<Player> scp079players;
        public override void Start()
        {
            handler = this;
            scp079players = new List<Player>();
            Exiled.Events.Handlers.Player.Spawning += OnPlayerSpawn;
            Exiled.Events.Handlers.Player.ChangedRole += OnChangingRole;
            Exiled.Events.Handlers.Scp079.Recontained += OnRecontain;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnd;
        }

        public override void Stop()
        {
            handler = null;
            scp079players = null;
            Exiled.Events.Handlers.Player.Spawning -= OnPlayerSpawn;
            Exiled.Events.Handlers.Player.ChangedRole -= OnChangingRole;
            Exiled.Events.Handlers.Scp079.Recontained -= OnRecontain;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnd;
        }

        public void OnPlayerSpawn(SpawningEventArgs ev)
        {
            if (ev.Player.Role == RoleType.Scp079)
            {
                ev.Player.ShowHint(plugin.Config.SpawnMsg, 15);
            }
        }
        public void OnRoundEnd(RoundEndedEventArgs ev)
        {

            scp079players.Clear();
        }
        public void OnRoundStart()
        {
            a1cooldown = 0f;
            a2cooldown = 0f;
            a3cooldown = 0f;
            a4cooldown = 0f;
            a5cooldown = 0f;
        }
        public void OnRecontain(RecontainedEventArgs ev)
        {
            if (scp079players.Contains(ev.Target))
            {
                scp079players.Remove(ev.Target);
            }
        }
        public void OnChangingRole(ChangedRoleEventArgs ev)
        {
            if (scp079players.Contains(ev.Player))
            {
                scp079players.Remove(ev.Player);
            }
        }
        /// <summary>
        /// Cuando la camara gana XP por hacer determinadas acciones.
        /// </summary>
        /// <param name="ev"></param>
        public void OnGainXP(GainingExperienceEventArgs ev)
        {
            switch (ev.GainType)
            {
                // cuando ayuda a un SCP a matar a un tarado.
                case ExpGainType.KillAssist:
                    break;
                // Cuando mata por Tesla.
                case ExpGainType.DirectKill:
                    break;
                // al blockear puertas.
                case ExpGainType.HardwareHack:
                    break;
                //Ni idea que es esto.
                case ExpGainType.AdminCheat:
                    break;
                //Cuando abre puertas.
                case ExpGainType.GeneralInteractions:
                    break;
                // Cuando ayudo a 106 a meter a alguien en su dmension y se muere.
                case ExpGainType.PocketAssist:
                    ev.Amount = 50f; // Un ejemplo
                    break;
                default:
                    break;
            }
        }
        public void A1Overload(Player scp079)
        {
            a1cooldown = Time.time + plugin.Config.Abilitys.A1.OverloadCoolDown;

            float energy, maxenergy;
            energy = scp079.Energy;
            maxenergy = scp079.MaxEnergy;
            scp079.MaxEnergy = 99999;
            scp079.Energy = 99999;

            Timing.CallDelayed(plugin.Config.Abilitys.A1.OverloadDuration, () =>
            {
                scp079.Energy = energy;
                scp079.MaxEnergy = maxenergy;
            });

        }
        public void A2Poison(Room room, Player scp079)
        {
            if (!scp079players.Contains(scp079)) scp079players.Add(scp079);
            foreach (Player plys in room.Players)
            {
                if (plys.Side == Exiled.API.Enums.Side.Scp) continue;
                plys.EnableEffect(Exiled.API.Enums.EffectType.Disabled, 3f);
                plys.EnableEffect(Exiled.API.Enums.EffectType.Poisoned, plugin.Config.Abilitys.A2.PoisonDuration);
                plys.EnableEffect(Exiled.API.Enums.EffectType.Burned, plugin.Config.Abilitys.A2.PoisonDuration);
                plys.ShowHint($"<b>Fuiste rociado con <color=green>Veneno</color> por el <color=red>SCP-079</color></b>", 8);
            }
            if (room.Zone == Exiled.API.Enums.ZoneType.Surface)
            {
                a2cooldown = Time.time + 60f;
            }
            else
            {
                a2cooldown = Time.time + plugin.Config.Abilitys.A2.PoisonCooldown;
            }
        }
        public void A3Flash(ReferenceHub hub)
        {
            a3cooldown = Time.time + plugin.Config.Abilitys.A3.FlashCooldown;

            var pos = hub.scp079PlayerScript.currentCamera.transform.position;
            GrenadeManager gm = hub.GetComponent<GrenadeManager>();
            GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFlash);
            FlashGrenade flash = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FlashGrenade>();
            flash.fuseDuration = 0.5f;
            flash.InitData(gm, Vector3.zero, Vector3.down, 1f);
            flash.transform.position = pos;
            NetworkServer.Spawn(flash.gameObject);
        }
        public void A4Cure(Room room, Player scp079)
        {
            a4cooldown = Time.time + plugin.Config.Abilitys.A4.CureCooldown;

            foreach (Player plys in room.Players)
            {
                if (plys.Side != Exiled.API.Enums.Side.Scp || plys.Role == RoleType.Scp079) continue;

                plys.GameObject.AddComponent<GameObjects.CureAbility>();

            }
        }
        public void A5Find(Player scp079) {
            var list = GameObject.FindObjectsOfType<Camera079>();
            foreach(Player ply in Player.List)
            {
                if(ply.Team == Team.SCP && ply.Role != RoleType.Scp079)
                {
                    foreach(var cam in list)
                    {
                        if(Vector3.Distance(cam.transform.position, ply.GameObject.transform.position) <= 15f)
                        {
                            scp079.SetCamera(cam.cameraId);
                            a5cooldown = Time.time + plugin.Config.Abilitys.A5.FindCooldown;
                            break;
                        }
                    }
                }
            }
        }

    }
}
