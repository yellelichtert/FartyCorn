using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Model;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public static class PowerUpManager
    {
        public static event Action CollectedCoinsChanged;
        
        public static List<PowerUp> PowerUps { get;}
        private static int _coinsCollected;

        private readonly static string _directoryPath = Path.Combine(Application.persistentDataPath, DataFiles.FolderName);
        private readonly static string _filePath = Path.Combine(Application.persistentDataPath, DataFiles.PowerUps);
        
         static PowerUpManager()
         {
             string json;
             if (!File.Exists(_filePath))
             {
                 Debug.Log("File Doesn't Exist, creating a new one");
                 
                 Debug.Log("Loading From Resources");
                 json = Resources.Load<TextAsset>(DataFiles.PowerUps.Replace(".json", "")).text;

                 
                 if (!Directory.Exists(_directoryPath))
                 {
                     Debug.Log("Creating folder");
                     Directory.CreateDirectory(_directoryPath); //  ?/
                 }
                 
                 
                 Debug.Log("Creating file at: " + _filePath);
                 File.Create(_filePath).Close();
                 
                 Debug.Log("Writing file");
                 File.WriteAllText(Path.Combine(_filePath), json);
             }
             else
             {
                 Debug.Log("File exists, Loading file");
                 json = File.ReadAllText(_filePath);
             }
             
             PowerUps = JsonConvert
                 .DeserializeObject<List<PowerUp>>(json);
         }

         
        
         public static int CoinsCollected
         {
             get => _coinsCollected;
             set
             {
                 _coinsCollected = value;
                 PlayerPrefs.SetInt(PlayerPrefKeys.CoinsCollected, value);
                 CollectedCoinsChanged?.Invoke();
             }
         }
         
        public static PowerUp GetRandom() 
            => PowerUps[Random.Range(0, PowerUps.Count-1)];

        public static int GetUpgradeCost(PowerUp powerUp)
            => (int)(powerUp.UpgradeCost * (powerUp.CurrentLevel > 0 ? powerUp.CurrentLevel * powerUp.UpgradeMultiplier : 1));

        
        public static void Upgrade(PowerUp powerUp)
        {
            CoinsCollected -= GetUpgradeCost(powerUp);
            powerUp.CurrentLevel++;
            File.WriteAllText("FilePath", JsonConvert.SerializeObject(PowerUps));
        }
    }
}