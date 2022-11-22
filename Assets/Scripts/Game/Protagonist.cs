using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace Game
{
    public class Protagonist : MonoBehaviour
    {
        private MyInputAction _myInputAction;
        private Vector2 _inputPosition;
        private bool _pressed;
        private bool _attack;
        private bool _invincible;
        private bool _isReady;
        private bool _normalMasked;
        private bool _n95Masked;
        private Coroutine _attackMode;
        private Coroutine _invincibleMode;
        private Coroutine _maskedMode;

        [Header("Control")] 
        public GameObject bigRect;
        public GameObject smallRect;
        
        [Header("Protagonist")]
        public float operationRange;
        public float playerSpeed;
        public int initLife;

        [Header("Ability Lasting Time")] 
        public float attackModeTime;
        public float invincibleModeTime;
        public float normalMaskTime;
        public float n95MaskTime;

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
            GameModel.Instance.Life = initLife;
            
            _myInputAction.Game.Position.performed += context =>
            {
                if (_pressed && Camera.main != null)
                {
                    _inputPosition = Camera.main.ScreenToWorldPoint((Vector3)context.ReadValue<Vector2>());
                    if (Vector3.Distance(_inputPosition,transform.position) < operationRange)
                    {
                        transform.Translate(((Vector3)_inputPosition - transform.position) * playerSpeed * Time.deltaTime);
                    }
                }
            };
            _myInputAction.Game.Act.performed += context =>
            {
                if (!_isReady)
                {
                    EventManager.Instance.Trigger(EventNameHelper.GameReady);
                    _isReady = true;
                }
                _pressed = context.ReadValueAsButton();
            };
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            int hit = col.GetComponent<Element>().damage;
            
            if (!_invincible && col.CompareTag("Virus"))
            {
                if (_n95Masked)
                {
                    hit -= 2;
                }
                else if (_normalMasked)
                {
                    hit -= 1;
                }
                Hited(hit);
            }

            if (col.CompareTag("Prop"))
            {
                Hited(hit);
            }
            
            if (_attack && col.CompareTag("Virus"))
                GameModel.Instance.Score += 20;

            if (col.CompareTag("Alcohol"))
            {
                if (_attackMode != null)
                {
                    StopCoroutine(_attackMode);
                }
                _attackMode = StartCoroutine(AttackMode());
            }
            
            if (col.CompareTag("ProtectiveSuit"))
            {
                if (_invincibleMode != null)
                {
                    StopCoroutine(_invincibleMode);
                }
                _invincibleMode = StartCoroutine(InvincibleMode());
            }

            if (col.CompareTag("MaskNormal") || col.CompareTag("MaskN95"))
            {
                if (_maskedMode != null)
                {
                    StopCoroutine(_maskedMode);
                }
                _maskedMode = col.CompareTag("MaskNormal")
                    ? StartCoroutine(Masked(true))
                    : StartCoroutine(Masked(false));
            }

            if (col.CompareTag("QA"))
                EventManager.Instance.Trigger(EventNameHelper.StartAnswer);

            Destroy(col.gameObject);
        }

        private void Hited(int damage)
        {
            if (GameModel.Instance.Life - damage <= 0)
            {
                GameModel.Instance.Life = 0;
                EventManager.Instance.Trigger(EventNameHelper.GameOver);
            }
            else if (GameModel.Instance.Life - damage >= initLife)
            {
                GameModel.Instance.Life = initLife;
            }
            else
            {
                GameModel.Instance.Life -= damage;
            }
        }

        IEnumerator AttackMode()
        {
            _attack = true;
            yield return new WaitForSeconds(attackModeTime);
            _attack = false;
            StopCoroutine(_attackMode);
            _attackMode = null;
        }
        
        IEnumerator InvincibleMode()
        {
            _invincible = true;
            yield return new WaitForSeconds(invincibleModeTime);
            _invincible = false;
            StopCoroutine(_invincibleMode);
            _invincibleMode = null;
        }

        IEnumerator Masked(bool normal)
        {
            
            if (normal)
            {
                _normalMasked = true;
                _n95Masked = false;
                yield return new WaitForSeconds(normalMaskTime);
            }
            else
            {
                _normalMasked = false;
                _n95Masked = true;
                yield return new WaitForSeconds(n95MaskTime);
            }
            _normalMasked = false;
            _n95Masked = false;
            
            StopCoroutine(_maskedMode);
            _maskedMode = null;
        }
    }
}