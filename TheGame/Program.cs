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
    public LightUnit(string name) : base(name, 100, 30, 10, 0) { }
}

public class HeavyUnit : Unit
{
    public HeavyUnit(string name) : base(name, 200, 50, 20, 0) { }
}

public class Army
{
    private const int LightUnitCost = 30;
    private const int HeavyUnitCost = 100;

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
            Console.WriteLine($"0. Вернуться в меню");

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
        if (Units.Count > 0 && enemyArmy.Units.Count > 0)
        {
            // Атака юнита из левой армии
            Unit attacker1 = Units[0];
            Unit defender1 = enemyArmy.Units[0];

            if (attacker1.IsAlive() && defender1.IsAlive())
            {
                Console.WriteLine($"{attacker1.Name} из {Name} наносит урон, и {defender1.Name} из {enemyArmy.Name} получает урон.");
                attacker1.Attack(defender1);

                if (!defender1.IsAlive())
                {
                    Console.WriteLine($"{defender1.Name} из {enemyArmy.Name} погиб!");
                    enemyArmy.Units.RemoveAt(0); // Удаляем убитого юнита из армии противника
                }
                else
                {
                    Console.WriteLine($"{defender1.Name} из {enemyArmy.Name} остается с {defender1.HealthPoints} HP.");
                }
            }

            // Атака юнита из правой армии
            if (enemyArmy.Units.Count > 0)
            {
                Unit attacker2 = enemyArmy.Units[0];
                Unit defender2 = Units[0];

                if (attacker2.IsAlive() && defender2.IsAlive())
                {
                    Console.WriteLine($"{attacker2.Name} из {enemyArmy.Name} наносит урон, и {defender2.Name} из {Name} получает урон.");
                    attacker2.Attack(defender2);

                    if (!defender2.IsAlive())
                    {
                        Console.WriteLine($"{defender2.Name} из {Name} погиб!");
                        Units.RemoveAt(0); // Удаляем убитого юнита из текущей армии
                    }
                    else
                    {
                        Console.WriteLine($"{defender2.Name} из {Name} остается с {defender2.HealthPoints} HP.");
                    }
                }
            }

            // Удаляем убитых юнитов из обеих армий
            Units.RemoveAll(unit => !unit.IsAlive());
            enemyArmy.Units.RemoveAll(unit => !unit.IsAlive());
            if (Units.Count == 0)
            {
                Console.WriteLine($"{Name} армия победила!");
            }
            else if (enemyArmy.Units.Count == 0)
            {
                Console.WriteLine($"{enemyArmy.Name} армия победила!");
            }
        }
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

public class BattleGame
{
    private Army initialArmy1;
    private Army initialArmy2;

    public Army Army1 { get; set; }
    public Army Army2 { get; set; }

    private Army currentAttacker;
    private Army currentDefender;

    public BattleGame()
    {
        initialArmy1 = new Army("Левая армия");
        initialArmy2 = new Army("Правая армия");
    }

    public void CreateArmies()
    {
        Console.WriteLine("Создание левой армии:");
        initialArmy1.CreateArmy(450);

        Console.WriteLine("Создание правой армии:");
        initialArmy2.CreateArmy(450);

        // Создаем копии армий для текущего хода
        Army1 = new Army("Левой армия");
        Army2 = new Army("Правой армия");

        // Копируем состояние из начальных армий
        Army.CopyArmyState(initialArmy1, Army1);
        Army.CopyArmyState(initialArmy2, Army2);
    }

    public void PlayUntilEnd()
    {
        // Перед началом боя копируем состояние из начальных армий
        Army.CopyArmyState(initialArmy1, Army1);
        Army.CopyArmyState(initialArmy2, Army2);

        while (Army1.IsAlive() && Army2.IsAlive())
        {
            Army1.MakeMove(Army2);
            Army2.MakeMove(Army1);

            Console.WriteLine();
            Army1.DisplayArmy();
            Console.WriteLine();
            Army2.DisplayArmy();
        }

        Console.WriteLine(Army1.IsAlive() ? $"{Army1.Name} победила!" : $"{Army2.Name} победила!");
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
        BattleGame game = new BattleGame();

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
