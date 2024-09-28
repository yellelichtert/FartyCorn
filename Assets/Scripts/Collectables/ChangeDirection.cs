using Enums;
using UnityEngine;


namespace Collectables
{
    
    public class ChangeDirection : CollectableBase
    {
        private GameController _gameController;
        
        public override void Start()
        {
            _gameController = GameController.Instance;
            
            base.Start();
            SpriteRenderer.flipX = _gameController.CurrentGameDirection == GameDirection.Left;
        }
        
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var newDirection = _gameController.CurrentGameDirection == GameDirection.Left
                ? GameDirection.Right : GameDirection.Left; 
        
            GameController.Instance.CurrentGameDirection = newDirection;
            base.OnTriggerEnter2D(other);
        }
    }
}
