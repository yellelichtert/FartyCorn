using System;
using System.Collections;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{

    private AudioSource _collectSound;
    private SpriteRenderer _spriteRenderer;
    private bool _isCollected;

    private void Start()
    {
        _collectSound = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isCollected = true;
            
            _collectSound.Play();
            
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }


}
