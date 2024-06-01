using static Unit;
public class LightUnit : Unit, ICloneableUnit, IHealableUnit
{
    public LightUnit(string name) : base(name, 100, 15, 5, 4) { } 

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
        HealthPoints = System.Math.Min(HealthPoints + amount, HealthPoints);
    }
}
