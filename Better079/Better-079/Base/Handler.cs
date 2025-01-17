﻿using Better079.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better079.Base
{
    public abstract class Handler
    {
        /// <summary>
        /// Plugin Singleton instance.
        /// </summary>
        protected Plugin plugin => Plugin.Instance;

        /// <summary>
        /// Activated when you start the Plugin, use it to initialize your variables and the class.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Activated when the plugin is deactivated or when the server is restarted, use it to clear variables and class.
        /// </summary>
        public abstract void Stop();
    }
}
