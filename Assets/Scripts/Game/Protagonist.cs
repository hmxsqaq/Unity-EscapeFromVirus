using System.Collections;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Protagonist : MonoBehaviour
    {
        private MyInputAction _myInputAction;
        private Vector3 _inputPosition;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _direction;
        
        private bool _pressed;
        private bool _isReady;// need to show the ready panel
        
        private bool _attack;// attack state mark
        private bool _invincible;// invincible state mark
        private bool _normalMasked;// normalmask state mark
        private bool _n95Masked;// n95mask state mark
        private bool _slow;
        private bool _hited;

        // record the Coroutine ID to stop the corresponding Coroutine
        private Coroutine _attackMode;
        private Coroutine _invincibleMode;
        private Coroutine _maskedMode;
        private Coroutine _lowSpeedMode;
        
        // display states
        private SpriteRenderer _maskNormal;
        private SpriteRenderer _maskN95;
        private SpriteRenderer _protected;
        private SpriteRenderer _spriteRenderer;

        // the virtual rocker
        private Transform _bigRect;
        private Transform _smallRect;
        
        private Camera _camera;
        private const float XMax = 2.3f;
        private const float YMax = 3.76f;
        private const float XMin = -2.3f;
        private const float YMin = -5f;

        [Header("Protagonist")]
        public float playerSpeed;
        public int initLife;

        [Header("Ability Lasting Time")] 
        public float attackModeTime;
        public float invincibleModeTime;
        public float normalMaskTime;
        public float n95MaskTime;
        public float lowSpeedTime;

        [Header("Anima")] public float hitFadeTime;
        
        
        private void Awake()
        {
            _myInputAction = new MyInputAction();
            GameModel.Instance.Life = initLife;
            GameModel.Instance.Minutes = 0;
            GameModel.Instance.Seconds = 0;
        }

        private void OnEnable()
        {
            _myInputAction.Enable();
        }

        private void OnDisable()
        {
            _myInputAction.Disable();
            StopAllCoroutines();
        }

        private void Start()
        {
            _camera = Camera.main;
            _bigRect = GameObject.Find("BigCircle").transform;
            _smallRect = _bigRect.GetChild(0);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _maskNormal = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _maskN95 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            _protected = transform.GetChild(2).GetComponent<SpriteRenderer>();

            _myInputAction.Game.Act.performed += context =>
            {
                // “press to start the game”
                if (!_isReady)
                {
                    EventManager.Instance.Trigger(EventNameHelper.GameReady);
                    _isReady = true;
                }
                else
                {
                    // _pressed = context.ReadValueAsButton();

                    // _inputPosition = Vector3.zero; // init the inputPosition
                    //
                    // // show the virtual rocker where you click
                    // Vector3 point = _bigRect.transform.position;
                    // if (Camera.main != null)
                    // {
                    //     if (Mouse.current != null)
                    //     {
                    //         point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    //     }
                    //
                    //     if (Touchscreen.current != null)
                    //     {
                    //         point = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
                    //     }
                    //
                    //     point.z = 0;
                    //     _bigRect.transform.position = point;
                    // }
                    //
                    // _bigRect.GetComponent<Renderer>().enabled = _pressed;
                    // _smallRect.GetComponent<Renderer>().enabled = _pressed;

                    if (Mouse.current != null)
                    {
                        if (_camera != null)
                            _inputPosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    }
                    if (Touchscreen.current != null)
                    {
                        if (_camera != null)
                            _inputPosition = _camera.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
                    }
                    _inputPosition.z = 0;
                    if (context.ReadValueAsButton())
                    {
                        _startPosition = _inputPosition;
                    }
                    else if (_startPosition != null)
                    {
                        _endPosition = _inputPosition;
                        _direction = _endPosition - _startPosition;
                    }
                }
            };
        }

        private void FixedUpdate()
        {
            // // protagonist move
            // if (_pressed && _camera != null)
            // {
            //     if (Mouse.current != null)
            //     {
            //         _inputPosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //     }
            //     if (Touchscreen.current != null)
            //     {
            //         _inputPosition = _camera.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
            //     }
            //     _inputPosition.z = 0;
            //     _inputPosition -= _bigRect.transform.position;
            //     transform.Translate(_inputPosition.normalized * playerSpeed);// Move
            //     
            //     // small virtual rocker position 
            //     if (_inputPosition.magnitude >= 1)
            //         _inputPosition = _inputPosition.normalized;
            //     _smallRect.transform.localPosition = (Vector3)_inputPosition * 0.33f;
            //     
            //     // protagonist position rect limit
            //     transform.position = new Vector3(Mathf.Clamp(transform.position.x, XMin, XMax),
            //         Mathf.Clamp(transform.position.y, YMin, YMax), 0);
            // }

            
            transform.Translate(_direction * playerSpeed);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, XMin, XMax),
                Mathf.Clamp(transform.position.y, YMin, YMax), 0);
            
        }

        private void Update()
        {
            if (_hited)
            {
                _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.red, Time.deltaTime * hitFadeTime);
                if (_spriteRenderer.color.g < 0.05)
                {
                    if (_slow)
                    {
                        _spriteRenderer.color = Color.cyan;
                    }
                    else
                    {
                        _spriteRenderer.color = Color.white;
                    }
                    _hited = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            int hit = col.GetComponent<Element>().damage;
            
            if (!_invincible && col.CompareTag("Virus"))
            {
                _hited = true;
                if (_n95Masked)
                {
                    hit -= 2;
                }
                else if (_normalMasked)
                {
                    hit -= 1;
                }
                else
                {
                    if (_lowSpeedMode != null) StopCoroutine(_lowSpeedMode);
                    _lowSpeedMode = StartCoroutine(LowSpeed());
                }
                Hited(hit);
                EventManager.Instance.Trigger(EventNameHelper.AudioVirusAttack);
            }

            if (col.CompareTag("Prop"))
            {
                Hited(hit);
                EventManager.Instance.Trigger(EventNameHelper.AudioGetProps);
            }
            
            if (_attack && col.CompareTag("Virus"))
            {
                EventManager.Instance.Trigger(EventNameHelper.ShowScoreNumber);
                GameModel.Instance.Score += 20;
            }

            if (col.CompareTag("Alcohol"))
            {
                if (_attackMode != null) StopCoroutine(_attackMode);
                _attackMode = StartCoroutine(AttackMode());
                EventManager.Instance.Trigger(EventNameHelper.AudioGetProps);
            }
            
            if (col.CompareTag("ProtectiveSuit"))
            {
                if (_invincibleMode != null) StopCoroutine(_invincibleMode);
                _invincibleMode = StartCoroutine(InvincibleMode());
                EventManager.Instance.Trigger(EventNameHelper.AudioGetProps);
            }

            if (col.CompareTag("MaskNormal") || col.CompareTag("MaskN95"))
            {
                if (_maskedMode != null) StopCoroutine(_maskedMode);
                _maskedMode = col.CompareTag("MaskNormal")
                    ? StartCoroutine(Masked(true))
                    : StartCoroutine(Masked(false));
                EventManager.Instance.Trigger(EventNameHelper.AudioGetProps);
            }

            if (col.CompareTag("QA"))
            {
                EventManager.Instance.Trigger(EventNameHelper.StartAnswer);
                EventManager.Instance.Trigger(EventNameHelper.AudioGetProps);
            }

            Destroy(col.gameObject);
        }

        private void Hited(int damage)
        {
            if (GameModel.Instance.Life - damage <= 0)// if you die
            {
                GameModel.Instance.CurrentDamage = GameModel.Instance.Life;
                EventManager.Instance.Trigger(EventNameHelper.ShowHitNumber);
                GameModel.Instance.Life = 0;
                EventManager.Instance.Trigger(EventNameHelper.GameOver);
            }
            else if (GameModel.Instance.Life - damage >= initLife)// if you are healed when heal + life >= 10
            {
                GameModel.Instance.CurrentDamage = initLife - GameModel.Instance.Life;
                GameModel.Instance.Life = initLife;
                EventManager.Instance.Trigger(EventNameHelper.ShowHealNumber);
            }
            else
            {
                if (damage < 0)// normal heal
                {
                    GameModel.Instance.CurrentDamage = -damage;
                    EventManager.Instance.Trigger(EventNameHelper.ShowHealNumber);
                }
                if (damage > 0)// noraml hit
                {
                    GameModel.Instance.CurrentDamage = damage;
                    EventManager.Instance.Trigger(EventNameHelper.ShowHitNumber);
                }
                GameModel.Instance.Life -= damage;
            }
        }

        IEnumerator AttackMode()
        {
            _attack = true;
            EventManager.Instance.Trigger(EventNameHelper.AttackModeSwitch);
            yield return new WaitForSeconds(attackModeTime);
            _attack = false;
            EventManager.Instance.Trigger(EventNameHelper.NormalModeSwitch);
            StopCoroutine(_attackMode);
            _attackMode = null;
        }
        
        IEnumerator InvincibleMode()
        {
            _invincible = true;
            _protected.enabled = true;
            yield return new WaitForSeconds(invincibleModeTime);
            _invincible = false;
            _protected.enabled = false;
            StopCoroutine(_invincibleMode);
            _invincibleMode = null;
        }

        IEnumerator Masked(bool normal)
        {
            
            if (normal)
            {
                _normalMasked = true;
                _n95Masked = false;
                _maskNormal.enabled = true;
                _maskN95.enabled = false;
                yield return new WaitForSeconds(normalMaskTime);
            }
            else
            {
                _normalMasked = false;
                _n95Masked = true;
                _maskNormal.enabled = false;
                _maskN95.enabled = true;
                yield return new WaitForSeconds(n95MaskTime);
            }
            _normalMasked = false;
            _n95Masked = false;
            _maskNormal.enabled = false;
            _maskN95.enabled = false;
            StopCoroutine(_maskedMode);
            _maskedMode = null;
        }

        IEnumerator LowSpeed()
        {
            playerSpeed = (float)(playerSpeed * 0.8);
            _spriteRenderer.color = Color.cyan;
            _slow = true;
            yield return new WaitForSeconds(lowSpeedTime);
            _slow = false;
            _spriteRenderer.color = Color.white;
            playerSpeed = (float)(playerSpeed * 1.25);
        }
    }
}