using System;
using System.Collections.Generic;
using System.IO;
using Behaviours.Collectables;
using Behaviours.PowerUps;
using Collectables;
using Model;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public static class CollectableManager
    {
        public static event Action CollectedCoinsChanged;

        public static readonly CollectableBase DefaultCollectable;
        public static readonly CollectableBase CollectableData;
        
        
        public static readonly List<PowerUp> PowerUps;
        public static readonly List<Modifier> Modifiers;
        public static List<Upgrade> Upgrades;

        public static List<CollectableData> ActiveNonStackable = new();
        
        
        private static int _coinsCollected;
        
        private static readonly string DirectoryPath = Path.Combine(Application.persistentDataPath, DataFiles.FolderName);
        
         static CollectableManager()
         {
             File.Delete(GetFilePath<Modifier>());
             File.Delete(GetFilePath<PowerUp>());
             
             _coinsCollected = PlayerPrefs.GetInt(PlayerPrefKeys.CoinsCollected, 0);
             
             PowerUps = LoadStaticData<PowerUp>();
             Modifiers = LoadStaticData<Modifier>();
             Upgrades = LoadLocalData<Upgrade>();
             
             if ( Upgrades.Count > PowerUps.Count) CleanUpgradesFile();
             
             
             DefaultCollectable = Resources.Load<CollectableCoin>("Collectables/Prefabs/Default");
             CollectableData = Resources.Load<SpecialCollectable>("Collectables/Prefabs/Special");
             
         }


         public static CollectableData GetSpecial()
         {
             CollectableData randomCollectable = null;
             var randomType = Random.Range(0, 2);
             Debug.Log("Random Type: " + randomType);
             
             while (randomCollectable == null || ActiveNonStackable.Contains(randomCollectable))
             {
                 switch (randomType)
                 {
                     case 0:
                         randomCollectable = Modifiers[Random.Range(0, Modifiers.Count)];
                         break;
                     case 1:
                         randomCollectable = PowerUps[Random.Range(0, PowerUps.Count)];
                         break;
                         
                 }
             }
             
             return randomCollectable;
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


        public static int GetUpgradeCost(PowerUp powerUpData)
        {
            var currentLevel = GetUpgrade(powerUpData).UpgradeLevel;

            return (int)(powerUpData.UpgradeCost * (currentLevel > 0 ? currentLevel * powerUpData.UpgradeMultiplier : 1));
        }
        

        
        public static void Upgrade(PowerUp powerUpData)
        {
            CoinsCollected -= GetUpgradeCost(powerUpData);
            GetUpgrade(powerUpData).UpgradeLevel++;
            SaveLocalData(Upgrades);
        }
        

        private static string GetFilePath<T>()
            => Path.Combine(Application.persistentDataPath, DataFiles.FolderName, $"{typeof(T).Name}.json");

        public static Upgrade GetUpgrade(PowerUp powerUpData)
        {
            var upgrade = Upgrades.Find(u => u.UpgradableName == powerUpData.Name);

            if (upgrade == null)
            {
                upgrade = new Upgrade(){UpgradableName = powerUpData.Name};
                Upgrades.Add(upgrade);
            }
            
            return upgrade;
        }


        private static List<T> LoadStaticData<T>()
            => JsonConvert.DeserializeObject<List<T>>(Resources.Load<TextAsset>(Path.Combine(DataFiles.FolderName, typeof(T).Name)).text);
        
        private static List<T> LoadLocalData<T>()
        {
            Debug.Log($"Loading data from {GetFilePath<T>()}");
            
            
            if (!File.Exists(GetFilePath<T>()))
            {
                Debug.Log("Creating new File");
                
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath); 
                }
                
                File.Create(GetFilePath<T>()).Close();
            }
            
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(GetFilePath<T>())) ?? new List<T>();
        }

        private static void SaveLocalData<T>(List<T> data)
            => File.WriteAllText(GetFilePath<T>(), JsonConvert.SerializeObject(data));

        private static void CleanUpgradesFile()
        {
            var newList = new List<Upgrade>();
            foreach (var upgrade in Upgrades)
            {
                if (PowerUps.Find(p => p.Name == upgrade.UpgradableName) != null)
                    newList.Add(upgrade);
            }
            
            Upgrades = newList;
        }
        
    }
}