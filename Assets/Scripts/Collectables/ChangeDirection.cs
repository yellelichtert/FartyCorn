using UnityEngine;

namespace Collectables
{
    public class ChangeDirection : CollectableBase
    {
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var newDirection = GameController.Instance.CurrentDirection == GameController.Direction.Left
                    ? GameController.Direction.Right : GameController.Direction.Left; 
        
                GameController.Instance.CurrentDirection = newDirection;
                base.OnTriggerEnter2D(other);   
            }
        }
    }
}
