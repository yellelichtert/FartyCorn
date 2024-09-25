using UnityEngine;

namespace Collectables
{
    public class CollectableBase : MonoBehaviour
    {
        private AudioSource _collectSound;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _collectSound = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            _collectSound.Play();
            _spriteRenderer.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Destroy(gameObject);
        }
    }
}
