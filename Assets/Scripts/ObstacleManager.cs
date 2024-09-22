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
    private int _currentDifficulty = 0;
    private float _spawnLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        //Sets spawn location to right edge of the screen.
        _spawnLocation = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x+1;
        
        //Sets initial obstacles
        _availableObstacles = _availableObstacles = obstacles.Where(o => o.difficulty <= _currentDifficulty).ToList();
        
        GameController.ScoreChanged += GameControllerOnScoreChanged;
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
    }
    
    /// <summary>
    /// Adds obstacles with greater difficulty
    /// </summary>
    private void GameControllerOnScoreChanged(int newScore)
    {
        int difficultyValue = newScore / 10;
        if (difficultyValue != _currentDifficulty)
        {
            _currentDifficulty = difficultyValue;
            SetAvailableObstacles();
        }
                    
    }

    /// <summary>
    /// Starts spawning obstacles when game state changes to Playing.
    /// </summary>
    private void GameControllerOnGameStateChanged(GameController.GameState newState)
    {
        if (newState == GameController.GameState.Playing)
        {
            _currentDifficulty = 0; 
            SetAvailableObstacles();
            SpawnObstacle();
        }
    }
    
    //
    private void SetAvailableObstacles()
    {
           _availableObstacles = obstacles.Where(o => o.difficulty <= _currentDifficulty).ToList();
    }

    /// <summary>
    /// Instantiates a random obstacle from the available obstacles
    /// </summary>
    private void SpawnObstacle()
    {
        if (GameController.Instance.CurrentGameState != GameController.GameState.Playing)
            return;
        
        var obstacle = Instantiate(_availableObstacles[Random.Range(0, _availableObstacles.Count)], new Vector2(_spawnLocation, 0), Quaternion.identity);
        obstacle.DestroyEvent += SpawnObstacle;
    }
}
