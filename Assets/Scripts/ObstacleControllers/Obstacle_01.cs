using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Obstacle_01 : ObstacleBase
{
    
    
    //switches between up and down 1 in 2 chance.
    public override float GetRandomHeight()
    {
        //Set random height.
        if (Random.Range(1,30) % 2 == 1)
        {
            return heightVariation;       
        }
        else
        {
            return -heightVariation;
        }
    }
}
