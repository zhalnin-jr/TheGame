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
    /// <summary>
    /// Интерфейс для звука.
    /// </summary>
    public interface ISoundProxy
    {
        void PlaySound(string sound);
    }

    /// <summary>
    /// Логирование атаки.
    /// </summary>
    public interface IAttackLogProxy
    {
        void LogAttack(Unit attacker, Unit defender);
    }

    /// <summary>
    /// Логирование спешл абилити.
    /// </summary>
    public interface ISpecialAbilityLogProxy
    {
        void LogSpecialAbility(string abilityName, Unit unit);
    }

    /// <summary>
    /// Воспроизведение звука.
    /// </summary>
    public class SoundProxy : ISoundProxy
    {
        public void PlaySound(string sound)
        {
            Debug.Log($"Playing sound: {sound}");
            // Здесь можно добавить код для воспроизведения звука через Unity
            // Например, используя AudioSource:
            AudioSource audioSource = GameObject.FindObjectOfType<AudioSource>();
            if (audioSource != null)
            {
                AudioClip clip = Resources.Load<AudioClip>(sound);
                if (clip != null)
                {
                    audioSource.PlayOneShot(clip);
                }
                else
                {
                    Debug.LogError($"Audio clip not found: {sound}");
                }
            }
            else
            {
                Debug.LogError("AudioSource not found in the scene.");
            }
        }
    }

    /// <summary>
    /// Форма записи лога.
    /// </summary>
    public class AttackLogProxy : IAttackLogProxy
    {
        public void LogAttack(Unit attacker, Unit defender)
        {
            string logEntry = $"Attack log: {attacker.Name} attacks {defender.Name}\n";
            FileManager.WriteToFile("attackLog.txt", logEntry);
            Debug.Log(logEntry);
        }
    }

    /// <summary>
    /// Форма записи лога. 
    /// </summary>
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

    /// <summary>
    /// Запись в файл и считывание.
    /// </summary>
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
