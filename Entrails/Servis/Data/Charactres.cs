using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNDAPP.Entrails.Mechanics.fightsystem;

namespace DNDAPP.Entrails.Servis.Data
{
    internal class Charactres
    {
        public string Name { get; set; } = "";
        public string Race { get; set; } = "";
        public string ClassName { get; set; } = "";
        public int Level { get; set; }

        public int ArmorClass { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int Speed { get; set; }

        public int ProficiencyBonus { get; set; }

        public AbilityScores Abilities { get; set; } = new();
        public List<Attack> Attacks { get; set; } = new();
        public List<InventoryItem> Inventory { get; set; } = new();
    }

    internal class AbilityScores
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

}
