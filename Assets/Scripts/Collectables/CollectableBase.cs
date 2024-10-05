using UnityEngine;

namespace Collectables
{
    public class CollectableBase : MonoBehaviour
    {
        private AudioSource _collectSound;
        private Collider2D _collider2D;
        protected SpriteRenderer SpriteRenderer;

        public virtual void Start()
        {
            _collectSound = GetComponent<AudioSource>();
            _collider2D = GetComponent<Collider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            _collider2D.enabled = false;
            SpriteRenderer.enabled = false;
            
            if (_collectSound != null) _collectSound.Play();
        }
    }
}
