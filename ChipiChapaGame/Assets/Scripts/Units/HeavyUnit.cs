using static Unit;

public class HeavyUnit : Unit, IHealableUnit
{
    public HeavyUnit(string name) : base(name, 200, 10, 10, 2) { }

    public void Heal(int amount)
    {
        HealthPoints = System.Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
