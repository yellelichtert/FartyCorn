using System.Collections.Generic;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class ObjectSpawner : MonoBehaviour
    {
    
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Vector2 playerVector;
        
        [SerializeField] private GameObject cloudPrefab;
        [SerializeField] private List<Sprite> cloudSprites = new List<Sprite>();
        [SerializeField] private float minCloudInterval;
        [SerializeField] private float maxCloudInterval;


        [SerializeField] private GameObject backgroundPrefab;
        [SerializeField] private float backgroundMoveSpeed;
        private float _backgroundOffset;
        private List<ObjectController> _backgroundControllers = new();
        private GameObject _firstBackgroundObject;
        
        private Vector2 _screen;
        
        void Start()
        {
            _screen = Camera.main!.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        
            _firstBackgroundObject = Instantiate(backgroundPrefab,  new Vector3(8, -_screen.y * 0.57f, 20), Quaternion.identity);
            
            var objController = _firstBackgroundObject.GetComponent<ObjectController>();
            objController.destroyLocation = (objController.destroyLocation / 2) + objController.destroyLocation;
            _backgroundControllers.Add(objController);
            
            _backgroundOffset = backgroundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 1.8f;
            
            GameController.GameStateChanged += GameControllerOnGameStateChanged;
            PlayerController.FirstTap += InvokeSpawnCloud;
            PlayerController.FirstTap += () => SetBackgroundMoveSpeed(backgroundMoveSpeed);
        }


        private void Update()
        {
            if (_firstBackgroundObject.transform.position.x < -7f)
            {
                var obj = Instantiate(backgroundPrefab, new Vector3(_screen.x + _backgroundOffset, -_screen.y * 0.57f, 20), Quaternion.identity);
                var objController = obj.GetComponent<ObjectController>();
                objController.destroyLocation = _backgroundControllers[0].destroyLocation;
                objController.moveSpeed = backgroundMoveSpeed;
                
                _backgroundControllers.Add(objController);
                _firstBackgroundObject = obj;
            }
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

