using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.UtilitesProxy;

public class GameFacade
{
    private static GameFacade instance;
    private static readonly object lockObject = new object();
    private readonly BattleGame game;

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
            lock (lockObject)
            {
                instance ??= new GameFacade();
            }
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
        game.Army1.MakeMove(game.Army1, game.Army2);
        FrontManager.Instance.Printer("");
        game.Army1.DisplayArmy();   
        FrontManager.Instance.Printer("");
        game.Army2.DisplayArmy();
    }

    public void StartNewGame()
    {
        game.ClearArmies();
        PhysicalUnitManager.Instance.ClearAllPhysicalUnits();
        GameManager.Instance.ShowFacadeMenu();
    }

    public GameState GetGameState()
    {
        if (game.Army2 == null)
        {
            return GameState.ArmiesDoesntExist;
        }
        else if (game.Army1.IsAlive() && game.Army2.IsAlive())
        {
            return GameState.CanMakeMove;
        }
        else
        {
            return GameState.GameFinished;
        }
    }
    public enum GameState
    { 
        ArmiesDoesntExist,
        CanMakeMove,
        GameFinished
    }
}
