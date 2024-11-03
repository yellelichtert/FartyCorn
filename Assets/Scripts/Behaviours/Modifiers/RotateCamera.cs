using System.Linq;
using System.Threading.Tasks;
using Enums;
using Managers;
using Model;
using Unity.VisualScripting;
using UnityEngine;

namespace Behaviours.Modifiers
{
    public class RotateCamera : IModifier
    {
        private readonly Modifier _data;
        private readonly Camera _camera;

        public RotateCamera()
        {
           
                
            _data = CollectableManager.Modifiers.First(m => m.Name == GetType().Name);
            _camera = Camera.main;
            
            GameController.GameStateChanged += OnGameStateChanged;
        }

        
        public async void Apply()
        {
            Rotate(180);

            await Task.Delay((int)(_data.Duration * 1000));
            
            if (GameController.Instance.CurrentGameState != GameState.Playing) return;
            
            Rotate(-180);
        }

        
        
        private void Rotate(int degrees)
        {
            _camera.transform.Rotate(new Vector3(0, 0, degrees));
        }


        private void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.GameOver)
                _camera.transform.rotation = default;
        }
            
    }
}