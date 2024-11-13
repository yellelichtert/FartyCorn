using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UIElements
{
    public class GameScoreOLD : VisualElement
    {
        private Label _levelUp;
        private Label _currentScore;
        private bool _isSwitchingColors;
        
        public GameScoreOLD()
        {
            
            var font = Resources.Load<Font>("Fonts/Font_Pixel");
            
            style.position = Position.Absolute;
            style.top = Length.Percent(5);
            style.unityFontDefinition = new FontDefinition() { font = font };
            style.flexGrow = 0;
            style.flexShrink = 0;
            style.alignSelf = Align.Center;
            style.width = Length.Percent(100);
            style.unityTextAlign = TextAnchor.MiddleCenter;
            style.top = Length.Percent(8);
            
            _currentScore = new Label
            {
                style =
                {
                    position = Position.Absolute,
                    fontSize = Length.Percent(150),
                    color = Color.white,
                    alignContent = Align.Center,
                    flexGrow = 0,
                    flexShrink = 0,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                    right = Length.Percent(5)
                },
                text = "0"
            };
            Add(_currentScore);
            
            _levelUp = new Label
            {
                style =
                {
                    position = Position.Absolute,
                    fontSize = Length.Percent(150),
                    color = Color.white,
                    alignContent = Align.Center,
                    flexGrow = 0,
                    flexShrink = 0,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                    right = Length.Percent(5),
                },
            };
            Add(_levelUp);
            
            
            
            
            _currentScore.text = "0";

            GameController.GameStateChanged += OnGameStateChanged;
            ScoreManager.ScoreChanged += OnScoreChanged;
            ScoreManager.HighScoreChanged += OnHighScoreChanged;
            ScoreManager.LevelChanged += OnLevelChanged;
        }

        private async void OnLevelChanged(int newLevel)
        {
            _levelUp.text = $"Level {ScoreManager.CurrentLevel}";
            
            ChangeVisibility();
            
            
            var fadeTime = 1000;
            _levelUp.style.opacity = 0;
            while (fadeTime != 0)
            {
                _levelUp.style.opacity = new StyleFloat() { };
                await Task.Delay(10);
                fadeTime -= 10;
            }
            
            await Task.Delay(1500);
            ChangeVisibility();
            return;

            
            void ChangeVisibility()
            {
                _currentScore.visible = !_currentScore.visible;
                _levelUp.visible = !_currentScore.visible;
            }
        }

        private void OnGameStateChanged(GameState newState)
        { 
            if(newState == GameState.GameOver) _isSwitchingColors = false;
            _currentScore.style.color = Color.white;
        }
            
        
        private void OnScoreChanged(int newScore)
        {
            _currentScore.text = newScore.ToString();
        }


        private void OnHighScoreChanged(int _) => RainbowColor();
        private async void RainbowColor()
        {
            if (_isSwitchingColors) return;
            _isSwitchingColors = true;
            
            List<Color> colors = new List<Color>()
            {
                Color.red,
                Color.yellow,
                Color.green,
                Color.blue,
                Color.magenta
            };
            
            while (_isSwitchingColors)
            {
                foreach (var color in colors)
                {
                    _currentScore.style.color = color;
                    await Task.Delay(125);
                }
            }
        }
    }
}