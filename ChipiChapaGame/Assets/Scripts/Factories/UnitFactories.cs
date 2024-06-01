/// <summary>
/// Фабричные методы для создания юнитов.
/// </summary>
public class UnitFactories
{
    /// <summary>
    /// Один интерфейс на класс.
    /// </summary>
    public interface IUnitFactory
    {
        Unit CreateUnit(string name);
    }

    /// <summary>
    /// Для создания Лёгкого Пехотинца.
    /// </summary>
    public class LightUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new LightUnit(name);
        }
    }

    /// <summary>
    /// Для создания Тяжёлого Пехотинца.
    /// </summary>
    public class HeavyUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new HeavyUnit(name);
        }
    }

    /// <summary>
    /// Для создания Лучника.
    /// </summary>
    public class ArcherUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new ArcherUnit(name);
        }
    }

    /// <summary>
    /// Для создания Мага.
    /// </summary>
    public class MageUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new MageUnit(name);
        }
    }

    /// <summary>
    /// Для создания Врача.
    /// </summary>
    public class HealerUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new HealerUnit(name);
        }
    }
}
