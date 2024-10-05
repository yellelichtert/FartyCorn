using Enums;
using UnityEngine;

namespace Collectables
{
    public class ChangeDirection : CollectableBase
    {
        
        private GameController _gameController;
        
        
        
        protected override void Start()
        {
            _gameController = GameController.Instance;
            
            base.Start();
            SpriteRenderer.flipX = _gameController.CurrentGameDirection == GameDirection.Left;
        }
        
        
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var newDirection = _gameController.CurrentGameDirection == GameDirection.Left
                ? GameDirection.Right 
                : GameDirection.Left; 
        
            _gameController.CurrentGameDirection = newDirection;
            base.OnTriggerEnter2D(other);
        }
    }
}
