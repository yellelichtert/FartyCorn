using System;
using Behaviours.Modifiers;
using Behaviours.PowerUps;
using Collectables;
using Managers;
using Model;
using UnityEngine;

namespace Behaviours.Collectables
{
    public class SpecialCollectable : CollectableBase
    {

        private CollectableData _collectableDataData;
        
        
        protected override void Start()
        {
            base.Start();
            
            _collectableDataData = CollectableManager.GetSpecial();
            SpriteRenderer.sprite = Resources.Load<Sprite>(ResourcePaths.CollectableSprites + _collectableDataData.Name);
        }
        
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.Player)) return;

            base.OnTriggerEnter2D(other);
            
            if (_collectableDataData.GetType() == typeof(PowerUp))
            {
                other
                    .gameObject
                    .AddComponent(GetBehaviourType<PowerUpBehaviour>());
            }
            else
            {
                ((IModifier)Activator
                    .CreateInstance(GetBehaviourType<IModifier>()))
                    .Apply();
            }
        }
        
        
        private Type GetBehaviourType<T>()
            => Type.GetType($"{typeof(T).Namespace}.{_collectableDataData.Name}");
    }
}