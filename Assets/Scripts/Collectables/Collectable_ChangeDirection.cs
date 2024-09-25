using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_ChangeDirection : Collectable_Base
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
