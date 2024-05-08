using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unit;

public class ArcherUnit : Unit, IHealableUnit
{

    private static readonly Random random = new Random();
    public int Range { get; private set; }
    public int RangeDamage { get; private set; }
    public Unit LastTarget { get; private set; } // Сохраняет последнюю цель атаки
    public int LastDamage { get; private set; } // Сохраняет нанесенный урон

    public ArcherUnit(string name) : base(name, 90, 70, 25, 5)
    {
        Range = 3;
        RangeDamage = 30;
    }

    // Реализация атаки с ренджом
    public void AttackWithRange(List<Unit> enemyUnits)
    {
        // Вычисляем диапазон позиций, в которые может попасть лучник
        int minRangePosition = Math.Max(0, enemyUnits.IndexOf(this) - Range);
        int maxRangePosition = Math.Min(enemyUnits.Count - 1, enemyUnits.IndexOf(this) + Range);

        // Создаем список позиций в диапазоне, в которые может попасть лучник
        List<int> possiblePositions = Enumerable.Range(minRangePosition, maxRangePosition - minRangePosition + 1).ToList();

        // Выбираем случайную позицию из списка возможных позиций
        int targetPositionIndex = random.Next(possiblePositions.Count);
        int targetPosition = possiblePositions[targetPositionIndex];

        // Атакуем воина противника в выбранной позиции, если такой есть
        if (enemyUnits[targetPosition] != null)
        {
            Unit target = enemyUnits[targetPosition];
            int damage = Math.Max(0, RangeDamage - target.DefensePoints);
            target.HealthPoints -= damage;
            FrontManager.GetInstance().Printer($"{Name} атакует {target.Name} с расстояния и наносит {damage} урона.");
        }
    }

    public void Heal(int amount)
    {
        HealthPoints = Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
