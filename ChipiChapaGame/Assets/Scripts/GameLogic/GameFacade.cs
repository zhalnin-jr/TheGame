using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.UtilitesProxy;

/// <summary>
/// Отвечает за реализацию паттерна Фасад (объединение методов).
/// </summary>
public class GameFacade
{
    private static GameFacade instance;
    private static readonly object lockObject = new object();
    private readonly BattleGame game;

    /// <summary>
    /// Приватный конструктор для синглтона.
    /// </summary>
    private GameFacade()
    {
        game = BattleGame.Instance;
    }

    /// <summary>
    /// Метод для получения экземпляра GameFacade (реализация Singleton).
    /// </summary>
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

    /// <summary>
    /// Создание армии (здесь и ниже - методы для управления игрой).
    /// </summary>
    public void CreateArmies()
    {
        game.StartArmiesCreation();
    }

    /// <summary>
    /// Доиграть до конца.
    /// </summary>
    public void PlayUntilEnd()
    {
        game.PlayUntilEnd();
    }

    /// <summary>
    /// Сделать ход.
    /// </summary>
    public void MakeMove()
    {
        game.Army1.MakeMove(game.Army1, game.Army2);
        FrontManager.Instance.Printer("");
        game.Army1.DisplayArmy();   
        FrontManager.Instance.Printer("");
        game.Army2.DisplayArmy();
    }

    /// <summary>
    /// Действия при начале новой игры.
    /// </summary>
    public void StartNewGame()
    {
        game.ClearArmies();
        PhysicalUnitManager.Instance.ClearAllPhysicalUnits();
        GameManager.Instance.ShowFacadeMenu();
    }

    /// <summary>
    /// Выбор состояния игры.
    /// </summary>
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

    /// <summary>
    /// Состояние игры - армии не существует, сделать ход, игра окончена.
    /// </summary>
    public enum GameState
    { 
        ArmiesDoesntExist,
        CanMakeMove,
        GameFinished
    }
}
