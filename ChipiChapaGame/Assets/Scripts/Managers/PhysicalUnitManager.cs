using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Отвечает за скрикпты ударов, анимации.
/// </summary>
public class PhysicalUnitManager : MonoBehaviour
{
    [SerializeField] private PhysicalUnit lightUnitPrefab;
    [SerializeField] private PhysicalUnit healerUnitPrefab;
    [SerializeField] private PhysicalUnit heavyUnitPrefab;
    [SerializeField] private PhysicalUnit mageUnitPrefab;
    [SerializeField] private PhysicalUnit archerUnitPrefab;

    [SerializeField] private Transform armyParent1;
    [SerializeField] private Transform armyParent2;

    private static PhysicalUnitManager _instance;

    private static readonly object lockObj = new object();

    public int currentArmyID = 0;

    /// <summary>
    /// Только единичная инициализация.
    /// </summary>
    public static PhysicalUnitManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    _instance = FindObjectOfType<PhysicalUnitManager>();
                    if (_instance == null)
                    {
                        Debug.LogError("There needs to be one active PhysicalUnitManager script on a GameObject in your scene.");
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Удаление объектов (для Unity).
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Выбор модельки юнита.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public PhysicalUnit GetPhysicalUnit(Unit target)
    {
        PhysicalUnit newPrefab;
        if (target is LightUnit)
        {
            newPrefab = lightUnitPrefab;
        }
        else if (target is HeavyUnit)
        {
            newPrefab = heavyUnitPrefab;
        }
        else if (target is ArcherUnit)
        {
            newPrefab = archerUnitPrefab;
        }
        else if (target is HealerUnit)
        {
            newPrefab = healerUnitPrefab;
        }
        else 
        {
            newPrefab = mageUnitPrefab;
        }
        if (currentArmyID == 0) return Instantiate(newPrefab, armyParent1);
        else return Instantiate(newPrefab, armyParent2);
    }

    /// <summary>
    /// Чистим поле.
    /// </summary>
    public void ClearAllPhysicalUnits()
    {
        foreach (Transform child in armyParent1.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in armyParent2.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
