using static Unit;
public class LightUnit : Unit, ICloneableUnit, IHealableUnit
{
    public LightUnit(string name) : base(name, 150, 80, 20, 10, PhysicalUnitManager.Instance.GetPhysicalUnit(Unit.UnitType.Light)) { }

    public Unit Clone()
    {
        return new LightUnit(Name + "_clone")
        {
            HealthPoints = this.HealthPoints,
            AttackPoints = this.AttackPoints,
            DefensePoints = this.DefensePoints,
            DodgeChance = this.DodgeChance
        };
    }

    public void Heal(int amount)
    {
        HealthPoints = System.Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
