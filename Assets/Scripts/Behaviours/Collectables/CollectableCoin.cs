using Managers;
using UnityEngine;

namespace Behaviours.Collectables
{
    public class CollectableCoin : CollectableBase
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.Player)) return;
            
            base.OnTriggerEnter2D(other);

            CollectableManager.CoinsCollected++;
        }
    }
}
