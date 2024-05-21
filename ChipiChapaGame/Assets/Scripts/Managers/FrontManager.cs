using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FrontManager: MonoBehaviour
{
    private static FrontManager _instance;
    private static readonly object lockObj = new object();

    // Новое
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform menuParent;
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

    public bool ReadIntegerInput(out int inputInt)
    {
        while (true)
        {
            string inputText = inputField.text;
            int inputValue;

            if (int.TryParse(inputText, out inputValue))
            {
                inputInt = inputValue;
                return true;
            }
            else
            {
                inputInt = -1;
                return false;
            }
        }
    }

    public void AddMenuBlock(string _buttonText, Action _onClick = null)
    {
        GameObject newBlock;
        if (_onClick != null)
        {
            newBlock = Instantiate(buttonMenu, menuParent);
            newBlock.GetComponent<Button>().onClick.AddListener(() => _onClick.Invoke());
        }
        else 
        {
            newBlock = Instantiate(menuHeader, menuParent);
        }
        newBlock.GetComponentInChildren<TMP_Text>().text = _buttonText;
    }
    public void ClearMenuBlocks()
    {
        foreach (Transform child in menuParent.transform)
        {
            // Уничтожаем каждый дочерний объект
            Destroy(child.gameObject);
        }
    }

}
