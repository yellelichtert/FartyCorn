using System;
using Enums;
using Managers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviours.Obstacles
{
    public class ObstacleBase : MonoBehaviour
    {
        private const float SpecialCollectableProbability = 100;
        
        [SerializeField] public int difficulty;
        [SerializeField] public float heightVariation;


        protected float _moveSpeed = 2;
        private float _startLocation;
        private Vector2 _movementDirection;
        
        
        
        public event Action DestroyEvent;
    
    
        
        public void Start()
        {
            _startLocation = transform.position.x;
        
            _movementDirection = GameController.Instance.CurrentDirection == Direction.Left 
                ? Vector2.right 
                : Vector2.left;
            
            transform.position = new Vector2(_startLocation, GetRandomHeight());


            var collectable = Random.Range(0, 100) <= SpecialCollectableProbability
                ? CollectableManager.CollectableData
                : CollectableManager.DefaultCollectable;
            
            
            
            var collectables = GameObject.FindGameObjectsWithTag("Collectable");
            foreach (var currentObject in collectables)
            {
                var collectableObject = Instantiate(collectable, currentObject.transform, true);
                collectableObject.transform.position = currentObject.transform.position;
            }
        
            GameController.DirectionChanged += GameControllerOnDirectionChanged;
        }
    
        
        
        public virtual void FixedUpdate()
        {
            transform.Translate(_movementDirection * (_moveSpeed * Time.deltaTime));

            //Destroys obstacle when out of screen.
            if (IsOutOfBounds())
            {
                Destroy(gameObject);
            }
        }

        
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ScoreManager.CurrentScore += 1;
            }
        }
    
        
        
        private void GameControllerOnDirectionChanged(Direction newDirection)
        {
            if (newDirection == Direction.Left)
            {
                _movementDirection = Vector2.right;
                _startLocation = -_startLocation;
            }
            else
            {
                _movementDirection = Vector2.left;
                _startLocation = math.abs(_startLocation);
            }
        }
        
        
        
        private void OnDestroy()
        {
            DestroyEvent?.Invoke();
        }
        
        
        
        private bool IsOutOfBounds()
        {
            return (_startLocation > 0 && transform.position.x < -_startLocation) || (_startLocation < 0 && transform.position.x > math.abs(_startLocation));
        }
        
        
        
        protected virtual float GetRandomHeight()
        {
            var randomValue = Random.Range(0, heightVariation);
            
            if (Random.Range(1,30) % 2 == 1)
            {
                return randomValue;       
            }
            else
            {
                return -randomValue;
            }
        }
    }
}
