using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Obstacle_01 : BaseObstacleController
{
    
    [SerializeField] float heightVariation;
    
    public override void Start()
    {
        

        if (Random.Range(1,30) % 2 == 1)
        {
            transform.position = new Vector3(transform.position.x, heightVariation, transform.position.z);            
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -heightVariation, transform.position.z);
        }
        
        base.Start();
    }


    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
