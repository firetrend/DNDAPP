using DNDAPP.Entrails.Mechanics;
using DNDAPP.Entrails.Servis;
using DNDAPP.Entrails.Servis.Data;

namespace DNDAPP
{
    class programm
    {
        static void Main(string[] args)
        {

            string inputPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "InputCharactres")
            );

            Loader loader = new Loader(inputPath);

            Menu menu = new Menu(loader);
            menu.Run();

            //Для проверку работоспособности:
            /*
            Console.WriteLine($"Текущая папка: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Ищу тут: {Path.GetFullPath("InputCharactres")}");
            Console.WriteLine($"Папка существует: {Directory.Exists("InputCharactres")}");

            Loader loader = new Loader("InputCharactres");

            List<Charactres> characters = loader.LoadAll();

            Console.WriteLine($"Загружено персонажей: {characters.Count}");

            foreach (Charactres character in characters)
            {
                Console.WriteLine("==============================");
                Console.WriteLine($"Имя: {character.Name}");
                Console.WriteLine($"Раса: {character.Race}");
                Console.WriteLine($"Класс: {character.ClassName}");
                Console.WriteLine($"Уровень: {character.Level}");
                Console.WriteLine($"HP: {character.CurrentHp}/{character.MaxHp}");
                Console.WriteLine($"КД: {character.ArmorClass}");
                Console.WriteLine($"Скорость: {character.Speed}");
            }
            
            Console.WriteLine("Начал");
            Console.Write("Сколькиграневый куб:");
            int max = int.Parse(Console.ReadLine()!);
            Console.WriteLine("   ");
            Console.Write("Сколько кубов:");
            int quantity = int.Parse(Console.ReadLine()!);
            Console.WriteLine("   ");
            Console.Write("Сумму модификаторов:");
            int mod = int.Parse(Console.ReadLine()!);
            Console.WriteLine("   ");

            DiceContext result = Diceroll.Dice(max, quantity, mod);
            Diceroll.OutDice(result);
            */
        }  
    }
}