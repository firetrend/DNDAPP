using DNDAPP.Entrails.Mechanics;
using DNDAPP.Entrails.Servis.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNDAPP.Entrails.Mechanics.fightsystem;

namespace DNDAPP.Entrails.Servis
{

    internal class Menu
    {
        private List<Charactres> _characters = new();
        private readonly Loader _loader;

        public Menu(Loader loader)
        {
            _loader = loader;
        }

        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine("1. Загрузить персонажей");
                Console.WriteLine("2. Показать персонажей");
                Console.WriteLine("3. Бросить кубы");
                Console.WriteLine("4. Проверка характеристики персонажа");
                Console.WriteLine("5. Боевой режим");
                Console.WriteLine("6. Выход");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        LoadCharacters();
                        break;

                    case "2":
                        ShowCharacters();
                        break;

                    case "3":
                        RollDice();
                        break;

                    case "4":
                        AbilityCheck();
                        break;

                    case "5":
                        StartCombatMode();
                        break;

                    case "6":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        Pause();
                        break;
                }
            }
        }



        private void LoadCharacters()
        {
            Console.Clear();

            _characters = _loader.LoadAll();

            Console.WriteLine($"Загружено персонажей: {_characters.Count}");

            if (_characters.Count == 0)
                Console.WriteLine("Проверьте папку InputCharacters и наличие JSON-файлов.");

            Pause();
        }

        private void ShowCharacters()
        {
            Console.Clear();

            Console.WriteLine("=== Персонажи ===");

            if (_characters.Count == 0)
            {
                Console.WriteLine("Персонажи не загружены.");
                Pause();
                return;
            }

            for (int i = 0; i < _characters.Count; i++)
            {
                Charactres character = _characters[i];

                Console.WriteLine($"{i + 1}. {character.Name} | {character.Race} | {character.ClassName} {character.Level} ур. | HP {character.CurrentHp}/{character.MaxHp} | КД {character.ArmorClass}");
            }

            Pause();
        }

        private void RollDice()
        {
            Console.Clear();

            Console.WriteLine("=== Бросок кубов ===");

            Console.Write("Сколько граней у куба: ");
            int sides = ReadInt();

            Console.Write("Сколько кубов: ");
            int quantity = ReadInt();

            Console.Write("Модификатор: ");
            int modifier = ReadInt();

            DiceContext result = Diceroll.Dice(sides, quantity, modifier);
            Diceroll.OutDice(result);

            Pause();
        }

        private void AbilityCheck()
        {
            Console.Clear();

            if (_characters.Count == 0)
            {
                Console.WriteLine("Персонажи не загружены.");
                Pause();
                return;
            }

            Console.WriteLine("=== Проверка характеристики ===");

            for (int i = 0; i < _characters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_characters[i].Name}");
            }

            Console.Write("Выберите персонажа: ");
            int characterIndex = ReadInt() - 1;

            if (characterIndex < 0 || characterIndex >= _characters.Count)
            {
                Console.WriteLine("Неверный номер персонажа.");
                Pause();
                return;
            }

            Charactres character = _characters[characterIndex];

            Console.WriteLine("1. Сила");
            Console.WriteLine("2. Ловкость");
            Console.WriteLine("3. Телосложение");
            Console.WriteLine("4. Интеллект");
            Console.WriteLine("5. Мудрость");
            Console.WriteLine("6. Харизма");
            Console.Write("Выберите характеристику: ");

            int abilityChoice = ReadInt();

            int modifier = GetAbilityModifier(character, abilityChoice);

            DiceContext result = Diceroll.Dice(20, 1, modifier);

            Console.WriteLine($"Проверка персонажа: {character.Name}");
            Diceroll.OutDice(result);

            Pause();
        }

        private int GetAbilityModifier(Charactres character, int choice)
        {
            int score = choice switch
            {
                1 => character.Abilities.Strength,
                2 => character.Abilities.Dexterity,
                3 => character.Abilities.Constitution,
                4 => character.Abilities.Intelligence,
                5 => character.Abilities.Wisdom,
                6 => character.Abilities.Charisma,
                _ => 10
            };

            return (score - 10) / 2;
        }

        private int ReadInt()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.Write("Введите число: ");
            }
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
        }
        private void StartCombatMode()
        {
            if (_characters.Count == 0)
            {
                Console.WriteLine("Персонажи не загружены. Сначала загрузите персонажей.");
                Pause();
                return;
            }

            Combat combat = new Combat();

            foreach (Charactres character in _characters)
            {
                Combatant combatant = CombatantFactory.FromCharacter(character);
                combat.Participants.Add(combatant);
            }

            combat.Start();

            CombatMenu combatMenu = new CombatMenu(combat);
            combatMenu.Run();

            Pause();
        }
    }
}
