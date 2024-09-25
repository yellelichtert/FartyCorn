using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
        _moveSpeed = ObstacleManager.Instance.moveSpeed;
        _startLocation = transform.position.x;
        
        _movementDirection = GameController.Instance.CurrentDirection == GameController.Direction.Left 
            ? Vector2.right : Vector2.left;
        
        //move to random height.
        transform.position = new Vector2(_startLocation, GetRandomHeight());

        //Get random collectable & collectable locations.
        var randomCollectable = ObstacleManager.Instance.GetRandomCollectable();
        var collectables = GameObject.FindGameObjectsWithTag("Collectable");

        //Fill al collectable location with the random collectable.
        foreach (var collectable in collectables)
        {
            var collectableObject = Instantiate(randomCollectable, collectable.transform, true);
            collectableObject.transform.position = collectable.transform.position;
        }
        
        GameController.DirectionChanged += GameControllerOnDirectionChanged;
    }
    
    public virtual void FixedUpdate()
    {
        //Moves obstacle.
        transform.Translate(_movementDirection * (_moveSpeed * Time.deltaTime));

        //Destroys obstacle when out of screen.
        if ((_startLocation > 0 && transform.position.x < -_startLocation) || (_startLocation < 0 && transform.position.x > math.abs(_startLocation)) )
        {
            Destroy(gameObject);
        }
    }

    public virtual float GetRandomHeight()
    {
        return transform.position.y + Random.Range(-heightVariation, heightVariation);
    }

    //Triggers event in obstacle manager.
    private void OnDestroy()
    {
        DestroyEvent?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.CurrentScore += 1;
        }
    }
    
    private void GameControllerOnDirectionChanged(GameController.Direction newDirection)
    {
        if (newDirection == GameController.Direction.Left)
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
    
}
