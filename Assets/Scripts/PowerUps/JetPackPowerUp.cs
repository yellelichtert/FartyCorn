
using System;
using System.Collections;
using UIElements;
using UnityEngine;

namespace PowerUps
{
    public class JetPackPowerUp : PowerUpBase
    {
        private readonly float _powerUpDuration = 150;
        private readonly float _jetpackPower = 20;



        private Rigidbody2D _rb;
        private AudioSource _audio;
        private FuelBar _fuelBar;
        private Animator _animator;
        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        
        
        private UIController _ui;
        private PlayerController _player;

        private float _fuel;
        private bool _holding;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _audio = gameObject.AddComponent<AudioSource>();
            _audio.clip = Resources.Load<AudioClip>("Audio/LongFart");

            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _fuelBar = new FuelBar(_powerUpDuration);

            _animator = GetComponent<Animator>();
            
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.clip = Resources.Load<AudioClip>("Audio/Jetpack");
            _audioSource.loop = true;

            _ui = UIController.Instance;
        }


        private void Start()
        {
            _fuel = _powerUpDuration;
            _fuelBar.CurrentFuel = _fuel;
            
            _ui.AddPowerUpElement(_fuelBar);
            
            AddPowerUp();
        }
        

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
                {
                    _holding = true;
                }
                else if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended)
                {
                    _holding = false;
                }
            }
        }


        private void FixedUpdate()
        {
            if (_holding)
            {
                if (!_animator.enabled)
                {
                    _audioSource.Play();
                    _animator.enabled = true;
                    _animator.SetBool("Holding", _holding);
                }


                _rb.AddForce(Vector2.up * _jetpackPower, ForceMode2D.Force);
                _fuel--;
                _fuelBar.CurrentFuel = _fuel;


                if (!_audio.isPlaying) _audio.Play();
                if (_fuel <= 0)
                {
                    _ui.RemovePowerUpElement();
                    //_player.RemovePowerUp();

                    Destroy(_audioSource);
                    RemovePowerUp();
                }
            }
            else if (!_holding)
            {
                _audioSource.Stop();
                _animator.enabled = false;
                _animator.SetBool("Holding", _holding);
                _audio.Stop();
            }
        }
        
        
        
        public override void ResetPowerUp() => _fuel = _powerUpDuration;
        
        private void AddPowerUp()
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/Jetpack");
        }
        
        private void RemovePowerUp()
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/Default");
            _animator.enabled = false;
            
            Destroy(this);
        }
    }
}
