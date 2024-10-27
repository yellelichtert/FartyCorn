using System;
using System.Collections.Generic;
using System.Linq;
using Behaviours.PowerUps;
using Enums;
using JetBrains.Annotations;
using Managers;
using Model;
using UIElements;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    private GameController _gameController;
    
    private VisualElement _uiRoot;
    
    private VisualElement _mainMenu;
    private VisualElement _settings;
    private VisualElement _upgrades;
    
    
    private VisualElement _gameOver;
    private VisualElement _share; 
    
    
    private VisualElement _currentMenu;
    private VisualElement _currentSubMenu;
    
    private Label _gameScore;
    private Label _highScore;
    

    [CanBeNull] private VisualElement _powerUpElement;
    
    
    void Awake()
    {
        Instance = this;
        
        _gameController = GameController.Instance;
        _uiRoot = GetComponent<UIDocument>().rootVisualElement;
        
        
        _mainMenu = _uiRoot.Q<VisualElement>("MainMenu");
        _settings = _uiRoot.Q<VisualElement>("Settings");
        _upgrades = _uiRoot.Q<VisualElement>("Upgrades");
        
        Button playButton = _uiRoot.Q<Button>("PlayButton");
        playButton.clicked += PlayButtonOnClicked;
        
        Button settingsButton = _uiRoot.Q<Button>("SettingsButton");
        settingsButton.clicked += SettingsButtonOnClicked;
        
        Button upgradesButton = _uiRoot.Q<Button>("UpgradesButton");
        upgradesButton.clicked += UpgradesButtonOnClick;
        
        _settings.visible = false;
        _upgrades.visible = false;
        
        _currentMenu = _mainMenu;
        
        
        _gameOver = _uiRoot.Q<VisualElement>("GameOver");
        _share = _uiRoot.Q<VisualElement>("Share");
        
        Button playAgainButton = _uiRoot.Q<Button>("PlayAgainButton");
        playAgainButton.clicked += PlayButtonOnClicked;
        
        Button menuButton = _uiRoot.Q<Button>("MenuButton");
        menuButton.clicked += MenuButtonOnClicked;
        
        Button shareButton = _uiRoot.Q<Button>("ShareButton");
        shareButton.clicked += ShareButtonOnclicked;

        _gameOver.visible = false;
        _share.visible = false;
        
        _uiRoot.Query<Button>(name: "BackButton")
            .ForEach((backButton) =>
            {
                backButton.clicked += BackButtonOnclicked;
            });
        
        
        _highScore = _uiRoot.Q<Label>("HighScoreValue");
        _gameScore = _uiRoot.Q<Label>("GameScoreValue");
        
        
        Toggle soundToggle = _uiRoot.Q<Toggle>("SoundToggle");
        soundToggle.value = SettingsManager.IsSoundEnabled();
        soundToggle.RegisterValueChangedCallback(SoundToggled);
        
        Toggle lowPowerModeToggle = _uiRoot.Q<Toggle>("LowPowerModeToggle");
        lowPowerModeToggle.value = SettingsManager.IsLowPowerMode();
        lowPowerModeToggle.RegisterValueChangedCallback(LowPowerToggle);
        
        
        VisualElement upgradebars =_uiRoot.Query<VisualElement>(name: "UpgradeBars");

        Label totalCoins = new()
        {
            text = $"Total Coins: {PowerUpManager.CoinsCollected}"
        };
        totalCoins.style.fontSize = 40;
        
        upgradebars.Add(totalCoins);
        
        foreach (var powerUp in PowerUpManager.PowerUps)
        {
            if (powerUp.UpgradeLevels == 0) continue;
            upgradebars.Add(new UpgradeBar(powerUp, upgradebars));
        }
        
        
        GameController.GameStateChanged += GameControllerOnGameStateChanged;
        GameController.ScoreChanged += GameControllerOnScoreChanged;
        GameController.HighScoreChanged += GameControllerOnHighScoreChanged;
        
        PowerUpBehaviour.PowerUpAdded += OnPowerUpAdded;
        PowerUpBehaviour.PowerUpRemoved += RemovePowerUpElement;
        
        PowerUpManager.CollectedCoinsChanged += () => totalCoins.text = $"Total Coins: {PowerUpManager.CoinsCollected}";
    }




    //
    //Button Handlers    
    //
    private void MenuButtonOnClicked()
        => _gameController.CurrentGameState = GameState.Menu;
   
    private void PlayButtonOnClicked()
        => _gameController.CurrentGameState = GameState.Playing;
    
    private void SettingsButtonOnClicked()
        => OpenSubMenu(_settings);
    
    private void UpgradesButtonOnClick()
        => OpenSubMenu(_upgrades);

    private void ShareButtonOnclicked()
        => OpenSubMenu(_share);
    
    private void BackButtonOnclicked()
        => CloseSubMenu();

    private void SoundToggled(ChangeEvent<bool> e)
        => SettingsManager.ToggleSound(e.newValue);
    
    private void LowPowerToggle(ChangeEvent<bool> e)
        => SettingsManager.ToggleLowPowerMode(e.newValue);

    
    
    //
    //Game Events
    //
    private void GameControllerOnHighScoreChanged(int newHighScore)
        => _highScore.text = newHighScore.ToString();
    
    private void GameControllerOnScoreChanged(int newScore)
        => _gameScore.text = newScore.ToString();
    
    
    
    //
    // Manages Ui changes
    //
    private void GameControllerOnGameStateChanged(GameState newState)
    {
        _currentMenu.visible = false;
        
        switch (newState)
        {
            case GameState.Menu:
                _currentMenu = _mainMenu;
               break;
            case GameState.GameOver:
                _currentMenu = _gameOver;
                if (_powerUpElement != null) RemovePowerUpElement();
                break;
        }
        
        _currentMenu.visible = newState != GameState.Playing;
    }
    
    
    //
    //Methods
    //
    private void OpenSubMenu(VisualElement subMenu)
    {
        _currentSubMenu = subMenu;
        
        
        _currentMenu.visible = false;
        _currentSubMenu.visible = true;
    }

    
    private void CloseSubMenu()
    {
        _currentSubMenu.visible = false;
        _currentMenu.visible = true;
    }

    private void OnPowerUpAdded(PowerUp x, VisualElement uiElement)
    {
        _powerUpElement = uiElement;
        _uiRoot.Add(_powerUpElement);
    }


    private void RemovePowerUpElement()
    {
        if (_powerUpElement != null)
        {
            _uiRoot.Remove(_powerUpElement);
            _powerUpElement = null;
        }
    }
}
