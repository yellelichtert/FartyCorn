using System;
using Enums;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class ObstacleBase : MonoBehaviour
    {
        
        [SerializeField] public int difficulty;
        [SerializeField] public float heightVariation;
    
        
        private float _moveSpeed;
        private float _startLocation;
        private Vector2 _movementDirection;
        
        
        
        public event Action DestroyEvent;
    
    
        
        public void Start()
        {
            _moveSpeed = ObstacleManager.Instance.MoveSpeed;
            _startLocation = transform.position.x;
        
            _movementDirection = GameController.Instance.CurrentGameDirection == GameDirection.Left 
                ? Vector2.right 
                : Vector2.left;
            
            transform.position = new Vector2(_startLocation, GetRandomHeight());
            
            var randomCollectable = ObstacleManager.Instance.GetRandomCollectable();
            var collectables = GameObject.FindGameObjectsWithTag("Collectable");
            
            foreach (var collectable in collectables)
            {
                var collectableObject = Instantiate(randomCollectable, collectable.transform, true);
                collectableObject.transform.position = collectable.transform.position;
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
                GameController.Instance.CurrentScore += 1;
            }
        }
    
        
        
        private void GameControllerOnDirectionChanged(GameDirection newGameDirection)
        {
            if (newGameDirection == GameDirection.Left)
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
            return transform.position.y + Random.Range(-heightVariation, heightVariation);
        }
    }
}
