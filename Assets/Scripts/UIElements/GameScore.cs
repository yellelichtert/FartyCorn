using System;
using System.Collections;
using System.Threading.Tasks;
using Enums;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UIElements
{
   public class GameScore : MonoBehaviour
   {
      public TextMeshProUGUI currentScore;
      public TextMeshProUGUI level;
      public Image highScore;
      public Sprite[] highScoreSprites;

      private void Awake()
      {
         ScoreManager.ScoreChanged += OnScoreChanged;
         ScoreManager.LevelChanged += OnLevelChanged;
         ScoreManager.HighScoreChanged += _ => StartCoroutine(nameof(OnHighScoreChanged));
         
         GameController.GameStateChanged += OnGameStateChanged;
         
         SetInitialState();
      }

      private void OnGameStateChanged(GameState obj)
      {
         switch (obj)
         {
            case GameState.Playing:
               currentScore.enabled = true;
               level.enabled = false;
               break;
            
            default:
               StopAllCoroutines();
               SetInitialState();
               break;
         }
      }
      
      private void OnScoreChanged(int newScore)
         => currentScore.text = newScore.ToString();


     
      private IEnumerator OnHighScoreChanged()
      {
         ChangeFlameVisibility(Visibility.Visible);

         var currentIndex = 0;
         while (highScore.enabled)
         {
            highScore.sprite = highScoreSprites[currentIndex];

            currentIndex++;
            if (currentIndex > highScoreSprites.Length-1) 
               currentIndex = 0;

            yield return new WaitForSeconds(0.1f);
            
         }
      }

      private async void OnLevelChanged(int newLevel)
      {
         bool highScoreEnabled = highScore.enabled;
         
         level.alpha = 0;
         level.text = $"Level {newLevel}";
         
         ChangeVisibility();
         await Task.Delay(2500);
         ChangeVisibility();
         return;
         
         
         void ChangeVisibility()
         {
            currentScore.enabled = !currentScore.enabled;
            level.enabled = !level.enabled;
         }
      }

      private void ChangeFlameVisibility(Visibility visibility)
      {
         var color = highScore.color;
         color.a = visibility == Visibility.Hidden ? 0f : 1f;
         
         highScore.color = color;
      }
      

      private void Update()
      {
         if (level.enabled && level.alpha < 1)
         {
            level.alpha += Time.deltaTime;
         }
      }



      private void SetInitialState()
      {
         ChangeFlameVisibility(Visibility.Hidden);
         level.enabled = false;
         currentScore.enabled = false;
      }
   }
}
