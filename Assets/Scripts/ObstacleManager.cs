using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour
{
    //Singleton class.
    public static ObstacleManager Instance;
    private void Awake() => Instance = this;

    [SerializeField] public float moveSpeed;
    [SerializeField] private List<ObstacleBase> obstacles;
    
    private List<ObstacleBase> _availableObstacles;
    private int _currentDifficulty;
    private float _spawnLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        //Sets spawn location to right edge of the screen.
        _spawnLocation = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x+1;
        
        //GameController.ScoreChanged += GameControllerOnScoreChanged;
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
    }

    /// <summary>
    /// Starts spawning obstacles when game state changes to Playing.
    /// </summary>
    /// <param name="newState"></param>
    private void GameControllerOnGameStateChanged(GameController.GameState newState)
    {
        if (newState == GameController.GameState.Playing)
            SpawnObstacle();
    }
    

    /// <summary>
    /// Instantiates a random obstacle from the available obstacles
    /// </summary>
    private void SpawnObstacle()
    {
        if (GameController.Instance.CurrentGameState != GameController.GameState.Playing)
            return;
        
        var obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Count)], new Vector2(_spawnLocation, 0), Quaternion.identity);
        obstacle.DestroyEvent += SpawnObstacle;
    }
    
    
    
}
