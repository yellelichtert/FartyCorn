using System;
using Enums;
using UnityEngine;

namespace Managers
{
    public static class ScoreManager
    {
        private static int _currentScore;
        private static int _highScore;
        private static int _currentLevel;
        
        public delegate void ScoreChangedHandler(int newScore);
        public static event ScoreChangedHandler ScoreChanged;
        public static event ScoreChangedHandler HighScoreChanged;
        public static event ScoreChangedHandler LevelChanged;



        static ScoreManager()
        {
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
            GameController.GameStateChanged += OnGameStateChanged;

            HighScore = 0;
        }


        private static void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.Playing)
                CurrentScore = 0;
        }

        public static int CurrentScore
        {
            get => _currentScore;
            set
            {   
                _currentScore = value;
                ScoreChanged?.Invoke(_currentScore);
                
                
                if (_highScore < value)
                    HighScore = value;

                if (value == 0)
                    _currentLevel = 0;
                else if (_currentScore % 10 == 0)
                    CurrentLevel++;
            }
        }
        
        
        
        public static int HighScore
        {
            get => _highScore;
            private set
            {
                PlayerPrefs.SetInt("HighScore", value);
                _highScore = value;
                HighScoreChanged?.Invoke(value);
            }
        }
        
        
        
        public static int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                _currentLevel = value;
                LevelChanged?.Invoke(value);
            }
        }
    }
}