using UnityEngine;

namespace Collectables
{
    public class CollectableCoin : CollectableBase
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            base.OnTriggerEnter2D(other);
            
            var currentAmount = PlayerPrefs.GetInt("CoinsCollected", 0);
            PlayerPrefs.SetInt("CoinsCollected", currentAmount + 1);
        }
    }
}
