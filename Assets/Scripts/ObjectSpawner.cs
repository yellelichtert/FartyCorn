using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector2 playerVector;
    
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] float obstacleInterval;
    [SerializeField] float ObstacleHeightVariation;
    
    [SerializeField] GameObject cloudPrefab;
    [SerializeField] List<Sprite> cloudSprites = new List<Sprite>();
    [SerializeField] float minCloudInterval;
    [SerializeField] float maxCloudInterval;
    
    private Vector2 _screen;
    void Start()
    {
        _screen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
    }

    private void GameControllerOnGameStateChanged(GameController.GameState newState)
    {
        switch (newState)
        {
            case GameController.GameState.Playing:
                Instantiate(playerPrefab, playerVector, Quaternion.identity);
                InvokeRepeating(nameof(SpawnObstacle), 0, obstacleInterval);
                InvokeSpawnCloud();
                break;
            case GameController.GameState.GameOver:
                CancelInvoke();
                break;
        }
    }

    void SpawnObstacle()
    {
        var heightRange = _screen.y-ObstacleHeightVariation;
        var height = Random.Range(-heightRange, heightRange);
        //Instantiates the obstacle out of view (Moving and despawning is handles by the object itself).
        Instantiate(obstaclePrefab, new Vector2(_screen.x+1, height), Quaternion.identity);
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

