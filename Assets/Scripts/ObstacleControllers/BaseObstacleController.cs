using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I plan on handeling speed increase in this class.
/// currently this wil remain empty and be used as base class.
/// </summary>
public class BaseObstacleController : ObjectController
{ 
    public virtual void Start()
    {
        moveSpeed = GameController.instance.ObstacleSpeed;
        base.Start();
    }

    public virtual void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
