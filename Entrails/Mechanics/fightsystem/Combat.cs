using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Mechanics.fightsystem
{
    internal class Combat
    {
        public List<Combatant> Participants { get; set; } = new();
        public List<string> Log { get; set; } = new();

        public int CurrentTurnIndex { get; private set; }
        public int Round { get; private set; } = 1;
        public bool IsActive { get; private set; }

        private void ResetTurnActions(Combatant combatant)
        {
            combatant.HasAction = true;
            combatant.HasBonusAction = true;
        }

        public void Start()
        {
            if (Participants.Count == 0)
            {
                Log.Add("Нельзя начать бой: нет участников.");
                return;
            }

           
            
            RollInitiative();

            IsActive = true;
            CurrentTurnIndex = 0;
            Round = 1;

            Log.Add("Бой начался.");
            Log.Add($"Первый ход: {Participants[CurrentTurnIndex].Name}");
            ResetTurnActions(Participants[CurrentTurnIndex]);
        }

        public void End()
        {
            IsActive = false;
            Log.Add("Бой завершён.");
        }

        public Combatant? GetCurrentCombatant()
        {
            if (Participants.Count == 0)
                return null;

            return Participants[CurrentTurnIndex];
        }

        public void RollInitiative()
        {
            foreach (Combatant participant in Participants)
            {
                DiceContext roll = Diceroll.Dice(20, 1, participant.DexterityModifier);
                participant.Initiative = roll.Total;

                Log.Add($"{participant.Name} бросает инициативу: {roll.Total}");
            }

            Participants = Participants
                .OrderByDescending(p => p.Initiative)
                .ThenByDescending(p => p.DexterityModifier)
                .ToList();

            Log.Add("Порядок инициативы сформирован. При равной инициативе выше ставится участник с большим модификатором ловкости.");
        }

        public void NextTurn()
        {
            if (!IsActive || Participants.Count == 0)
                return;

            CurrentTurnIndex++;

            if (CurrentTurnIndex >= Participants.Count)
            {
                CurrentTurnIndex = 0;
                Round++;
                Log.Add($"Начался раунд {Round}.");
            }

            Log.Add($"Ход: {Participants[CurrentTurnIndex].Name}");
            ResetTurnActions(Participants[CurrentTurnIndex]);
        }
        public void ChangeHp(Combatant target, int amount)
        {
            int oldHp = target.CurrentHp;

            target.CurrentHp += amount;

            if (target.CurrentHp > target.MaxHp)
                target.CurrentHp = target.MaxHp;

            if (target.CurrentHp < 0)
                target.CurrentHp = 0;

            Log.Add($"{target.Name}: HP {oldHp} -> {target.CurrentHp} ({amount}).");
        }
        public void ChangeItemQuantity(Combatant combatant, string itemName, int amount)
        {
            InventoryItem? item = combatant.Inventory
                .FirstOrDefault(i => i.Name == itemName);

            if (item == null)
            {
                if (amount <= 0)
                {
                    Log.Add($"У {combatant.Name} нет предмета {itemName}, уменьшение невозможно.");
                    return;
                }

                combatant.Inventory.Add(new InventoryItem
                {
                    Name = itemName,
                    Quantity = amount
                });

                Log.Add($"{combatant.Name} получил предмет {itemName}: {amount}.");
                return;
            }

            int oldQuantity = item.Quantity;

            item.Quantity += amount;

            if (item.Quantity < 0)
                item.Quantity = 0;

            Log.Add($"{combatant.Name}: {itemName} {oldQuantity} -> {item.Quantity} ({amount}).");
        }
        public void AddAttack(Combatant combatant, Attack attack)
        {
            combatant.Attacks.Add(attack);
            Log.Add($"{combatant.Name} получил атаку: {attack.Name}.");
        }

        public void PerformAttack(Combatant attacker, Combatant target, Attack attack)
        {
            if (!IsActive)
            {
                Log.Add("Атака невозможна: бой не начат.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(attack.RequiredAmmoName))
            {
                InventoryItem? ammo = attacker.Inventory
                    .FirstOrDefault(i => i.Name == attack.RequiredAmmoName);

                if (ammo == null || ammo.Quantity < attack.AmmoCost)
                {
                    Log.Add($"{attacker.Name} не может использовать {attack.Name}: не хватает {attack.RequiredAmmoName}.");
                    return;
                }

                ChangeItemQuantity(attacker, attack.RequiredAmmoName, -attack.AmmoCost);
            }

            DiceContext attackRoll = Diceroll.Dice(20, 1, attack.AttackBonus);

            Log.Add($"{attacker.Name} использует {attack.Name}: бросок атаки {attackRoll.Total} против КД {target.ArmorClass}.");

            if (attackRoll.Total < target.ArmorClass)
            {
                Log.Add($"{attacker.Name} промахнулся по {target.Name}.");
                return;
            }

            DiceContext damageRoll = Diceroll.Dice(
                attack.DamageDiceSides,
                attack.DamageDiceQuantity,
                attack.DamageModifier
            );

            if (attack.IsHealing)
            {
                ChangeHp(target, damageRoll.Total);
                Log.Add($"{attacker.Name} восстанавливает {target.Name} {damageRoll.Total} HP.");
                return;
            }

            ChangeHp(target, -damageRoll.Total);

            Log.Add($"{attacker.Name} попал по {target.Name}: урон {damageRoll.Total} {attack.DamageType}. HP цели: {target.CurrentHp}/{target.MaxHp}.");
        }

        public bool UseAction(Combatant combatant, string description)
        {
            if (!combatant.HasAction)
            {
                Log.Add($"{combatant.Name} уже использовал действие в этом ходу.");
                return false;
            }

            combatant.HasAction = false;
            Log.Add($"{combatant.Name} использует действие: {description}.");
            return true;
        }

        public bool UseBonusAction(Combatant combatant, string description)
        {
            if (!combatant.HasBonusAction)
            {
                Log.Add($"{combatant.Name} уже использовал бонусное действие в этом ходу.");
                return false;
            }

            combatant.HasBonusAction = false;
            Log.Add($"{combatant.Name} использует бонусное действие: {description}.");
            return true;
        }

        public bool AttackCheck(Combatant attacker, Combatant target, int attackBonus)
        {
            if (!UseAction(attacker, $"атака по {target.Name}"))
                return false;

            DiceContext attackRoll = Diceroll.Dice(20, 1, attackBonus);

            Log.Add($"{attacker.Name} атакует {target.Name}: {attackRoll.Total} против КД {target.ArmorClass}.");

            if (attackRoll.Total < target.ArmorClass)
            {
                Log.Add($"{attacker.Name} промахнулся по {target.Name}.");
                return false;
            }

            Log.Add($"{attacker.Name} попал по {target.Name}.");
            return true;
        }

        public void ApplyDamage(Combatant target, int damage)
        {
            int oldHp = target.CurrentHp;

            target.CurrentHp -= damage;

            if (target.CurrentHp < 0)
                target.CurrentHp = 0;

            Log.Add($"{target.Name} получает {damage} урона. HP {oldHp} -> {target.CurrentHp}.");
        }

        public void ApplyHealing(Combatant target, int healing)
        {
            int oldHp = target.CurrentHp;

            target.CurrentHp += healing;

            if (target.CurrentHp > target.MaxHp)
                target.CurrentHp = target.MaxHp;

            Log.Add($"{target.Name} восстанавливает {healing} HP. HP {oldHp} -> {target.CurrentHp}.");
        }


    }
}
