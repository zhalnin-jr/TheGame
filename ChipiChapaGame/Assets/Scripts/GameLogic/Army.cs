using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.UI;
using static UnitFactories;

/// <summary>
/// Отвечает за армию.
/// </summary>
public class Army
{
    private const int LIGHTUNITCOST = 40;
    private const int HEAVYUNITCOST = 80;
    private const int MAGEUNITCOST = 150;
    private const int ARCHERUNITCOST = 100;
    private const int HEALERUNITCOST = 50;
    public string Name { get; set; }
    public List<Unit> Units { get; set; }

    IUnitFactory lightFactory = new LightUnitFactory();
    IUnitFactory heavyFactory = new HeavyUnitFactory();
    IUnitFactory archerFactory = new ArcherUnitFactory();
    IUnitFactory magaFactory = new MageUnitFactory();
    IUnitFactory healerFactory = new HealerUnitFactory();

    private int points;
    private Action finishAction = null;

    public Army(string name)
    {
        Name = name;
        Units = new List<Unit>();
    }

    /// <summary>
    /// Добавление юнита в армию при создании.
    /// </summary>
    /// <param name="factory"> - наша фабрика. </param>
    /// <param name="name"> - выбранный юнит. </param>
    public void AddUnit(UnitFactories.IUnitFactory factory, string name)
    {
        var unit = factory.CreateUnit(name);
        Units.Add(unit);
        FrontManager.Instance.Printer($"{unit.GetType().Name} {name} добавлен в армию {Name}.");
        FrontManager.Instance.Printer($"У вас осталось {points} поинтов");
        ArmyCreation();
    }

    /// <summary>
    /// При создании лёгкого вычитаем из общей суммы его поинты.
    /// </summary>
    public void AddLightUnit()
    {
        points -= LIGHTUNITCOST;
        AddUnit(lightFactory, $"L{Units.Count + 1}");
    }

    /// <summary>
    /// При создании тяжёлого вычитаем из общей суммы его поинты.
    /// </summary>
    public void AddHeavyUnit()
    {
        points -= HEAVYUNITCOST;
        AddUnit(heavyFactory, $"H{Units.Count + 1}");
    }

    /// <summary>
    /// При создании лучника вычитаем из суммы его поинты.
    /// </summary>
    public void AddArcherUnit()
    {
        points -= ARCHERUNITCOST;
        AddUnit(archerFactory, $"A{Units.Count + 1}");
    }

    /// <summary>
    /// При создании мага вычитаем из суммы его поинты.
    /// </summary>
    public void AddMageUnit()
    {
        points -= MAGEUNITCOST;
        AddUnit(magaFactory, $"M{Units.Count + 1}");
    }

    /// <summary>
    /// При создании врача вычитаем из суммы его поинты.
    /// </summary>
    public void AddHealerUnit()
    {
        points -= HEALERUNITCOST;
        AddUnit(healerFactory, $"C{Units.Count + 1}");
    }

    /// <summary>
    /// Чистим меню, когда армии созданы.
    /// </summary>
    public void FinishCreation()
    {
        FrontManager.Instance.ClearMenuBlocks();
        finishAction?.Invoke();
    }

    /// <summary>
    /// Для создания армии.
    /// </summary>
    /// <param name="nextArmyCreation"> - переходим к созданию следующей армии.</param>
    public void CreateArmy(int _points, Action nextArmyCreation)
    {
        points = _points;
        finishAction = nextArmyCreation;

        FrontManager.Instance.Printer($"Создание армии {Name}");

        ArmyCreation();
    }


    /// <summary>
    /// Текст кнопок при добавлении юнитов в армию.
    /// </summary>
    public void ArmyCreation()
    {
        // Минимальная стоимость юнита.
        FrontManager.Instance.ClearMenuBlocks();
        if (points >= 40)
        {
            FrontManager.Instance.AddMenuBlock($"У вас осталось {points} поинтов", null, true);
            FrontManager.Instance.AddMenuBlock($"Создание армии {Name}");
            if (CanAddLightUnit()) FrontManager.Instance.AddMenuBlock($"[+] Добавить легкого юнита ({LIGHTUNITCOST} поинтов)", AddLightUnit);
            if (CanAddHeavyUnit()) FrontManager.Instance.AddMenuBlock($"[+] Добавить тяжелого юнита ({HEAVYUNITCOST} поинтов)", AddHeavyUnit);
            if (CanAddArcherUnit()) FrontManager.Instance.AddMenuBlock($"[+] Добавить archer юнита ({ARCHERUNITCOST} поинтов)", AddArcherUnit);
            if (CanAddMageUnit()) FrontManager.Instance.AddMenuBlock($"[+] Добавить MAGA юнита ({MAGEUNITCOST} поинтов)", AddMageUnit);
            if (CanAddHealerUnit()) FrontManager.Instance.AddMenuBlock($"[+] Добавить Cleric юнита ({HEALERUNITCOST} поинтов)", AddHealerUnit);
            if (Units.Count() > 0) FrontManager.Instance.AddMenuBlock($">> Следующее действие", FinishCreation);
        }
        else
        {
            FinishCreation();
        }
    }

    /// <summary>
    /// Отображение армии (в консоли).
    /// </summary>
    public void DisplayArmy()
    {
        FrontManager.Instance.Printer($"Армия {Name}:");
        var armyRepresentation = "";
        foreach (var unit in Units)
        {
            /*FrontManager.Instance.Printer($"Unit: {unit}\n");*/
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
        FrontManager.Instance.Printer(armyRepresentation);
    }

    /// <summary>
    /// Сделать ход.
    /// </summary>
    /// <param name="enemyArmy"> - армия оппонента.</param>
    public void MakeMove(Army currentArmy, Army enemyArmy)

    {
        List<Unit> unitsToAdd = new List<Unit>();
        if (currentArmy.Units.Count == 0 || enemyArmy.Units.Count == 0)
        {
            FrontManager.Instance.Printer($"{(currentArmy.Units.Count == 0 ? currentArmy.Name : enemyArmy.Name)} проиграла, так как не осталось юнитов для боя.");
            return;
        }

        Unit attacker = Units[0];
        Unit defender = enemyArmy.Units[0];

        while (attacker.IsAlive() && defender.IsAlive())
        {
            // Ход текущей армии.
            foreach (var unit in currentArmy.Units)
            {
                FrontManager.Instance.Printer($"{unit.HealthPoints}");
                if (unit.IsAlive() && currentArmy.Units.IndexOf(unit) == 0)
                {
                    unit.Attack(enemyArmy.Units[0]);


                    // Проверяем, умер ли противник после атаки.
                    if (!enemyArmy.Units[0].IsAlive())
                    {
                        FrontManager.Instance.Printer($"Пехотинец {enemyArmy.Units[0].Name} умер.");
                        break;
                    }
                }
                else if (unit is ArcherUnit archer)
                {
                    archer.AttackWithRange(enemyArmy.Units);
                }
                else if (unit is MageUnit mage)
                {
                    Unit clonedUnit = mage.CloneAdjacentLightUnit(currentArmy.Units);
                    // Вставляем клонированного юнита рядом с магом.
                    if (clonedUnit != null)
                    {
                        currentArmy.Units.Insert(currentArmy.Units.IndexOf(mage), clonedUnit);
                    }
                }
                else if (unit is HealerUnit healer)
                {
                    healer.HealFirstUnitWithChance(currentArmy.Units);
                }
            }

            unitsToAdd.Clear(); // Очищаем список unitsToAdd после добавления юнитов
            // Удаление погибших юнитов после хода текущей армии.
            currentArmy.Units.RemoveAll(unit => !unit.IsAlive());
            enemyArmy.Units.RemoveAll(unit => !unit.IsAlive());

            // Меняем местами армии.
            var temp = currentArmy;
            currentArmy = enemyArmy;
            enemyArmy = temp;
        }

        if (!currentArmy.IsAlive())
        {
            FrontManager.Instance.Printer($"{enemyArmy.Name} победила!");
            GameManager.Instance.ShowNewGameMenu(enemyArmy.Name);
        }
        else if (!enemyArmy.IsAlive())
        {
            FrontManager.Instance.Printer($"{currentArmy.Name} победила!");
            GameManager.Instance.ShowNewGameMenu(currentArmy.Name);
        }

    }

    /// <summary>
    /// Проверка на живость.
    /// </summary>
    /// <returns></returns>
    public bool IsAlive()
    {
        return Units.Any(unit => unit.IsAlive());
    }

    private bool CanAddLightUnit() { return points >= LIGHTUNITCOST; }
    private bool CanAddHeavyUnit() => points >= HEAVYUNITCOST;
    private bool CanAddArcherUnit() => points >= ARCHERUNITCOST;
    private bool CanAddMageUnit() => points >= MAGEUNITCOST;
    private bool CanAddHealerUnit() => points >= HEALERUNITCOST;

    /// <summary>
    /// Сохраняем состояние армии.
    /// </summary>
    /// <param name="source"> - значение начальное армии.</param>
    /// <param name="destination"> - значение конечное.</param>
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
            else if (unit is HealerUnit)
            {
                destination.Units.Add(new HealerUnit(unit.Name));
            }
            unit.DestroyPhysicalUnit();
        }
    }
}
