using System.Collections.Generic;
using Controllers;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviours.Spawners
{
    public class ObjectSpawner : MonoBehaviour
    {
    
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Vector2 playerVector;
        
        [SerializeField] private GameObject cloudPrefab;
        [SerializeField] private List<Sprite> cloudSprites = new List<Sprite>();
        [SerializeField] private float minCloudInterval;
        [SerializeField] private float maxCloudInterval;


        [SerializeField]private GameObject backgroundPrefab;
        [SerializeField] private float backgroundMoveSpeed;
        
        private float _backgroundOffset;
        private List<ObjectController> _backgroundControllers = new();
        private GameObject _firstBackgroundObject;
        
        private Vector2 _screen;
        
        
        
        void Start()
        {
            _screen = Camera.main!.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            _backgroundOffset = backgroundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 1.8f;
            
            
            SpawnBackground();
            
            
            GameController.GameStateChanged += GameControllerOnGameStateChanged;
            PlayerController.FirstTap += InvokeSpawnCloud;
            PlayerController.FirstTap += () => SetBackgroundMoveSpeed(backgroundMoveSpeed);
        }


        private void Update()
        {
            if (_firstBackgroundObject.transform.position.x < -7f) SpawnBackground();
        }
        
    
        private void GameControllerOnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                    Instantiate(playerPrefab, playerVector, Quaternion.identity);
                    break;
                case GameState.GameOver:
                    CancelInvoke();
                    break;
            }
            
            if (newState != GameState.Playing) SetBackgroundMoveSpeed(0);
        }
        
        
        
        void SetBackgroundMoveSpeed(float moveSpeed)
        {
            for (var i = 0; i < _backgroundControllers.Count; i++)
            {
                _backgroundControllers[i].moveSpeed = moveSpeed; 
            }
        }


        
        void SpawnBackground()
        {
            bool isFirstSpawn = _firstBackgroundObject == null;
                
            
            Vector3 spawnLocation = isFirstSpawn
                ? new Vector3(8, -_screen.y * 0.57f, 20)
                : new Vector3(_screen.x + _backgroundOffset, -_screen.y * 0.57f, 20);
            
            GameObject obj = Instantiate(backgroundPrefab, spawnLocation, Quaternion.identity);
            
            
            ObjectController objController = obj.GetComponent<ObjectController>();
            objController.moveSpeed = isFirstSpawn ? 0 : backgroundMoveSpeed;
            
            objController.destroyLocation = isFirstSpawn
                ? (objController.destroyLocation / 2) + objController.destroyLocation
                : _backgroundControllers[0].destroyLocation;
            
            
            _backgroundControllers.Add(objController);
            _firstBackgroundObject = obj;
        }

        
    
        void SpawnCloud()
        {
            var spriteRenderer = cloudPrefab.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = cloudSprites[Random.Range(0, cloudSprites.Count-1)];
        
            Instantiate(cloudPrefab,new Vector2(_screen.x, Random.Range(-_screen.y, _screen.y)), Quaternion.identity);
            InvokeSpawnCloud();
        }
    
    
    
        void InvokeSpawnCloud() => Invoke("SpawnCloud", Random.Range(minCloudInterval, maxCloudInterval));
    }
}

