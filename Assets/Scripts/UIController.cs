using System;
using Enums;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private Texture2D tapToStartSprite;
    
    public static UIController Instance;

    private VisualElement _uiRoot;
    private VisualElement _mainMenu;
    
    void Awake()
    {
        Instance = this;
        
        _uiRoot = GetComponent<UIDocument>().rootVisualElement;
        _mainMenu = _uiRoot.Q<VisualElement>("MainMenu");
        
        Button playButton = _uiRoot.Q<Button>("PlayButton");
        playButton.clicked += PlayButtonOnClicked;
        
        Button settingsButton = _uiRoot.Q<Button>("SettingsButton");
        settingsButton.clicked += SettingsButtonOnclicked;
        
        Button upgradesButton = _uiRoot.Q<Button>("UpgradeButton");
        upgradesButton.clicked += UpgradesButtonOnonClick;
        
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
        GameController.ScoreChanged += GameControllerOnScoreChanged;
        GameController.HighScoreChanged += GameControllerOnHighScoreChanged;
    }
    
    private void GameControllerOnHighScoreChanged(int newHighScore)
    {
        
    }

    private void GameControllerOnScoreChanged(int newScore)
    {
        
    }

    private void GameControllerOnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                _mainMenu.visible = true;
               break;
            case GameState.Playing:
                _mainMenu.visible = false;
              break;
            case GameState.GameOver:
                _mainMenu.visible = true; //Temporary untill gameover screen is done.
                break;
        }
    }

    private void PlayButtonOnClicked()
    {
        GameController.Instance.CurrentGameState = GameState.Playing;
    }
    
    private void SettingsButtonOnclicked()
    {
        throw new NotImplementedException();
    }
    
    private void UpgradesButtonOnonClick()
    {
        throw new NotImplementedException();
    }
}
