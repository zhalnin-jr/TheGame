using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameFacade
{
    private static GameFacade instance;
    private BattleGame game;

    // Приватный конструктор для Singleton.
    private GameFacade()
    {
        game = BattleGame.Instance;
    }

    // Метод для получения экземпляра GameFacade (реализация Singleton).
    public static GameFacade GetInstance()
    {
        if (instance == null)
        {
            instance = new GameFacade();
        }
        return instance;
    }

    // Методы для управления игрой.
    public void CreateArmies()
    {
        game.StartArmiesCreation();
    }

    public void PlayUntilEnd()
    {
        game.PlayUntilEnd();
    }

    public void MakeMove()
    {
        game.Army1.MakeMove(game.Army2);
        FrontManager.Instance.Printer("");
        game.Army1.DisplayArmy();
        FrontManager.Instance.Printer("");
        game.Army2.DisplayArmy();
    }
}
