using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Coin : Collectable_Base
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        var currentAmount = PlayerPrefs.GetInt("CoinsCollected", 0);
        PlayerPrefs.SetInt("CoinsCollected", currentAmount + 1);
        
        base.OnTriggerEnter2D(other);
    }
}
