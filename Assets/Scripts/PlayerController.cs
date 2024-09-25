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
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        GameController.DirectionChanged += GameControllerOnDirectionChanged;
    }
    
    public void Flap()
    {
        Debug.unityLogger.Log("Bird Flapped");
        //Adds upward force to player
        _rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
        StartCoroutine(Thrust());
    }
    
    void OnCollisionEnter2D(Collision2D collision)
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
        if (newDirection == GameController.Direction.Left)
        {
            transform.position = new Vector2(math.abs(transform.position.x), transform.position.y);
        }
        else
        {
            transform.position = new Vector2(-transform.position.x, transform.position.y);
        }
    }

    private void OnDestroy()
    {
        GameController.DirectionChanged -= GameControllerOnDirectionChanged;
    }
}
