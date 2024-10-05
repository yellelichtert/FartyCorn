using System;
using Data;
using PowerUps;
using Unity.VisualScripting;
using UnityEngine;

namespace Collectables
{
    public class CollectablePowerUp : CollectableBase
    {
        
        private PowerUpBase _powerUpType; //This is the variable I want to use
        
        protected override void Start()
        {
            base.Start();
            
            Debug.Log("CollectablePowerUpNAme: " + _powerUpType.GetType());
            
            _powerUpType = PowerUpTypes.GetRandom(); 
            SpriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{_powerUpType.GetType()}");
        }

        
        
        /// <summary>
        ///
        /// Dit uitzoeke, nullrefference maar    other.gameObject.AddComponent(_powerUpType.GetType()); zou moete werke
        /// Hier verder gaan
        /// </summary>
        /// <param name="other"></param>
        /// <exception cref="NotImplementedException"></exception>

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            base.OnTriggerEnter2D(other);

            var currentPowerUp = other.GetComponent<PowerUpBase>();
            if (!currentPowerUp)
            {
                other.gameObject.AddComponent(_powerUpType.GetType());
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
