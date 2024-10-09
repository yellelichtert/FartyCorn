
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PowerUps
{
    public class JetPackPowerUp : PowerUpBase
    {
        private float _duration;
        private float _jetpackPower;
        
        private bool _holding;
        private Rigidbody2D _rb;
        private AudioSource _audio;
        private InputAction _moveAction;
        private GameObject _thrustPrefab;
        private ProgressBar _progressBar;

        private void Start()
        {
            _jetpackPower = 20;
            ResetDuration();

            _audio = gameObject.AddComponent<AudioSource>();
            _audio.clip = Resources.Load<AudioClip>("Audio/LongFart");
            
            _thrustPrefab = Resources.Load<GameObject>("Prefabs/Thrust");
            
            _rb = GetComponent<Rigidbody2D>();


            _progressBar = new ProgressBar()
            {
                title = "Fuel",
                lowValue = 0f,
                highValue = _duration,
                value = _duration
            };
            _progressBar.visible = true;
            
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
                _rb.AddForce(Vector2.up * _jetpackPower, ForceMode2D.Force);
                _duration--;
                StartCoroutine(Thrust());
                
                if (!_audio.isPlaying) _audio.Play();
                if (_duration <= 0) Destroy(this);
            }
            else if (_audio.isPlaying)
            {
                _audio.Stop();
            }
        }
        
        
        public void ResetDuration() => _duration = 1000;
        
        
        //Expensive and copy of already existing methods
        //Needs to be removed when refacoring!
        private IEnumerator Thrust()
        {
            var thrust = Instantiate(_thrustPrefab, new Vector2(transform.position.x+0.1f, transform.position.y-0.5f), Quaternion.identity);
        
            var animationLength = thrust.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        
            yield return new WaitForSeconds(animationLength);
            Destroy(thrust.gameObject);
        }
    }
}
