using Managers;
using UnityEngine;

namespace Collectables
{
    public class CollectableCoin : CollectableBase
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            base.OnTriggerEnter2D(other);

            PowerUpManager.CoinsCollected++;
        }
    }
}
