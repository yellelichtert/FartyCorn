using UnityEngine;

namespace Behaviours.Obstacles
{
    public class Obstacle02 : Obstacle01
    {
        protected override void Awake()
        {
            base.Awake();
            YOffset = Camera.main!.ScreenToViewportPoint(new Vector2(Screen.width, Screen.height)).y;
        }

        private void Update()
        {
            if (YDirection == Vector2.up && transform.position.y >= YBase+YOffset)
            {
                YDirection = Vector2.down;
            }
            else if (transform.position.y <= YBase-YOffset)
            {
                YDirection = Vector2.up;
            } 
        }
    }
}