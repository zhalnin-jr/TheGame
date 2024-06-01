using static Unit;

/// <summary>
/// Описание лёгкого пехотинца.
/// </summary>
public class LightUnit : Unit, ICloneableUnit, IHealableUnit
{
    /// <summary>
    /// Создание согласно конструктору с параметрами.
    /// </summary>
    public LightUnit(string name) : base(name, 100, 15, 5, 4) { }

    /// <summary>
    /// Он может быть клонирован магом.
    /// </summary>
    /// <returns>Возвращает клонированного легкого пехотинца нового.</returns>
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

    /// <summary>
    /// Может быть вылеченным.
    /// </summary>
    /// <param name="amount">Количество поинтов для излечения.</param>
    public void Heal(int amount)
    {
        HealthPoints = System.Math.Min(HealthPoints + amount, HealthPoints);
    }
}
