using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Unit;

/// <summary>
/// Класс для Лучника.
/// </summary>
public class ArcherUnit : Unit, IHealableUnit
{
    [SerializeField] private GameObject graphicUnit;

    private static readonly System.Random random = new();
    public int Range { get; private set; }
    public int RangeDamage { get; private set; }

    // Сохраняет последнюю цель атаки.
    public Unit LastTarget { get; private set; }

    // Сохраняет нанесенный урон.
    public int LastDamage { get; private set; }

    /// <summary>
    /// Создание через конструктор с параметрами.
    /// </summary>
    public ArcherUnit(string name) : base(name, 100, 15, 5, 1)
    {
        Range = 3;
        RangeDamage = 20;
    }

    /// <summary>
    /// Реализация атаки с рейнджом.
    /// </summary>
    public void AttackWithRange(List<Unit> enemyUnits)
    {
        // Вычисляем диапазон позиций, в которые может попасть лучник.
        int minRangePosition = Math.Max(0, enemyUnits.IndexOf(this) - Range);
        int maxRangePosition = Math.Min(enemyUnits.Count - 1, enemyUnits.IndexOf(this) + Range);

        // Создаем список позиций в диапазоне, в которые может попасть лучник.
        List<int> possiblePositions = Enumerable.Range(minRangePosition, maxRangePosition - minRangePosition + 1).ToList();

        // Выбираем случайную позицию из списка возможных позиций.
        int targetPositionIndex = random.Next(possiblePositions.Count);
        int targetPosition = possiblePositions[targetPositionIndex];

        // Атакуем воина противника в выбранной позиции, если такой есть.
        if (enemyUnits[targetPosition] != null)
        {
            Unit target = enemyUnits[targetPosition];
            int damage = Math.Max(0, RangeDamage - target.DefensePoints);
            target.HealthPoints -= damage;
            if (!target.IsAlive())
            {
                target.DestroyPhysicalUnit();
            }
            FrontManager.Instance.Printer($"{Name} атакует {target.Name} с расстояния и наносит {damage} урона.");
        }
    }

    /// <summary>
    /// Может быть вылеченным.
    /// </summary>
    /// <param name="amount"> - количество поинтов.</param>
    public void Heal(int amount)
    {
        HealthPoints = Math.Min(HealthPoints + amount, base.HealthPoints);
    }
}
