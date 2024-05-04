public class UnitFactories
{
    public interface IUnitFactory
    {
        Unit CreateUnit(string name);
    }

    public class LightUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new LightUnit(name);
        }
    }

    public class HeavyUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new HeavyUnit(name);
        }
    }
    public class ArcherUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new ArcherUnit(name);
        }
    }

    public class MageUnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string name)
        {
            return new MageUnit(name);
        }
    }
}
