using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNDAPP.Entrails.Mechanics;

namespace DNDAPP.Entrails.Mechanics.fightsystem
{
    internal class CombatMenu
    {
        private readonly Combat _combat;

        public CombatMenu(Combat combat)
        {
            _combat = combat;
        }

        public void Run()
        {
            bool exit = false;

            while (!exit && _combat.IsActive)
            {
                Console.WriteLine("=== Боевой режим ===");
                Console.WriteLine($"Раунд: {_combat.Round}");
                Console.WriteLine($"Ход: {_combat.GetCurrentCombatant()?.Name}");
                Console.WriteLine("1. Показать участников");
                Console.WriteLine("2. Атака");
                Console.WriteLine("3. Следующий ход");
                Console.WriteLine("4. Показать логи");
                Console.WriteLine("5. Завершить бой");

                string input = Console.ReadLine()!;

                switch (input)
                {
                    case "1":
                        ShowParticipants();
                        break;

                    case "2":
                        AttackAction();
                        break;

                    case "3":
                        _combat.NextTurn();
                        break;

                    case "4":
                        ShowLog();
                        break;

                    case "5":
                        _combat.End();
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }

        private void ShowParticipants()
        {
            foreach (Combatant participant in _combat.Participants)
            {
                Console.WriteLine($"{participant.Name} | HP: {participant.CurrentHp}/{participant.MaxHp} | КД: {participant.ArmorClass} | Иниц.: {participant.Initiative}");
            }
        }

        private void ShowLog()
        {
            foreach (string log in _combat.Log)
            {
                Console.WriteLine(log);
            }
        }

        private Combatant? SelectParticipant()
        {
            Console.WriteLine("Выберите участника:");

            for (int i = 0; i < _combat.Participants.Count; i++)
            {
                Combatant participant = _combat.Participants[i];

                Console.WriteLine($"{i + 1}. {participant.Name} | HP: {participant.CurrentHp}/{participant.MaxHp} | КД: {participant.ArmorClass}");
            }

            Console.Write("Номер: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int number))
                return null;

            int index = number - 1;

            if (index < 0 || index >= _combat.Participants.Count)
                return null;

            return _combat.Participants[index];
        }

        private int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.WriteLine("Введите число.");
            }
        }

        private void AttackAction()
        {
            Combatant? attacker = _combat.GetCurrentCombatant();

            if (attacker == null)
            {
                Console.WriteLine("Нет текущего участника боя.");
                return;
            }

            Console.WriteLine($"Атакующий: {attacker.Name}");

            Combatant? target = SelectParticipant();

            if (target == null)
            {
                Console.WriteLine("Цель не выбрана.");
                return;
            }

            if (target == attacker)
            {
                Console.WriteLine("Нельзя атаковать самого себя.");
                _combat.Log.Add($"{attacker.Name} попытался атаковать самого себя. Действие отменено.");
                return;
            }

            int attackBonus = ReadInt("Бонус атаки: ");

            bool isHit = _combat.AttackCheck(attacker, target, attackBonus);

            if (!isHit)
                return;

            int quantity = ReadInt("Количество кубов урона: ");
            int sides = ReadInt("Количество граней у куба урона: ");
            int modifier = ReadInt("Модификатор урона: ");

            if (quantity <= 0 || sides <= 0)
            {
                Console.WriteLine("Количество кубов и граней должно быть больше 0.");
                _combat.Log.Add("Расчёт урона отменён: некорректные параметры кубов.");
                return;
            }

            DiceContext damageRoll = Diceroll.Dice(sides, quantity, modifier);

            _combat.ApplyDamage(target, damageRoll.Total);

            Console.WriteLine($"Урон: {quantity}d{sides} + {modifier} = {damageRoll.Total}");
            _combat.Log.Add($"Урон рассчитан броском {quantity}d{sides} + {modifier}: {damageRoll.Total}.");
        }
    }
}
