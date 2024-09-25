using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float flapForce;
    [SerializeField] GameObject thrustPrefab;
    
    private Rigidbody2D _rb;
    private AudioSource _audio;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        GameController.Instance.CurrentGameState = GameController.GameState.GameOver;
    }

    private IEnumerator Thrust()
    {
        var thrust = Instantiate(thrustPrefab, new Vector2(transform.position.x+0.1f, transform.position.y-0.5f), Quaternion.identity);
        
        var animationLength = thrust.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        
        yield return new WaitForSeconds(animationLength);
        Destroy(thrust.gameObject);
    }
    
    private void GameControllerOnDirectionChanged(GameController.Direction newDirection)
    {
        transform.position = newDirection == GameController.Direction.Left ? new Vector2(math.abs(transform.position.x), transform.position.y) : new Vector2(-transform.position.x, transform.position.y);
    }

    private void OnDestroy()
    {
        GameController.DirectionChanged -= GameControllerOnDirectionChanged;
    }
}
