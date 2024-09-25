using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Obstacles;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;
    private void Awake() => Instance = this;

    [SerializeField] public float moveSpeed;
    [SerializeField] private List<ObstacleBase> obstacles;
    [SerializeField] private GameObject defaultCollectable;
    [SerializeField] private List<GameObject> specialCollectables;
    [SerializeField] private int specialCollectablePropability;
    
    private List<ObstacleBase> _availableObstacles;
    private int _currentDifficulty = 0;
    private float _spawnLocation;
    private bool _directionChanged;
    
    private void Start()
    {
        _spawnLocation = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x+1;
        
        _availableObstacles = _availableObstacles = obstacles.Where(o => o.difficulty <= _currentDifficulty).ToList();
        
        GameController.ScoreChanged += GameControllerOnScoreChanged;
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
        GameController.DirectionChanged += GameControllerOnDirectionChanged;
    }
    
    private void GameControllerOnScoreChanged(int newScore)
    {
        var difficultyValue = newScore / 10;
        if (difficultyValue == _currentDifficulty) return;
        
        _currentDifficulty = difficultyValue;
        SetAvailableObstacles();
    }

    private void GameControllerOnGameStateChanged(GameController.GameState newState)
    {
        if (newState != GameController.GameState.Playing) return;
        
        _currentDifficulty = 0; 
        SetAvailableObstacles();
        SpawnObstacle();
    }

    private void SetAvailableObstacles()
    {
        _availableObstacles = obstacles.Where(o => o.difficulty <= _currentDifficulty).ToList();
    }

    private void SpawnObstacle()
    {
        if (GameController.Instance.CurrentGameState != GameController.GameState.Playing)
            return;
        
        var obstacle = Instantiate(_availableObstacles[Random.Range(0, _availableObstacles.Count)], new Vector2(_spawnLocation, 0), Quaternion.identity);
        obstacle.DestroyEvent += SpawnObstacle;
    }
    
    public GameObject GetRandomCollectable()
    {
        var randomValue = Random.Range(0, 100);
        return randomValue <= specialCollectablePropability ? specialCollectables[Random.Range(0, specialCollectables.Count)] : defaultCollectable;
    }
    
    private void GameControllerOnDirectionChanged(GameController.Direction newDirection)
    {
        _spawnLocation = newDirection == GameController.Direction.Right 
            ? math.abs(_spawnLocation) : -_spawnLocation;
    }
}
