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
        int dodge = (this is LightUnit) ? new Random().Next(5, 11) : new Random().Next(0, 6);
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
            Console.WriteLine($"1. Добавить легкого юнита (30 поинтов)");
            Console.WriteLine($"2. Добавить тяжелого юнита (100 поинтов)");
            Console.WriteLine($"0. Вернуться в меню");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    if (points >= 30)
                    {
                        LightUnit lightUnit = new LightUnit($"Light-{Units.Count + 1}");
                        Units.Add(lightUnit);
                        points -= 30;
                        Console.WriteLine($"Добавлен легкий юнит. Осталось {points} поинтов.");
                    }
                    else
                    {
                        Console.WriteLine("Недостаточно поинтов для добавления легкого юнита.");
                    }
                    break;
                case 2:
                    if (points >= 100)
                    {
                        HeavyUnit heavyUnit = new HeavyUnit($"Heavy-{Units.Count + 1}");
                        Units.Add(heavyUnit);
                        points -= 100;
                        Console.WriteLine($"Добавлен тяжелый юнит. Осталось {points} поинтов.");
                    }
                    else
                    {
                        Console.WriteLine("Недостаточно поинтов для добавления тяжелого юнита.");
                    }
                    break;
                case 0:
                    return;
            }
        }
    }

    public void DisplayArmy()
    {
        Console.WriteLine($"Армия {Name}:");
        foreach (var unit in Units)
        {
            Console.Write($"[{unit.Name} (HP: {unit.HealthPoints})] ");
        }
        Console.WriteLine();
    }

    public void MakeMove(Army enemyArmy)
    {
        for (int i = 0; i < Units.Count; i++)
        {
            Unit attacker = Units[i];
            Unit defender = enemyArmy.Units[i];

            if (attacker.IsAlive() && defender.IsAlive())
            {
                attacker.Attack(defender);

                if (!defender.IsAlive())
                {
                    Console.WriteLine($"{defender.Name} из {enemyArmy.Name} погиб!");
                }
            }
        }
    }

    public bool IsAlive()
    {
        return Units.Any(unit => unit.IsAlive());
    }
}

public class BattleGame
{
    public Army Army1 { get; set; }
    public Army Army2 { get; set; }

    public BattleGame()
    {
        Army1 = new Army("Левая армия");
        Army2 = new Army("Правая армия");
    }

    public void CreateArmies(int army1Points, int army2Points)
    {
        Console.WriteLine("Выберите армию для создания:");
        Console.WriteLine("1. Левая армия");
        Console.WriteLine("2. Правая армия");
        Console.WriteLine("0. Вернуться в меню");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                Army1.CreateArmy(army1Points);
                break;
            case 2:
                Army2.CreateArmy(army2Points);
                break;
            case 0:
                // Вернуться в меню
                return;
        }
    }

    public void PlayUntilEnd()
    {
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
}

class Program
{
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

            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Введите количество поинтов для левой армии: ");
                    int army1Points = int.Parse(Console.ReadLine());
                    Console.Write("Введите количество поинтов для правой армии: ");
                    int army2Points = int.Parse(Console.ReadLine());

                    game.CreateArmies(army1Points, army2Points);
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

        } while (choice != 0);
    }
}
