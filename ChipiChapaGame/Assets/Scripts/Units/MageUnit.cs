using System.Collections.Generic;

public class MageUnit : Unit
    {
    public MageUnit(string name) : base(name, 60, 30, 20, 5, PhysicalUnitManager.Instance.GetPhysicalUnit(Unit.UnitType.Light)) { }

    public void CloneAdjacentLightUnit(MageUnit mage, List<Unit> units)
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

            if (unitToClone != null && unitToClone is ICloneableUnit cloneable)
            {
                var clonedUnit = cloneable.Clone();
                units.Insert(mageIndex, clonedUnit);
                FrontManager.Instance.Printer($"Маг {mage.Name} клонировал {clonedUnit.Name} перед собой.");
            }
        }
    }
