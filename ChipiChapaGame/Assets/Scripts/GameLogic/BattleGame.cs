// Singleton - это паттерн проектирования, который гарантирует, что у класса есть только один экземпляр.
// и предоставляет глобальную точку доступа к этому экземпляру.
using System.Diagnostics;

public class BattleGame
{
    // Instance - хранит единственный экземпляр класса BattleGame.
    private Army initialArmy1;
    private Army initialArmy2;
    private static BattleGame instance;
    private static readonly object lockObj = new object();

    public Army Army1 { get; set; }
    public Army Army2 { get; set; }

    /// <summary>
    /// Приватный конструктор, который вызывается только внутри класса, что предотвращает создание экземпляров извне.
    /// </summary>
    private protected BattleGame()
    {
        initialArmy1 = new Army("Левая армия");
        initialArmy2 = new Army("Правая армия");
    }

    // Instance - предоставляет глобальную точку доступа к единственному экземпляру класса BattleGame. Если экземпляр не создан
    // он создается при первом вызове.
    public static BattleGame Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance ??= new BattleGame();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Начало создания армии.
    /// </summary>
    public void StartArmiesCreation()
    {
        PhysicalUnitManager.Instance.currentArmyID = 0;
        FrontManager.Instance.Printer("Создание левой армии:");
        initialArmy1.CreateArmy(450, CreateSecondArmy);
    }

    /// <summary>
    /// Создаём вторую армию.
    /// </summary>
    public void CreateSecondArmy()
    {
        PhysicalUnitManager.Instance.currentArmyID = 1;
        FrontManager.Instance.Printer("Создание правой армии:");
        initialArmy2.CreateArmy(450, FinishArmiesCreation);
    }

    /// <summary>
    /// Армии созданы.
    /// </summary>
    public void FinishArmiesCreation()
    {
        FrontManager.Instance.Printer("FINISH");
        // Создаем копии армий для текущего хода.
        Army1 = new Army("Левая армия");
        Army2 = new Army("Правая армия");
        // Копируем состояние из начальных армий.
        PhysicalUnitManager.Instance.currentArmyID = 0;
        Army.CopyArmyState(initialArmy1, Army1);
        PhysicalUnitManager.Instance.currentArmyID = 1;
        Army.CopyArmyState(initialArmy2, Army2);
        GameManager.Instance.ShowFacadeMenu();
    }

    /// <summary>
    /// Играть до конца.
    /// </summary>
    public void PlayUntilEnd()
    {
        while (Army1.IsAlive() && Army2.IsAlive())
        {

            Army1.MakeMove(Army1,Army2);
            // Проверяем, остались ли живые юниты в армии 2 после хода армии 1.
            if (!Army2.IsAlive())
            {
                FrontManager.Instance.Printer($"{Army1.Name} победила!");
                // Завершаем игру, если армия 2 уничтожена.
                return;
            }
            if (!Army1.IsAlive())
            {
                FrontManager.Instance.Printer($"{Army2.Name} победила!");
                // Завершаем игру, если армия 1 уничтожена.
                return;
            }
        }

        // Если цикл выше прервался без объявления победителя, проверяем, кто выиграл.
        if (!Army1.IsAlive() && Army2.IsAlive())
        {
            FrontManager.Instance.Printer($"{Army2.Name} победила!");
            GameManager.Instance.ShowNewGameMenu(Army2.Name);
        }
        else if (Army1.IsAlive() && !Army2.IsAlive())
        {
            FrontManager.Instance.Printer($"{Army1.Name} победила!");
            GameManager.Instance.ShowNewGameMenu(Army1.Name);
        }
        // В случае, если обе армии были уничтожены одновременно.
        else
        {
            FrontManager.Instance.Printer("Битва окончена ничьей.");
        }
    }

    /// <summary>
    /// Очистить армии.
    /// </summary>
    public void ClearArmies()
    {
        Army1 = null;
        Army2 = null;
        initialArmy1.Units.Clear();
        initialArmy2.Units.Clear();
    }
}
