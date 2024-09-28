using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public static UIController Instance;

    private UIDocument _uiDoc;
    private Label _currentScoreLabel;

    private VisualElement _menu;
    private Label _logo;
    
    private Label _menuScoreLabel;
    private Label _highScoreLabel;
    private Button _playButton;
    
    private Animator _playerSpriteAnimator;
    
    void Awake()
    {
        Instance = this;
        
        _uiDoc = GetComponent<UIDocument>();
        
        _currentScoreLabel = _uiDoc.rootVisualElement.Q<Label>("currentScore");
        
        _logo = _uiDoc.rootVisualElement.Q<Label>("logo");
        _menu = _uiDoc.rootVisualElement.Q<VisualElement>("menu");
        _menuScoreLabel = _menu.Q<Label>("menuCurrentScore");
        _highScoreLabel = _uiDoc.rootVisualElement.Q<Label>("highScore");
        _playButton = _uiDoc.rootVisualElement.Q<Button>("playButton");
        
        _playButton.clicked += PlayButtonOnClicked;
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
        GameController.ScoreChanged += GameControllerOnScoreChanged;
        GameController.HighScoreChanged += GameControllerOnHighScoreChanged;
    }

    private void GameControllerOnHighScoreChanged(int newHighScore)
    {
        _highScoreLabel.text = "Highscore: " + newHighScore;
    }

    private void GameControllerOnScoreChanged(int newScore)
    {
        _currentScoreLabel.text = newScore.ToString();
        _menuScoreLabel.text = "Score: " + newScore;
    }

    private void GameControllerOnGameStateChanged(GameController.GameState newState)
    {
        switch (newState)
        {
            case GameController.GameState.Menu:
                _logo.visible = true;
                _currentScoreLabel.visible = false;
                _menu.visible = true;
                _menuScoreLabel.visible = true;
                _currentScoreLabel.visible = false;
                break;
            case GameController.GameState.Playing:
                _logo.visible = false;
                _currentScoreLabel.visible = true;
                _playButton.visible = false;
                _highScoreLabel.visible = false;
                _menu.visible = false;
                _currentScoreLabel.visible = true;
                _menuScoreLabel.visible = false;
                _logo.visible = false;
                break;
            case GameController.GameState.GameOver:
                _logo.visible = true;
                _playButton.text = "Again";
                _playButton.visible = true;
                _highScoreLabel.visible = true;
                _menu.visible = true;
                _menuScoreLabel.visible = true;
                _currentScoreLabel.visible = false;
                _logo.text = "Game Over";
                _logo.visible = true;
                break;
        }
    }

    private void PlayButtonOnClicked()
    {
        GameController.Instance.CurrentGameState = GameController.GameState.Playing;
    }

    
}
