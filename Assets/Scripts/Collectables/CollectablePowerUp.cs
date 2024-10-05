using PowerUps;
using Unity.VisualScripting;
using UnityEngine;

namespace Collectables
{
    public class CollectablePowerUp : CollectableBase
    {
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            base.OnTriggerEnter2D(other);

            var currentPowerUp = other.GetComponent<JetPackPowerUp>();
            if (!currentPowerUp)
            {
                other.AddComponent<JetPackPowerUp>();
            }
            else
            {
                currentPowerUp.FillFuel();
            }
            
        }
    }
}
