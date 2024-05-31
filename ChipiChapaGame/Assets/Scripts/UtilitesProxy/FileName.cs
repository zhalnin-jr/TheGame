using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO; 

namespace TheGame.UtilitesProxy
{
    public interface ISoundProxy
    {
        void PlaySound(string sound);
    }

    public interface IAttackLogProxy
    {
        void LogAttack(Unit attacker, Unit defender);
    }

    public interface ISpecialAbilityLogProxy
    {
        void LogSpecialAbility(string abilityName, Unit unit);
    }
    public class SoundProxy : ISoundProxy
    {
        public void PlaySound(string sound)
        {
            Debug.Log($"Playing sound: {sound}");
            // Здесь можно добавить код для воспроизведения звука через Unity
        }
    }

    public class AttackLogProxy : IAttackLogProxy
    {
        public void LogAttack(Unit attacker, Unit defender)
        {
            string logEntry = $"Attack log: {attacker.Name} attacks {defender.Name}\n";
            FileManager.WriteToFile("attackLog.txt", logEntry);
            Debug.Log(logEntry);
        }
    }

    public class SpecialAbilityLogProxy : ISpecialAbilityLogProxy
    {
        private string logFilePath = "specialAbilityLog.txt";

        public void LogSpecialAbility(string abilityName, Unit unit)
        {
            string logMessage = $"Special ability log: {unit.Name} uses {abilityName}";
            Debug.Log(logMessage);
            WriteLog(logMessage);
        }

        private void WriteLog(string message)
        {
            File.AppendAllText(logFilePath, message + "\n");
        }
    }

public class FileManager
    {
        public static void WriteToFile(string filePath, string message)
        {
            File.AppendAllText(filePath, message + "\n");
        }

        public static string ReadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return string.Empty;
        }
    }
  
}
