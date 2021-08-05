using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better079.System
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public class AbilityConfigs
    {
        /// <summary>
        /// Overload Ability.
        /// </summary>
        public OverLoadAbilityConfig A1 { get; set; } = new OverLoadAbilityConfig();
        /// <summary>
        /// Poison Ability.
        /// </summary>
        public PoisonAbilityConfig A2 { get; set; } = new PoisonAbilityConfig();
        /// <summary>
        /// Flash Ability.
        /// </summary>
        public FlashAbilityConfig A3 { get; set; } = new FlashAbilityConfig();
        /// <summary>
        /// Cure Scp Ability.
        /// </summary>
        public CureAbilityConfig A4 { get; set; } = new CureAbilityConfig();

        /// <summary>
        /// Find SCP Ability
        /// </summary>
        public FiendAbilityConfig A5 { get; set; } = new FiendAbilityConfig();
    }
    public class OverLoadAbilityConfig
    {
        public float OverloadCoolDown { get; set; } = 85f;
        public float OverloadDuration { get; set; } = 15f;
    }
    public class PoisonAbilityConfig
    {
        public float PoisonCooldown { get; set; } = 45;
        public float PoisonDuration { get; set; } = 15f;
        public float PoisonEnergyCost { get; set; } = 85;

    }
    public class FlashAbilityConfig
    {
        public float FlashCooldown { get; set; } = 85f;
        public float FlashEnergyCost { get; set; } = 85f;
    }
    public class CureAbilityConfig
    {
        public float CureCooldown { get; set; } = 35f;
        public float CurePerTick { get; set; } = 5f;

        public int CureDuration { get; set; } = 15;
        public float CureEnergyCost { get; set; } = 100f;

    }
    public class FiendAbilityConfig
    {
        public float FindCooldown { get; set; } = 10f;
    }

#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
}
