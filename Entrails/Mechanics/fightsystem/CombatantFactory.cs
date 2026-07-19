using DNDAPP.Entrails.Servis.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Mechanics.fightsystem
{
    internal static class CombatantFactory
    {
        public static Combatant FromCharacter(Charactres character)
        {
            return new Combatant
            {
                Name = character.Name,
                Type = "Player",
                ArmorClass = character.ArmorClass,
                MaxHp = character.MaxHp,
                CurrentHp = character.CurrentHp,
                DexterityModifier = GetAbilityModifier(character.Abilities.Dexterity)
            };
        }

        private static int GetAbilityModifier(int abilityScore)
        {
            return (int)Math.Floor((abilityScore - 10) / 2.0);
        }
    }
}
