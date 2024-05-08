using static Unit;

public class HeavyUnit : Unit, IHealableUnit
{
    public HeavyUnit(string name) : base(name, 200, 40, 60, 0) { }

    public void Heal(int amount)
    {
        HealthPoints = Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
