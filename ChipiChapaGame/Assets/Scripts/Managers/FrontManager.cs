using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Отвечает за UI в игре. MonoBehavior - базовый класс для корректной работы с Unity.
/// </summary>
public class FrontManager: MonoBehaviour
{
    private static FrontManager _instance;
    private static readonly object lockObj = new object();

    // Новое.
    [SerializeField] private Transform menuCenterParent;
    [SerializeField] private Transform menuBottomParent;
    [SerializeField] private GameObject menuHeader;
    [SerializeField] private GameObject buttonMenu;

    /// <summary>
    /// Единственная инициализация скриптов на сцене.
    /// </summary>
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

    /// <summary>
    /// Удаление объектов.
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
    /// Объявление.
    /// </summary>
    private protected FrontManager()
    {
    }

    /// <summary>
    /// Функция для вывода логов.
    /// </summary>
    /// <param name="output"> - информация, которая выводится. </param>
    public void Printer(string output)
    {
        Debug.Log(output);
        //Console.WriteLine(output);
    }

    /// <summary>
    /// Конструктор для создания кнопок.
    /// </summary>
    /// <param name="_buttonText"> - определение текста внутри кнопки. </param>
    /// <param name="_onClick"> - возможность кликать. </param>
    /// <param name="bottom"> - положение кнопок. </param>
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

    /// <summary>
    /// Очищение ячейки списка.
    /// </summary>
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

    /// <summary>
    /// Показываем меню по прокси.
    /// </summary>
    /// <param name="selectSoundProxy"> - прокси по добавлению звука смерти. </param>
    /// <param name="selectAttackLogProxy"> - прокси по логированию атак. </param>
    /// <param name="selectSpecialAbilityLogProxy"> - прокси по логированию спешл абилити.</param>
    public void ShowProxySelectionMenu(Action selectSoundProxy, Action selectAttackLogProxy, Action selectSpecialAbilityLogProxy)
    {
        ClearMenuBlocks();
        AddMenuBlock("Выбрать Sound Proxy", selectSoundProxy, true);
        AddMenuBlock("Выбрать Attack Log Proxy", selectAttackLogProxy, true);
        AddMenuBlock("Выбрать Special Ability Log Proxy", selectSpecialAbilityLogProxy, true);
    }
}
