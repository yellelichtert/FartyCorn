using System;
using System.Collections;
using Behaviours.PowerUps;
using Enums;
using Model;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
    
    
        [SerializeField] private float tapForce;
        [SerializeField] private GameObject thrustPrefab;
    
        private AudioSource _audio;
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
        private Animator _animator;
        private bool _firstTap = true;
    
    
    
        public static event Action FirstTap;
    

    

        private void Start()
        {
            _audio = gameObject.AddComponent<AudioSource>();
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = gameObject.GetComponent<Animator>();
        
            SetToDefault();

            PowerUpBehaviour.PowerUpAdded += OnPowerUpAdded;
            PowerUpBehaviour.PowerUpRemoved += SetToDefault;
        
            GameController.DirectionChanged += OnDirectionChanged;
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
            if (GetComponent<PowerUpBehaviour>()) return;
        
        
            if (_firstTap)
            {
                _firstTap = false;
                FirstTap?.Invoke();
                _rb.simulated = true;
            }
        
            _audio.Play();
            _rb.AddForce(Vector2.up * tapForce, ForceMode2D.Impulse);
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
    
    
    
        private void OnDirectionChanged(Direction newDirection)
        {
            transform.position = newDirection == Direction.Left ? new Vector2(math.abs(transform.position.x), transform.position.y) : new Vector2(-transform.position.x, transform.position.y);
            _renderer.flipX = newDirection == Direction.Left;
        }
    
        private void OnDestroy()
        {
            GameController.DirectionChanged -= OnDirectionChanged;
        
            PowerUpBehaviour.PowerUpAdded -= OnPowerUpAdded;
            PowerUpBehaviour.PowerUpRemoved -= SetToDefault;
        }

        private void OnPowerUpAdded(PowerUp powerUpData, VisualElement UiElement)
        {
            _renderer.sprite = Resources.Load<Sprite>(ResourcePaths.PlayerSprites + powerUpData.Name);
            _animator.enabled = true;
        }
    
        private void SetToDefault()
        {
            var fileName = "Default";
        
            _renderer.sprite = Resources.Load<Sprite>(ResourcePaths.PlayerSprites + fileName);
            _audio.clip = Resources.Load<AudioClip>(ResourcePaths.PlayerAudio + fileName);
        
            _animator.enabled = false;
        }
    }
}