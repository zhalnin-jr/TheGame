using System;
using System.Collections.Generic;
public class MageUnit : Unit
{
    private static readonly Random random = new Random();

    public MageUnit(string name) : base(name, 50, 30, 10, 3) { }

    public Unit CloneAdjacentLightUnit(List<Unit> units)
    {
        int chance = random.Next(1, 101);

        // Проверяем, выпала ли у нас удача с шансом 20%.
        if (chance <= 20)
        {
            int mageIndex = units.IndexOf(this);
            Unit unitToClone = null;

            if (mageIndex > 0 && units[mageIndex - 1] is LightUnit leftUnit)
            {
                unitToClone = leftUnit;
            }
            else if (mageIndex < units.Count - 1 && units[mageIndex + 1] is LightUnit rightUnit)
            {
                unitToClone = rightUnit;
            }

            if (unitToClone != null && unitToClone is ICloneableUnit cloneable)
            {
                var clonedUnit = cloneable.Clone();
                FrontManager.Instance.Printer($"Маг {this.Name} клонировал {clonedUnit.Name} перед собой.");
                return clonedUnit;
            }
        }

        return null; // Если клонирование не произошло, возвращаем null.
    }
}