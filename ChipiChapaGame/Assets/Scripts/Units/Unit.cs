using System;
using UnityEditor;
using UnityEngine;

public class Unit
{
    private PhysicalUnit PhysicalUnit;

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
        PhysicalUnit = PhysicalUnitManager.Instance.GetPhysicalUnit(this);
    }

    public void Attack(Unit target)
    {
        PhysicalUnit.PlayAttack();
        int dodge = (this is LightUnit) ? random.Next(5, 11) : random.Next(0, 6);
        int damage = Math.Max(0, AttackPoints - target.DefensePoints - dodge);
        target.HealthPoints -= damage;
        FrontManager.Instance.Printer($"{Name} атакует {target.Name} и наносит {damage} урона.");
    }

    public bool IsAlive()
    {
        return HealthPoints > 0;
    }

    public void DestroyPhysicalUnit()
    {
        PhysicalUnit.Destroy();
    }
}
