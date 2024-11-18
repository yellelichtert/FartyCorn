using System;
using Enums;
using UnityEngine;



public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private void Awake() => Instance = this;
    
    
    [SerializeField] private GameObject SkyColor;
    

    private GameState _currentGameState;
    private Direction _currentDirection;
    
    
    public static event Action<GameState> GameStateChanged;
    public static event Action<Direction> DirectionChanged;

    
    
    private void Start()
    {
        Application.targetFrameRate = 15;
        
        Instantiate(SkyColor);
        SkyColor.transform.localScale = new Vector2(Screen.width, Screen.height);
        

        CurrentGameState = GameState.Menu;
        
        SettingsManager.SetUserSettings();
        
    }

    

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;
            
            if (_currentGameState == GameState.Playing)
            {
                CurrentDirection = Direction.Right;
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
