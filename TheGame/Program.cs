using System;
using System.Collections.Generic;
using System.Linq;

public class Unit
{
    public string Name { get; set; }
    public int HealthPoints { get; set; }
    public int AttackPoints { get; set; }
    public int DefensePoints { get; set; }
    public int DodgeChance { get; set; }

    private static readonly Random random = new Random();

    public Unit(string name, int healthPoints, int attackPoints, int defensePoints, int dodgeChance)
    {
        Name = name;
        HealthPoints = healthPoints;
        AttackPoints = attackPoints;
        DefensePoints = defensePoints;
        DodgeChance = dodgeChance;
    }

    public void Attack(Unit target)
    {
        int dodge = (this is LightUnit) ? random.Next(5, 11) : random.Next(0, 6);
        int damage = Math.Max(0, AttackPoints - target.DefensePoints - dodge);
        target.HealthPoints -= damage;
        Console.WriteLine($"{Name} атакует {target.Name} и наносит {damage} урона.");
    }

    public bool IsAlive()
    {
        return HealthPoints > 0;
    }
}

public class LightUnit : Unit
{
    public LightUnit(string name) : base(name, 150, 80, 20, 0) { }
}

public class HeavyUnit : Unit
{
    public HeavyUnit(string name) : base(name, 200, 40, 60, 0) { }
}

public class Army
{
    private const int LightUnitCost = 40;
    private const int HeavyUnitCost = 80;

    public string Name { get; set; }
    public List<Unit> Units { get; set; }

    public Army(string name)
    {
        Name = name;
        Units = new List<Unit>();
    }

    public void CreateArmy(int points)
    {
        Console.WriteLine($"Создание армии {Name}");
        Console.WriteLine($"Осталось {points} поинтов.");

        while (points > 0)
        {
            Console.WriteLine($"1. Добавить легкого юнита ({LightUnitCost} поинтов)");
            Console.WriteLine($"2. Добавить тяжелого юнита ({HeavyUnitCost} поинтов)");
            Console.WriteLine($"0. Следующее действие");

            try
            {
                int choice = ReadIntegerInput();

                switch (choice)
                {
                    case 1:
                        if (CanAddLightUnit(points))
                        {
                            LightUnit lightUnit = new LightUnit($"L{Units.Count + 1}");
                            Units.Add(lightUnit);
                            points -= LightUnitCost;
                            Console.WriteLine($"Добавлен легкий юнит. Осталось {points} поинтов.");
                        }
                        else
                        {
                            Console.WriteLine("Недостаточно поинтов для добавления легкого юнита.");
                        }
                        break;
                    case 2:
                        if (CanAddHeavyUnit(points))
                        {
                            HeavyUnit heavyUnit = new HeavyUnit($"H{Units.Count + 1}");
                            Units.Add(heavyUnit);
                            points -= HeavyUnitCost;
                            Console.WriteLine($"Добавлен тяжелый юнит. Осталось {points} поинтов.");
                        }
                        else
                        {
                            Console.WriteLine("Недостаточно поинтов для добавления тяжелого юнита.");
                        }
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, введите 1, 2 или 0.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода. Введите число.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка ввода. Введено слишком большое число.");
            }
        }
    }

    public void DisplayArmy()
    {
        Console.WriteLine($"Армия {Name}:");
        string armyRepresentation = string.Join("", Units.Select(unit => unit is LightUnit ? "L" : "H"));
        Console.WriteLine(armyRepresentation);
    }

    public void MakeMove(Army enemyArmy)
    {
        for (int i = 0; i < Units.Count; i++)
        {
            if (i >= enemyArmy.Units.Count) break; // Проверка на случай, если в армии противника меньше юнитов

            Unit attacker = Units[0];
            Unit defender = enemyArmy.Units[00];

            // Обмениваются ударами до смерти одного из них
            while (attacker.IsAlive() && defender.IsAlive())
            {
                attacker.Attack(defender);
                if (defender.IsAlive()) // Проверка после каждой атаки, жив ли защитник
                {
                    defender.Attack(attacker); // Защитник контратакует, если жив
                }
                else
                {
                    Console.WriteLine($"{defender.Name} из {enemyArmy.Name} погиб!");
                }

                if (!attacker.IsAlive()) // Проверка после контратаки, жив ли атакующий
                {
                    Console.WriteLine($"{attacker.Name} из {Name} погиб!");
                }
            }
        }

        // Удаление погибших юнитов после каждого обмена ударами
        Units.RemoveAll(unit => !unit.IsAlive());
        enemyArmy.Units.RemoveAll(unit => !unit.IsAlive());
    }


    public bool IsAlive()
    {
        return Units.Any(unit => unit.IsAlive());
    }

    private static int ReadIntegerInput()
    {
        while (true)
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода. Введите число.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка ввода. Введено слишком большое число.");
            }
        }
    }

    private bool CanAddLightUnit(int points) => points >= LightUnitCost;
    private bool CanAddHeavyUnit(int points) => points >= HeavyUnitCost;

    public static void CopyArmyState(Army source, Army destination)
    {
        destination.Units.Clear(); // Очищаем армию назначения перед копированием

        foreach (var unit in source.Units)
        {
            if (unit is LightUnit)
            {
                destination.Units.Add(new LightUnit(unit.Name));
            }
            else if (unit is HeavyUnit)
            {
                destination.Units.Add(new HeavyUnit(unit.Name));
            }
        }
    }
}

// Singleton - это паттерн проектирования, который гарантирует, что у класса есть только один экземпляр, и предоставляет глобальную точку доступа к этому экземпляру.
public class BattleGame
{
    // Instance - хранит единственный экземпляр класса BattleGame.
    private Army initialArmy1;
    private Army initialArmy2;
    private static BattleGame instance;
    private static readonly object lockObj = new object();


    public Army Army1 { get; set; }
    public Army Army2 { get; set; }

    // Приватный конструктор, который вызывается только внутри класса, что предотвращает создание экземпляров извне.
    private BattleGame()
    {
        initialArmy1 = new Army("Левая армия");
        initialArmy2 = new Army("Правая армия");
    }

    // Instance - предоставляет глобальную точку доступа к единственному экземпляру класса BattleGame. Если экземпляр не создан, он создается при первом вызове.
    public static BattleGame Instance
    {
        get
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new BattleGame();
                }
                return instance;
            }
        }
    }

    public void CreateArmies()
    {
        Console.WriteLine("Создание левой армии:");
        initialArmy1.CreateArmy(450);

        Console.WriteLine("Создание правой армии:");
        initialArmy2.CreateArmy(450);

        // Создаем копии армий для текущего хода
        Army1 = new Army("Левой армии");
        Army2 = new Army("Правой армии");

        // Копируем состояние из начальных армий
        Army.CopyArmyState(initialArmy1, Army1);
        Army.CopyArmyState(initialArmy2, Army2);
    }

    public void PlayUntilEnd()
    {
        while (Army1.IsAlive() && Army2.IsAlive())
        {
            Army1.MakeMove(Army2);
            if (!Army2.IsAlive()) // Проверяем, остались ли живые юниты в армии 2 после хода армии 1
            {
                Console.WriteLine($"{Army1.Name} победила!");
                return; // Завершаем игру, если армия 2 уничтожена
            }

            Army2.MakeMove(Army1);
            if (!Army1.IsAlive()) // Проверяем, остались ли живые юниты в армии 1 после хода армии 2
            {
                Console.WriteLine($"{Army2.Name} победила!");
                return; // Завершаем игру, если армия 1 уничтожена
            }
        }

        // Если цикл выше прервался без объявления победителя, проверяем, кто выиграл
        if (!Army1.IsAlive() && Army2.IsAlive())
        {
            Console.WriteLine($"{Army2.Name} победила!");
        }
        else if (Army1.IsAlive() && !Army2.IsAlive())
        {
            Console.WriteLine($"{Army1.Name} победила!");
        }
        else
        {
            Console.WriteLine("Битва окончена ничьей."); // В случае, если обе армии были уничтожены одновременно
        }
    }

    private static int ReadIntegerInput()
    {
        while (true)
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода. Введите число.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка ввода. Введено слишком большое число.");
            }
        }
    }
    static void Main()
    {
        BattleGame game = BattleGame.Instance;

        int choice;
        do
        {
            Console.WriteLine("1. Создать армию");
            Console.WriteLine("2. Сделать ход");
            Console.WriteLine("3. Доиграть игру до конца");
            Console.WriteLine("0. Выйти");

            try
            {
                choice = ReadIntegerInput();

                switch (choice)
                {
                    case 1:
                        game.CreateArmies();
                        break;
                    case 2:
                        game.Army1.MakeMove(game.Army2);
                        Console.WriteLine();
                        game.Army1.DisplayArmy();
                        Console.WriteLine();
                        game.Army2.DisplayArmy();
                        break;
                    case 3:
                        game.PlayUntilEnd();
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода. Введите число.");
                choice = -1;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка ввода. Введено слишком большое число.");
                choice = -1;
            }

        } while (choice != 0);
    }
}
