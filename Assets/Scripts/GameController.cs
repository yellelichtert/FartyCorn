using System;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private void Awake() => Instance = this;
    
    
    [SerializeField] float obstacleSpeed;
    [SerializeField] GameObject background;
    
    private int _highScore = 0;
    private int _currentScore;
    
    private GameState _currentGameState;
    private Direction _currentDirection;
    
    public enum GameState
    {
        Menu,
        Playing,
        GameOver
    }
    public enum Direction
    {
        Left,
        Right
    }

    public static event Action<GameState> GameStateChanged;
    public static event Action<int> ScoreChanged;
    public static event Action<int> HighScoreChanged;
    public static event Action<Direction> DirectionChanged;

    private void Start()
    {
        var screen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        background.transform.localScale = new Vector2(Screen.width, Screen.height);
    }


    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            
            if (value > HighScore) 
                HighScore = value;
            
            ScoreChanged?.Invoke(value);
        }
    }

    public int HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            PlayerPrefs.SetInt("HighScore", value);
            
            HighScoreChanged?.Invoke(value);
        }
    }
    
    

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;
            
            if (_currentGameState == GameState.Playing)
            {
                CurrentScore = 0;
                CurrentDirection = Direction.Right;
            }
            
            else if (_currentGameState == GameState.GameOver)
            {
                var gamePlayObjects = GameObject.FindGameObjectsWithTag("GamePlayObject");
                foreach (var gamePlayObject in gamePlayObjects)
                    Destroy(gamePlayObject);
            }

            GameStateChanged?.Invoke(value);
        }
    }
    
    public float ObstacleSpeed
    {
        get => obstacleSpeed;
    }

    public Direction CurrentDirection
    {
        get => _currentDirection;
        set
        {
            _currentDirection = value;
            DirectionChanged?.Invoke(value);
        }
    }
}
