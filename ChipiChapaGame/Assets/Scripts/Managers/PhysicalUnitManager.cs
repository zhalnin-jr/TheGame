using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhysicalUnitManager : MonoBehaviour
{
    [SerializeField] private PhysicalUnit lightUnitPrefab;
    [SerializeField] private Transform armyParent1;
    [SerializeField] private Transform armyParent2;
    private static PhysicalUnitManager _instance;

    private static readonly object lockObj = new object();

    public int currentArmyID = 0;

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
    public PhysicalUnit GetPhysicalUnit(Unit target)
    {
        PhysicalUnit newPrefab;
        if (target is LightUnit)
        {
            newPrefab = lightUnitPrefab;
        }
        else
        {
            newPrefab = lightUnitPrefab;
        }

        if (currentArmyID == 0) return Instantiate(newPrefab, armyParent1);
        else return Instantiate(newPrefab, armyParent2);
    }
}
