using System;
using System.Collections.Generic;
using System.IO;
using Behaviours.Collectables;
using Behaviours.PowerUps;
using Collectables;
using Enums;
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
        
        
        private static bool _hasPowerUp;
        private static int _coinsCollected;
        
        private static readonly string DirectoryPath = Path.Combine(Application.persistentDataPath, DataFiles.FolderName);
        
         static CollectableManager()
         {
             _coinsCollected = PlayerPrefs.GetInt(PlayerPrefKeys.CoinsCollected, 0);
             
             PowerUps = LoadData<PowerUp>();
             Modifiers = LoadData<Modifier>();
             
             DefaultCollectable = Resources.Load<CollectableCoin>("Collectables/Prefabs/Default");
             CollectableData = Resources.Load<SpecialCollectable>("Collectables/Prefabs/Special");
             
             PowerUpBehaviour.PowerUpRemoved += () => _hasPowerUp = false;
             GameController.GameStateChanged += state => _hasPowerUp = false;
         }


         public static CollectableData GetSpecial()
         {
             if (_hasPowerUp || Random.Range(0, 1) != 0) 
                 return Modifiers[Random.Range(0, Modifiers.Count)];
             
             
             _hasPowerUp = true;
             return PowerUps[Random.Range(0, PowerUps.Count)];
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
            => (int)(powerUpData.UpgradeCost * (powerUpData.CurrentLevel > 0 ? powerUpData.CurrentLevel * powerUpData.UpgradeMultiplier : 1));

        
        public static void Upgrade(PowerUp powerUpData)
        {
            CoinsCollected -= GetUpgradeCost(powerUpData);
            powerUpData.CurrentLevel++;
            File.WriteAllText(GetFilePath<PowerUp>(), JsonConvert.SerializeObject(PowerUps));
        }


        private static string GetFilePath<T>()
            => Path.Combine(Application.persistentDataPath, DataFiles.FolderName, $"{typeof(T).Name}.json");
        
        
        private static List<T> LoadData<T>()
        {
            Debug.Log($"Loading data from {GetFilePath<T>()}");
            
            string json;
            if (!File.Exists(GetFilePath<T>()))
            {
                Debug.Log("Loading default data from : " + Path.Combine(DataFiles.FolderName, typeof(T).Name));
                json = Resources.Load<TextAsset>(Path.Combine(DataFiles.FolderName, typeof(T).Name)).text;

                 
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath); 
                }
                
                File.Create(GetFilePath<T>()).Close();
                File.WriteAllText(Path.Combine(GetFilePath<T>()), json);
                 
            }
            else
            {
                json = File.ReadAllText(GetFilePath<T>());
            }
            
            Debug.Log("Done Loading data");
            
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}