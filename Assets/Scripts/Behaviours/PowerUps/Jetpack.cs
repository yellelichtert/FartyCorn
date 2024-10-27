using System;
using System.Linq;
using Managers;
using UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Behaviours.PowerUps
{
    public class Jetpack : PowerUpBehaviour
    {
        protected override VisualElement UiElement { get; set; }
        
        private Rigidbody2D _rb;
        private AudioSource _audio;
        private Animator _animator;
        
        
        private float _fuel;
        private bool _holding;
        

        private void Awake() => PowerUp =  PowerUpManager.PowerUps.FirstOrDefault(p => p.Name == nameof(Jetpack));

        
        
        private new void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _audio = gameObject.GetComponent<AudioSource>();
            _audio.clip = Resources.Load<AudioClip>(ResourcePaths.PlayerAudio + PowerUp.Name);

            ResetDuration();
            UiElement = new FuelBar(_fuel)
            {
                CurrentFuel = _fuel
            };
            
            base.Start();
        }

        
        
        
        private void Update()
        {
            if (Input.touchCount <= 0) return;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _holding = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _holding = false;
            }
        }

        
        

        private void FixedUpdate()
        {
            switch (_holding)
            {
                case true:
                {
                    if (!_animator.enabled)
                    {
                        _audio.Play();
                        _animator.SetBool(AnimatorStates.Holding, _holding);
                    }


                    _rb.AddForce(Vector2.up * PowerUp.Power , ForceMode2D.Force);
                    _fuel--;
                    ((FuelBar)UiElement).CurrentFuel = _fuel;


                    if (!_audio.isPlaying) _audio.Play();
                    if (_fuel <= 0)
                    {
                    
                        RemovePowerUp();
                    }

                    break;
                }
                case false:
                    _audio.Stop();
                    _animator.SetBool(AnimatorStates.Holding, _holding);
                    break;
            }
        }
        
        
        
        public override void ResetDuration() => _fuel = PowerUp.Duration + PowerUp.Duration * PowerUp.CurrentLevel;
    }
}
