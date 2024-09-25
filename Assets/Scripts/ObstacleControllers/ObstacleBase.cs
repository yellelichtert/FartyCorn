using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleBase : MonoBehaviour
{
    [SerializeField] public int difficulty;
    [SerializeField] public float heightVariation;
    
    private float _moveSpeed;
    private float _startLocation;
    public event Action DestroyEvent;
    
    public void Start()
    {
        _moveSpeed = ObstacleManager.Instance.moveSpeed;
        _startLocation = transform.position.x;
        
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
    }
    
    public virtual void FixedUpdate()
    {
        //Moves obstacle.
        transform.Translate(Vector3.left * (_moveSpeed * Time.deltaTime));

        //Destroys obstacle when out of screen.
        if (transform.position.x < -_startLocation)
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
}
