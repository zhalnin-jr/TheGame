using static Unit;

/// <summary>
/// Класс для описания тяжёлого пехотинца.
/// </summary>
public class HeavyUnit : Unit, IHealableUnit
{
    /// <summary>
    /// Создание через конструктор с параметрами.
    /// </summary>
    public HeavyUnit(string name) : base(name, 100, 30, 10, 2) { }

    /// <summary>
    /// Может быть вылеченным.
    /// </summary>
    /// <param name="amount"> - значение хилла.</param>
    public void Heal(int amount)
    {
        HealthPoints = System.Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
