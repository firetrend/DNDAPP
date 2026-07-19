using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Mechanics.fightsystem
{
    internal class Attack
    {
        public string Name { get; set; } = "";
        public AttackType Type { get; set; }

        public int AttackBonus { get; set; }

        public int DamageDiceQuantity { get; set; }
        public int DamageDiceSides { get; set; }
        public int DamageModifier { get; set; }

        public string DamageType { get; set; } = "";

        public string? RequiredAmmoName { get; set; }
        public int AmmoCost { get; set; }

        public bool IsHealing { get; set; }
    }
}
