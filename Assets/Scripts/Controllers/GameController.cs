using System;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private void Awake() => Instance = this;
    
    
    [FormerlySerializedAs("background")] [SerializeField] private GameObject SkyColor;
    
    private int _highScore = 0;
    private int _currentScore;
    private GameState _currentGameState;
    private GameDirection _currentGameDirection;
    
    
    public static event Action<GameState> GameStateChanged;
    public static event Action<int> ScoreChanged;
    public static event Action<int> HighScoreChanged;
    public static event Action<GameDirection> DirectionChanged;

    
    
    private void Start()
    {
        Application.targetFrameRate = 15;
        
        Instantiate(SkyColor);
        SkyColor.transform.localScale = new Vector2(Screen.width, Screen.height);
        
        HighScore = PlayerPrefs.GetInt("HighScore", 0);

        CurrentGameState = GameState.Menu;
        
        SettingsManager.SetUserSettings();
        
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

    
    
    private int HighScore
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
                CurrentGameDirection = GameDirection.Right;
            }
            else if (_currentGameState == GameState.GameOver)
            {
                var gamePlayObjects = GameObject.FindGameObjectsWithTag("GamePlayObject");
                foreach (var gamePlayObject in gamePlayObjects)
                    Destroy(gamePlayObject);
            }

            if (_currentGameState != GameState.Playing)
            {
                Application.targetFrameRate = 15;
            }
            else
            {
                Application.targetFrameRate = SettingsManager.IsLowPowerMode() ? 30 : 60;
            }
                

            GameStateChanged?.Invoke(value);
        }
    }
    
    
    
    public GameDirection CurrentGameDirection
    {
        get => _currentGameDirection;
        set
        {
            _currentGameDirection = value;
            DirectionChanged?.Invoke(value);
        }
    }
}
