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
        
        transform.position = new Vector2(_startLocation, GetRandomHeight());
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        //Moves obstacle.
        transform.Translate(Vector3.left * (_moveSpeed * Time.deltaTime));

        if (transform.position.x < -_startLocation)
        {
            Destroy(gameObject);
        }
    }

    public virtual float GetRandomHeight()
    {
        return transform.position.y + Random.Range(-heightVariation, heightVariation);
    }

private void OnDestroy()
    {
        DestroyEvent?.Invoke();
    }
}
