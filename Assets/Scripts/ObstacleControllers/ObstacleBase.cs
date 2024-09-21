using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleBase : MonoBehaviour
{
    [SerializeField] public int difficulty;
    [SerializeField] public float heightVariation;
    public event Action DestroyEvent;
    private float _moveSpeed;
    private float _startLocation;

    public void Start()
    {
        _moveSpeed = ObstacleManager.Instance.moveSpeed;
        _startLocation = transform.position.x;
        
        //move to random height.
        transform.position = new Vector2(_startLocation, GetRandomHeight());
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
}
