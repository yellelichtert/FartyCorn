using UnityEngine;

namespace Collectables
{
    public class CollectableBase : MonoBehaviour
    {
        private AudioSource _collectSound;
        public SpriteRenderer spriteRenderer;

        public virtual void Start()
        {
            _collectSound = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            _collectSound.Play();
            spriteRenderer.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Destroy(gameObject);
        }
    }
}
