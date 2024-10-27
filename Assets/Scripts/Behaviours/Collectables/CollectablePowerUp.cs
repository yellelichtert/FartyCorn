using System;
using System.Linq;
using Behaviours.PowerUps;
using Collectables;
using Managers;
using Model;
using UnityEngine;

namespace Behaviours.Collectables
{
    public class CollectablePowerUp : CollectableBase
    {

        private PowerUp _powerUp;
        
        
        protected override void Start()
        {
            base.Start();
            

            _powerUp = PowerUpManager.GetRandom();
            SpriteRenderer.sprite = Resources.Load<Sprite>(ResourcePaths.CollectableSprites + _powerUp.Name);
        }
        
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.Player)) return;

            base.OnTriggerEnter2D(other);
            
            var currentPowerUp = other.GetComponent<PowerUpBehaviour>();
            if (!currentPowerUp)
            {
                var powerUp = (PowerUpBehaviour)other.gameObject.AddComponent(
                    Type.GetType(typeof(PowerUpBehaviour).Namespace + "." + _powerUp.Name));
            }
            else
            {
                currentPowerUp.ResetDuration();   
            }
        }
    }
}
