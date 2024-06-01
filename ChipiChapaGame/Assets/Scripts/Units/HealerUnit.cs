using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Класс Врача.
/// </summary>
internal class HealerUnit : Unit
{
    private static readonly Random random = new Random();

    /// <summary>
    /// Создание через конструктор с параметрами.
    /// </summary>
    public HealerUnit(string name) : base(name, 30, 20, 10, 3) { }

    /// <summary>
    /// Логика работы хилла.
    /// </summary>
    /// <param name="units"> - передаем список юнитов.</param>
    public void HealFirstUnitWithChance(List<Unit> units)
    {
        // Генерируем случайное число от 1 до 100.
        int chance = random.Next(1, 101);

        // Проверяем, выпала ли у нас удача с шансом 25%.
        if (chance <= 30)
        {
                // Получаем первого юнита из списка.
                Unit firstUnit = units[0];

                // Проверяем, реализует ли юнит интерфейс IHealableUnit.
                if (firstUnit is IHealableUnit healableUnit)
                {
                    // Увеличиваем количество ХР у первого юнита.
                    healableUnit.Heal(10);

                    FrontManager.Instance.Printer($"Лечитель {Name} исцелил юнита {firstUnit.Name}.");
                }
                else
                {
                    FrontManager.Instance.Printer($"Юнит {firstUnit.Name} не может быть исцелен.");
                }
        }
    }
}