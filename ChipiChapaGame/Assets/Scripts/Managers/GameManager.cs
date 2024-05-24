using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class GameManager: MonoBehaviour
{
    private GameFacade gameFacade;
    private FrontManager frontManager;

    private static GameManager _instance;
    private static readonly object lockObj = new object();

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    _instance = FindObjectOfType<GameManager>();
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

    private void Start()
    {
        gameFacade = GameFacade.GetInstance();
        frontManager = FrontManager.Instance;
        ShowFacadeMenu();

    }

    public void ShowFacadeMenu()
    {
        frontManager.ClearMenuBlocks();

        if (gameFacade.GetGameState() == GameFacade.GameState.ArmiesDoesntExist)
        {
            frontManager.AddMenuBlock($"Создать армию", gameFacade.CreateArmies, true);
        }
        else if (gameFacade.GetGameState() == GameFacade.GameState.CanMakeMove)
        {
            frontManager.AddMenuBlock($"Сделать ход", gameFacade.MakeMove, true);
            frontManager.AddMenuBlock($"Доиграть до конца", gameFacade.PlayUntilEnd, true);
        }
        frontManager.AddMenuBlock($"Выйти", Application.Quit, true);
    }
}