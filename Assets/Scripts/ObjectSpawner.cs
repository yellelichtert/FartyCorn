using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector2 playerVector;
    
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
                InvokeSpawnCloud();
                break;
            case GameController.GameState.GameOver:
                CancelInvoke();
                break;
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

