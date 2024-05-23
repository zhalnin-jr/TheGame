using static Unit;

public class HeavyUnit : Unit, IHealableUnit
{
    public HeavyUnit(string name) : base(name, 200, 40, 60, 0, PhysicalUnitManager.Instance.GetPhysicalUnit(Unit.UnitType.Light)) { }

    public void Heal(int amount)
    {
        HealthPoints = System.Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
