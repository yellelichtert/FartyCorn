using Enums;
using UnityEngine;

namespace Behaviours.Modifiers
{
    public class ChangeDirection : IModifier
    {
        
        private readonly GameController _gameController = GameController.Instance;
        
        
        
        public void Apply()
        {
            var renderer = GameObject
                .FindWithTag(Tags.Player)
                .GetComponent<SpriteRenderer>();
            
            
            var newDirection = _gameController.CurrentDirection == Direction.Left
                ? Direction.Right 
                : Direction.Left; 
        
            _gameController.CurrentDirection = newDirection;
            
            renderer.flipX = GameController.Instance.CurrentDirection == Direction.Left;
        }
    }
}