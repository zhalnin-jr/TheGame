using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Army
{
    private const int LIGHTUNITCOST = 40;
    private const int HEAVYUNITCOST = 80;
    private const int MAGEUNITCOST = 150;
    private const int ARCHERUNITCOST = 100;
    private const int HEALERUNITCOST = 50;
    public string Name { get; set; }
    public List<Unit> Units { get; set; }

    public Army(string name)
    {
        Name = name;
        Units = new List<Unit>();
    }
    public void AddUnit(UnitFactories.IUnitFactory factory, string name)
    {
        var unit = factory.CreateUnit(name);
        Units.Add(unit);
        FrontManager.GetInstance().Printer($"{unit.GetType().Name} {name} добавлен в армию {Name}.");
    }

    public void CreateArmy(int points)
    {
        var lightFactory = new UnitFactories.LightUnitFactory();
        var heavyFactory = new UnitFactories.HeavyUnitFactory();
        var archerFactory = new UnitFactories.ArcherUnitFactory();
        var magaFactory = new UnitFactories.MageUnitFactory();
        var healerFactory = new UnitFactories.HealerUnitFactory();

        FrontManager.GetInstance().Printer($"Создание армии {Name}");
        while (points > 0)
        {
            FrontManager.GetInstance().Printer($"1. Добавить легкого юнита ({LIGHTUNITCOST} поинтов)");
            FrontManager.GetInstance().Printer($"2. Добавить тяжелого юнита ({HEAVYUNITCOST} поинтов)");
            FrontManager.GetInstance().Printer($"3. Добавить archer юнита ({ARCHERUNITCOST} поинтов)");
            FrontManager.GetInstance().Printer($"4. Добавить MAGA юнита ({MAGEUNITCOST} поинтов)");
            FrontManager.GetInstance().Printer($"5. Добавить Cleric юнита ({HEALERUNITCOST} поинтов)");
            FrontManager.GetInstance().Printer($"0. Следующее действие");

            try
            {
                var choice = ReadIntegerInput();

                switch (choice)
                {
                    case 1:
                        if (CanAddLightUnit(points))
                        {
                            AddUnit(lightFactory, $"L{Units.Count + 1}");
                            points -= LIGHTUNITCOST;
                            FrontManager.GetInstance().Printer($"У вас осталось {points} поинтов");
                        }
                        else
                        {
                            FrontManager.GetInstance().Printer("Недостаточно поинтов для добавления легкого юнита.");
                        }
                        break;
                    case 2:
                        if (CanAddHeavyUnit(points))
                        {
                            AddUnit(heavyFactory, $"H{Units.Count + 1}");
                            points -= HEAVYUNITCOST;
                            FrontManager.GetInstance().Printer($"У вас осталось {points} поинтов");
                        }
                        else
                        {
                            FrontManager.GetInstance().Printer("Недостаточно поинтов для добавления тяжелого юнита.");
                        }
                        break;
                    case 3:
                        if (CanAddArcherUnit(points))
                        {
                            AddUnit(archerFactory, $"A{Units.Count + 1}"); // Используем фабрику для создания лучника и добавляем его в армию с учетом порядкового номера
                            points -= ARCHERUNITCOST;
                            FrontManager.GetInstance().Printer($"У вас осталось {points} поинтов");
                        }
                        else
                        {
                            FrontManager.GetInstance().Printer("Недостаточно поинтов для добавления лучника.");
                        }
                        break;
                    case 4:
                        if (CanAddMageUnit(points))
                        {
                            AddUnit(magaFactory, $"M{Units.Count + 1}");
                            points -= MAGEUNITCOST;
                            FrontManager.GetInstance().Printer($"У вас осталось {points} поинтов");
                        }
                        else
                        {
                            FrontManager.GetInstance().Printer("Недостаточно поинтов для добавления лучника.");
                        }
                        break;
                        case 5:
                            if (CanAddHealerUnit(points))
                        {
                            AddUnit(healerFactory, $"C{Units.Count + 1}");
                            points -= HEALERUNITCOST;
                            FrontManager.GetInstance().Printer($"У вас осталось {points} поинтов");
                        }
                        else
                        {
                            FrontManager.GetInstance().Printer("Недостаточно поинтов для добавления лучника.");
                        }
                        break;
                    case 0:
                        return;
                    default:
                        FrontManager.GetInstance().Printer("Неверный выбор. Пожалуйста, введите 1, 2 или 0.");
                        break;
                }
            }
            catch (FormatException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введите число.");
            }
            catch (OverflowException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введено слишком большое число.");
            }
        }
    }

    public void DisplayArmy()
    {
        FrontManager.GetInstance().Printer($"Армия {Name}:");
        var armyRepresentation = "";
        foreach (var unit in Units)
        {
            /*FrontManager.GetInstance().Printer($"Unit: {unit}\n");*/
            if (unit is LightUnit)
            {
                armyRepresentation += "{L}";
            }
            else if (unit is HeavyUnit)
            {
                armyRepresentation += "{H}";
            }
            else if (unit is ArcherUnit)
            {
                armyRepresentation += "{A}";
            }
            else if (unit is MageUnit)
            {
                armyRepresentation += "{M}";
            }
            else if (unit is HeavyUnit)
            {
                armyRepresentation += "{C}";
            }
        }
        FrontManager.GetInstance().Printer(armyRepresentation);
    }

    public void MakeMove(Army enemyArmy)
    {
        Army currentArmy = this;
        Army opposingArmy = enemyArmy;
        Unit attacker = Units[0];
        Unit defender = enemyArmy.Units[0];

        while (attacker.IsAlive() && defender.IsAlive())
        {
            // Ход текущей армии.
            foreach (var unit in currentArmy.Units)
            {
                if (unit.IsAlive() && currentArmy.Units.IndexOf(unit) == 0)
                {
                    unit.Attack(opposingArmy.Units[0]);

                    // Проверяем, умер ли противник после атаки.
                    if (!opposingArmy.Units[0].IsAlive())
                    {
                        FrontManager.GetInstance().Printer($"Пехотинец {opposingArmy.Units[0].Name} умер.");
                    }
                }
                else if (unit is ArcherUnit archer)
                {
                    archer.AttackWithRange(opposingArmy.Units);
                }
                else if (unit is MageUnit mage)
                {
                    mage.CloneAdjacentLightUnit(mage, currentArmy.Units);
                }
                else if (unit is HealerUnit healer)
                {
                    healer.HealFirstUnitWithChance(opposingArmy.Units);
                }
            }

            // Удаление погибших юнитов после хода текущей армии.
            currentArmy.Units.RemoveAll(unit => !unit.IsAlive());
            opposingArmy.Units.RemoveAll(unit => !unit.IsAlive());

            // Меняем местами армии.
            var temp = currentArmy;
            currentArmy = opposingArmy;
            opposingArmy = temp;
        }

        if (!currentArmy.IsAlive())
        {
            FrontManager.GetInstance().Printer($"{opposingArmy.Name} победила!");
        }
        else if (!opposingArmy.IsAlive())
        {
            FrontManager.GetInstance().Printer($"{currentArmy.Name} победила!");
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
                FrontManager.GetInstance().Printer("Ошибка ввода. Введите число.");
            }
            catch (OverflowException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введено слишком большое число.");
            }
        }
    }

    private bool CanAddLightUnit(int points) => points >= LIGHTUNITCOST;
    private bool CanAddHeavyUnit(int points) => points >= HEAVYUNITCOST;
    private bool CanAddArcherUnit(int points) => points >= ARCHERUNITCOST;
    private bool CanAddMageUnit(int points) => points >= MAGEUNITCOST;
    private bool CanAddHealerUnit(int points) => points >= HEALERUNITCOST;


    public static void CopyArmyState(Army source, Army destination)
    {
        // Очищаем армию назначения перед копированием.
        destination.Units.Clear();

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
            else if (unit is ArcherUnit)
            {
                destination.Units.Add(new ArcherUnit(unit.Name));
            }
            else if (unit is MageUnit)
            {
                destination.Units.Add(new MageUnit(unit.Name));
            }
            else if (unit is MageUnit)
            {
                destination.Units.Add(new HealerUnit(unit.Name));
            }
        }
    }
}
