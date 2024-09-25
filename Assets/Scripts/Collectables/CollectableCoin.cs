using UnityEngine;

namespace Collectables
{
    public class CollectableCoin : CollectableBase
    {
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var currentAmount = PlayerPrefs.GetInt("CoinsCollected", 0);
                PlayerPrefs.SetInt("CoinsCollected", currentAmount + 1);
        
                base.OnTriggerEnter2D(other);
            }
        }
    }
}
