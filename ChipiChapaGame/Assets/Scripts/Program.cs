using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
public class Program: MonoBehaviour
{
    GameFacade gameFacade;

    private void Start()
    {
        gameFacade = GameFacade.GetInstance();
        ShowFacadeMenu();

    }

    public void ShowFacadeMenu()
    {
        FrontManager.Instance.ClearMenuBlocks();
        FrontManager.Instance.AddMenuBlock($"Создать армию", gameFacade.CreateArmies);
        FrontManager.Instance.AddMenuBlock($"Сделать ход", gameFacade.MakeMove);
        FrontManager.Instance.AddMenuBlock($"Доиграть игру до конца", gameFacade.PlayUntilEnd);
        FrontManager.Instance.AddMenuBlock($"Выйти", Application.Quit);
    }
}