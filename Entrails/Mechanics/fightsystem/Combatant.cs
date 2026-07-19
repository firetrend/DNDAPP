using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Mechanics.fightsystem
{
    internal class Combatant
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "Player";

        public int ArmorClass { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }

        public int DexterityModifier { get; set; }
        public int Initiative { get; set; }

        public List<Attack> Attacks { get; set; } = new();
        public List<InventoryItem> Inventory { get; set; } = new();
        public List<string> Conditions { get; set; } = new();

        public bool HasAction { get; set; } = true;
        public bool HasBonusAction { get; set; } = true;
        public bool IsAlive => CurrentHp > 0;
    }
}
