using Better079.System;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Extensions;

namespace Better079.Handlers.GameObjects
{
    public class CureAbility : MonoBehaviour
    {
        private Player scpPlayer;

        private void Awake()
        {
            RegisteringEvents();
        }

        private void Start()
        {
            InvokeRepeating("CureSCP", 0f, 0.5f);
            Destroy(Plugin.Instance.Config.Abilitys.A4.CureDuration);
            scpPlayer.ShowHint("<color=green>Nano-regeneracion activada:</color><color=red>Estas siendo curado por SCP-079</color>", 8);
        }

        private void OnDestroy()
        {
            CancelInvoke("CureSCP");
            UnRegisteringEvents();
        }

        public void Destroy(float timetodestroy = 0f)
        {
            try
            {
                if (timetodestroy > 0)
                {
                    Destroy(this, timetodestroy);
                }
                else
                {
                    Destroy(this);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception: {e}\n Couldn't destroy: {this}\nIs ReferenceHub null? {scpPlayer is null}");
            }
        }

        public void RegisteringEvents()
        {
            if (!(Player.Get(gameObject) is Player ply))
            {
                Destroy();
                return;
            }
            scpPlayer = ply;
            Exiled.Events.Handlers.Player.Destroying += OnLeft;
            Exiled.Events.Handlers.Player.Dying += OnDeath;
        }
        public void UnRegisteringEvents()
        {
            scpPlayer.ShowHint("La Nano-regeneracion ha terminado.");
            Exiled.Events.Handlers.Player.Destroying -= OnLeft;
            Exiled.Events.Handlers.Player.Dying -= OnDeath;
        }
        private void OnLeft(DestroyingEventArgs ev)
        {
            if(ev.Player == scpPlayer)
            {
                Destroy();
            }
        }
        private void OnDeath(DyingEventArgs ev)
        {
            if(ev.Target == scpPlayer)
            {
                foreach(Player scps in Mainhandler.scp079players)
                {
                    scps.Experience += 25;
                }
                Destroy();
            }
        }
        public void CureSCP()
        {
            float amount = Plugin.Instance.Config.Abilitys.A4.CurePerTick;

            scpPlayer.Health = Mathf.Clamp(scpPlayer.Health + amount, 1, scpPlayer.MaxHealth);
        }
    }
}
