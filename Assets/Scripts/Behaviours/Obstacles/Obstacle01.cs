using UnityEngine;

namespace Behaviours.Obstacles
{
    public class Obstacle01 : ObstacleBase
    { 
        protected Vector2 YDirection;
        protected float YBase;
        protected float YOffset;
        private float _yMoveSpeed;


        protected virtual void Awake()
        {
            _yMoveSpeed = 1;
            
            YDirection = Random.Range(0, 2) == 0
                ? Vector2.up
                : Vector2.down;
            
            YBase = transform.position.y;
            YOffset = Camera.main!.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        }
        
        
        protected override void FixedUpdate()
        {
            transform.Translate(YDirection * (_yMoveSpeed * Time.fixedDeltaTime));
            base.FixedUpdate();
        }
    }
}