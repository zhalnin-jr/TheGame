using System;
using System.Collections.Generic;

/// <summary>
/// Описание юнита Маг.
/// </summary>
public class MageUnit : Unit
{
    /// <summary>
    /// Создание юнита согласно конструктору с параметрами.
    /// </summary>
    public MageUnit(string name) : base(name, 50, 30, 10, 3) { }
    private static readonly Random random = new Random();

    /// <summary>
    /// Метод для клонирования лёгкого юнита магом.
    /// </summary>
    /// <param name="mage"> - передаём мага.</param>
    /// <param name="units"> - работаем со списком юнитов.</param>
    public void CloneAdjacentLightUnit(MageUnit mage, List<Unit> units)
    {
        int chance = random.Next(1, 101);

        // Проверяем, выпала ли у нас удача с шансом 25%.
        if (chance <= 20)
        {
            int mageIndex = units.IndexOf(mage);
            Unit unitToClone = null;

            if (mageIndex > 0 && units[mageIndex - 1] is LightUnit leftUnit)
            {
                unitToClone = leftUnit;
            }
            else if (mageIndex < units.Count - 1 && units[mageIndex + 1] is LightUnit rightUnit)
            {
                unitToClone = rightUnit;
            }
            // Клонируем.
            if (unitToClone != null && unitToClone is ICloneableUnit cloneable)
            {
                var clonedUnit = cloneable.Clone();
                units.Insert(mageIndex, clonedUnit);
                FrontManager.Instance.Printer($"Маг {mage.Name} клонировал {clonedUnit.Name} перед собой.");
            }
        }
    }
}
