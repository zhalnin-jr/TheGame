﻿using System.Collections.Generic;
using TheGame.UtilitesProxy;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    private GameFacade gameFacade;
    private FrontManager frontManager;
    public bool EnableSoundDeath { get; set; }
    public bool EnableAttackLogs { get; set; }
    public bool EnableSpecialLogs { get; set; }

    private static GameManager _instance;
    private static readonly object lockObj = new object();
    private ISoundProxy soundProxy;
    private IAttackLogProxy attackLogProxy;
    public IAttackLogProxy AttackLogProxy { get; private set; }
    private ISpecialAbilityLogProxy specialAbilityLogProxy;

    private bool isProxySelected = false;

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
                        Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
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

        if (!isProxySelected)
        {
            frontManager.ShowProxySelectionMenu(SelectSoundProxy, SelectAttackLogProxy, SelectSpecialAbilityLogProxy);
        }
        else
        {
            InitializeGame();
        }
    }

    private void SelectSoundProxy()
    {
        soundProxy = new SoundProxy();
        isProxySelected = true;
        InitializeGame();
    }

    private void SelectAttackLogProxy()
    {
        attackLogProxy = new AttackLogProxy();
        isProxySelected = true;
        InitializeGame();
    }

    private void SelectSpecialAbilityLogProxy()
    {
        specialAbilityLogProxy = new SpecialAbilityLogProxy();
        isProxySelected = true;
        InitializeGame();
    }

    private void InitializeGame()
    {
        ShowFacadeMenu();
    }

   
    public void ShowFacadeMenu()
    {
        frontManager.ClearMenuBlocks();

        if (gameFacade.GetGameState() == GameFacade.GameState.ArmiesDoesntExist)
        {
            frontManager.AddMenuBlock("Создать армию", gameFacade.CreateArmies, true);
        }
        else if (gameFacade.GetGameState() == GameFacade.GameState.CanMakeMove)
        {
            frontManager.AddMenuBlock("Сделать ход", gameFacade.MakeMove, true);
            frontManager.AddMenuBlock("Доиграть до конца", gameFacade.PlayUntilEnd, true);
            frontManager.AddMenuBlock("В главное меню", gameFacade.StartNewGame, true);
        }
        if (gameFacade.GetGameState() == GameFacade.GameState.ArmiesDoesntExist)
        {
            frontManager.AddMenuBlock("Выйти", Application.Quit, true);
        }
    }

    public void ShowNewGameMenu(string armyName)
    {
        frontManager.ClearMenuBlocks();
        frontManager.AddMenuBlock($"{armyName} победила", null, true);
        frontManager.AddMenuBlock("Новая игра", gameFacade.StartNewGame);
    }
}
