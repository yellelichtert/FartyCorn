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

        private static readonly string DirectoryPath = Path.Combine(Application.persistentDataPath, DataFiles.FolderName);
        private static readonly string FilePath = Path.Combine(Application.persistentDataPath, DataFiles.PowerUps);
        
         static PowerUpManager()
         {
             _coinsCollected = PlayerPrefs.GetInt(PlayerPrefKeys.CoinsCollected, 0);
             
             string json;
             if (!File.Exists(FilePath))
             {
                 json = Resources.Load<TextAsset>(DataFiles.PowerUps.Replace(".json", "")).text;

                 
                 if (!Directory.Exists(DirectoryPath))
                 {
                     Directory.CreateDirectory(DirectoryPath); 
                 }
                 
                 
                 File.Create(FilePath).Close();
                 File.WriteAllText(Path.Combine(FilePath), json);
                 
             }
             else
             {
                 json = File.ReadAllText(FilePath);
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
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(PowerUps));
        }
    }
}