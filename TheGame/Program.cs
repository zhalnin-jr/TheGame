using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
public class Program
{
    static void Main()
    {

        GameFacade gameFacade = GameFacade.GetInstance();

        int choice;
        do
        {
            FrontManager.GetInstance().Printer("1. Создать армию");
            FrontManager.GetInstance().Printer("2. Сделать ход");
            FrontManager.GetInstance().Printer("3. Доиграть игру до конца");
            FrontManager.GetInstance().Printer("0. Выйти");

            try
            {
                choice = BattleGame.ReadIntegerInput();

                switch (choice)
                {
                    case 1:
                        gameFacade.CreateArmies();
                        break;
                    case 2:
                        gameFacade.MakeMove();
                        break;
                    case 3:
                        gameFacade.PlayUntilEnd();
                        break;
                }
            }
            catch (FormatException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введите число.");
                choice = -1;
            }
            catch (OverflowException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введено слишком большое число.");
                choice = -1;
            }

        } while (choice != 0);
    }
}