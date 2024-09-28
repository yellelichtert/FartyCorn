using UnityEngine;

namespace Collectables
{
    public class CollectableBase : MonoBehaviour
    {
        private AudioSource _collectSound;
        protected SpriteRenderer SpriteRenderer;

        public virtual void Start()
        {
            _collectSound = GetComponent<AudioSource>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            _collectSound.Play();
            SpriteRenderer.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Destroy(gameObject);
        }
    }
}
