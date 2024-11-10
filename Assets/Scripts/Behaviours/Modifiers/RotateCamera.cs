using System;
using System.Linq;
using System.Threading.Tasks;
using Enums;
using Managers;
using Model;
using UIElements;
using UnityEngine;

namespace Behaviours.Modifiers
{
    public class RotateCamera : IModifier
    {
        private readonly Modifier _data;
        private readonly Camera _camera;
        private readonly CollectableTimer _uiElement;
        
        private bool _removed;
        private Action<GameState> _stateChangedHandler;
        
        public RotateCamera()
        {
            
            _data = CollectableManager.Modifiers.First(m => m.Name == nameof(RotateCamera));
            _camera = Camera.main;

            _uiElement = new CollectableTimer(_data.Duration, _data.Name, Color.cyan); //Dit nog fixe om gwn data object mee te geve?
            
            UIController.Instance.AddModifierElement(_uiElement);
            CollectableManager.ActiveNonStackable.Add(_data);
            
            GameController.GameStateChanged += _ => Remove();
        }

        
        public async void Apply()
        {
            _camera.transform.Rotate(new Vector3(0, 0, 180));


            for (int i = 0; i < _data.Duration*4; i++)
            {
                await Task.Delay(250);
                _uiElement.RemainingDuration = _data.Duration - i/4;
            }
            
            Remove();
        }


        private void Remove()
        {
            if (_removed) return;
            
            _uiElement.RemoveFromHierarchy();
            _camera.transform.rotation = default;
            GameController.GameStateChanged -= _stateChangedHandler;
            CollectableManager.ActiveNonStackable.Remove(_data);
            _removed = true;
        }
    }
}