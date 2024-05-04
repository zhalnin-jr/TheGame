using static Unit;
public class LightUnit : Unit, ICloneableUnit
{
    public LightUnit(string name) : base(name, 150, 80, 20, 10) { }

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
}
