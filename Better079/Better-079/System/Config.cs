using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace Better079.System
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string CommandPrefix { get; set; } = "s079";
        public string SpawnMsg { get; set; } = "<color=#329600>[SCP-079 Plus]</color><color=#08c3d4> Escribe \".079\" en la consola abriendola con la Ñ para ver las habilidades que tienes.</color>";
        public AbilityConfigs Abilitys { get; set; } = new AbilityConfigs();
    }
}
