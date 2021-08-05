using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better079.System
{
    public class Plugin : Plugin<Config>
    {
        private List<Base.Handler> handlers = new List<Base.Handler>();

        private static readonly Plugin Singleton = new Plugin();

        private Plugin()
        {
        }

        /// <summary>
        /// Gets the only existing instance of this plugin.
        /// </summary>
        public static Plugin Instance => Singleton;


        public override string Name => base.Name;
        public override string Author => base.Author;

        public override void OnEnabled()
        {
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnRegisteringEvents();
            base.OnDisabled();
        }

        public void RegisterEvents()
        {
            try
            {
                Log.Info("Loading MainHandler...");
                handlers = new List<Base.Handler> { new Handlers.Mainhandler() };
                foreach (var item in handlers)
                {
                    item.Start();
                }
                Log.Info("Plugin fully loaded.");
            }
            catch (Exception e)
            {
                Log.Error($"{e.TargetSite} {e.Message}\n{e.StackTrace}");
                return;
            }

        }
        public void UnRegisteringEvents()
        {
            foreach (var item in handlers)
            {
                item.Stop();
            }
            Log.Info("Good bye.");
        }

        public override Version Version => new Version(0, 0, 1);
    }
}
