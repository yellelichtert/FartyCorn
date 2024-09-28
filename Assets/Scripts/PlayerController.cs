using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float flapForce;
    [SerializeField] GameObject thrustPrefab;
    
    private Rigidbody2D _rb;
    private AudioSource _audio;
    private SpriteRenderer _renderer;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        
        GameController.DirectionChanged += GameControllerOnDirectionChanged;
    }
    
    public void Flap()
    {
        _rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
        StartCoroutine(Thrust());
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        GameController.Instance.CurrentGameState = GameState.GameOver;
    }

    private IEnumerator Thrust()
    {
        var thrust = Instantiate(thrustPrefab, new Vector2(transform.position.x+0.1f, transform.position.y-0.5f), Quaternion.identity);
        
        var animationLength = thrust.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        
        yield return new WaitForSeconds(animationLength);
        Destroy(thrust.gameObject);
    }
    
    private void GameControllerOnDirectionChanged(GameDirection newGameDirection)
    {
        transform.position = newGameDirection == GameDirection.Left ? new Vector2(math.abs(transform.position.x), transform.position.y) : new Vector2(-transform.position.x, transform.position.y);
        _renderer.flipX = newGameDirection == GameDirection.Left;
    }

    private void OnDestroy()
    {
        GameController.DirectionChanged -= GameControllerOnDirectionChanged;
    }
}
