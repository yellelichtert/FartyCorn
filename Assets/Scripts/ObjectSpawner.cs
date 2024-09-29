using System.Collections.Generic;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class is currently used to spawn all objects except obstacles.
/// I might change this in the future depending on the need.
/// currently this works, so I do need feel/see the need to put into seperate classes.
/// </summary>


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 playerVector;
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private List<Sprite> cloudSprites = new List<Sprite>();
    [SerializeField] private float minCloudInterval;
    [SerializeField] private float maxCloudInterval;
    
    private Vector2 _screen;
    void Start()
    {
        _screen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
        PlayerController.FirstFlap += InvokeSpawnCloud;
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

