using System;
using UnityEngine;

public class Unit
{
    public string Name { get; set; }
    public int HealthPoints { get; set; }
    public int AttackPoints { get; set; }
    public int DefensePoints { get; set; }
    public int DodgeChance { get; set; }

    private static readonly System.Random random = new();
    public interface ICloneableUnit
    {
        Unit Clone();
    }
    public interface IHealableUnit
    {
        void Heal(int amount);
    }

    public Unit(string name, int healthPoints, int attackPoints, int defensePoints, int dodgeChance)
    {
        Name = name;
        HealthPoints = healthPoints;
        AttackPoints = attackPoints;
        DefensePoints = defensePoints;
        DodgeChance = dodgeChance;
    }

    public void Attack(Unit target)
    {
        int dodge = (this is LightUnit) ? random.Next(5, 11) : random.Next(0, 6);
        int damage = Math.Max(0, AttackPoints - target.DefensePoints - dodge);
        target.HealthPoints -= damage;
        FrontManager.Instance.Printer($"{Name} атакует {target.Name} и наносит {damage} урона.");
    }

    public bool IsAlive()
    {
        return HealthPoints > 0;
    }
}
