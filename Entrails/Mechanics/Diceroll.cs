using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Mechanics
{
    internal class Diceroll
    {
        private static readonly Random rand = new Random();

        public static DiceContext Dice(int max, int quantity, int mod)
        {

            DiceContext result = new DiceContext
            {
                Sides = max,
                Quantity = quantity,
                Modifier = mod
            };

            for (int i = 0; i < quantity; i++)
            {
                int cubedice = rand.Next(1, max + 1);
                result.Rolls.Add(cubedice);
                result.Sum += cubedice;
            }
            result.Total = result.Sum + mod;

            if(max == 20 && quantity == 1)
            {
                result.IsNatural20 = result.Rolls[0] == 20;
                result.IsNatural1 = result.Rolls[0] == 1;
            }
            return result;
        }

        public static void OutDice(DiceContext cubedice)
        {
            Console.WriteLine($"Бросок: {cubedice.Quantity}d{cubedice.Sides} + {cubedice.Modifier}");
            Console.WriteLine("================================");

            for (int i = 0; i < cubedice.Rolls.Count; i++)
            {
                Console.WriteLine($"Куб №{i + 1} = {cubedice.Rolls[i]}");
            }

            if (cubedice.IsNatural20)
                Console.WriteLine("Натуральная 20!");

            if (cubedice.IsNatural1)
                Console.WriteLine("Натуральная 1!");

            Console.WriteLine("================================");
            Console.WriteLine($"Сумма кубов: {cubedice.Sum}");
            Console.WriteLine($"Модификатор: {cubedice.Modifier}");
            Console.WriteLine($"Итог: {cubedice.Total}");
        }



    }
}
