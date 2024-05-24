using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FrontManager: MonoBehaviour
{
    private static FrontManager _instance;
    private static readonly object lockObj = new object();

    // Новое
    [SerializeField] private Transform menuCenterParent;
    [SerializeField] private Transform menuBottomParent;
    [SerializeField] private GameObject menuHeader;
    [SerializeField] private GameObject buttonMenu;

    public static FrontManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    _instance = FindObjectOfType<FrontManager>();
                    if (_instance == null)
                    {
                        Debug.LogError("There needs to be one active FrontManager script on a GameObject in your scene.");
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

    private protected FrontManager()
    {
    }

    public void Printer(string output)
    {
        Debug.Log(output);
        //Console.WriteLine(output);
    }

    public void AddMenuBlock(string _buttonText, Action _onClick = null, bool bottom = false)
    {
        GameObject newBlock;

        Transform newParent = menuCenterParent;
        if (bottom) newParent = menuBottomParent;

        if (_onClick != null)
        {
            newBlock = Instantiate(buttonMenu, newParent);
            newBlock.GetComponent<Button>().onClick.AddListener(() => _onClick.Invoke());
        }
        else 
        {
            newBlock = Instantiate(menuHeader, newParent);
        }
        newBlock.GetComponentInChildren<TMP_Text>().text = _buttonText;
    }

    public void ClearMenuBlocks()
    {
        foreach (Transform child in menuCenterParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in menuBottomParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
