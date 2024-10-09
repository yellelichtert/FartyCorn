using System;
using Data;
using PowerUps;
using UnityEngine;

namespace Collectables
{
    public class CollectablePowerUp : CollectableBase
    {
        
        private PowerUpBase _powerUp; 
        
        
        
        protected override void Start()
        {
            base.Start();
            
            
            _powerUp = Data.PowerUps.GetRandom(); 
            SpriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{_powerUp.GetType()}");
            
            Debug.Log("CollectablePowerUpNAme: " + _powerUp.GetType());
        }

        

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            base.OnTriggerEnter2D(other);

            var currentPowerUp = other.GetComponent<PowerUpBase>();
            if (!currentPowerUp)
            {
                other.gameObject.AddComponent(_powerUp.GetType());
            }
            else
            {
                
            }
        }
    }
}
