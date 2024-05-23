using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhysicalUnitManager : MonoBehaviour
{
    [SerializeField] private PhysicalUnit lightUnitPrefab;
    private static PhysicalUnitManager _instance;

    private static readonly object lockObj = new object();


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
    public PhysicalUnit GetPhysicalUnit(Unit.UnitType type)
    {
        if (type == Unit.UnitType.Light)
        {
            return Instantiate(lightUnitPrefab);
        }
        return Instantiate(lightUnitPrefab);
    }
}
