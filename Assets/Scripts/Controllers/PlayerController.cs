using System;
using System.Collections;
using Enums;
using PowerUps;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     
    [SerializeField] private float flapForce;
    
    private GameObject _thrustPrefab;
    private AudioSource _flapSound;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private bool _firstFlap = true;
    
    
    
    public static event Action FirstFlap;
    
    
    
    private void Start()
    {
        _flapSound = gameObject.AddComponent<AudioSource>();
        _flapSound.clip = Resources.Load<AudioClip>("Audio/Fart");
        
        _thrustPrefab = Resources.Load<GameObject>("Prefabs/Thrust");
     
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        
        _animator = gameObject.GetComponent<Animator>();
        _animator.enabled = false;
            
        
        GameController.DirectionChanged += GameControllerOnDirectionChanged;
    }


    
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                Flap();
        }
    }

    
    
    private void Flap()
    { 
        if (GetComponent<JetPackPowerUp>()) return;
        
        if (_firstFlap)
        {
            _firstFlap = false;
            FirstFlap?.Invoke();
            _rb.simulated = true;
        }
        
        _flapSound.Play();
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
        var thrust = Instantiate(_thrustPrefab, new Vector2(transform.position.x+0.1f, transform.position.y-0.5f), Quaternion.identity);
        
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