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

        public float operationRange;
        public float playerSpeed;
        public int initLife;

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
                _pressed = context.ReadValueAsButton();
                if (!_isReady)
                {
                    EventManager.Instance.Trigger(EventNameHelper.GameReady);
                    _isReady = true;
                }
            };
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Exchange"))
            {
                if (!_invincible || col.CompareTag("Prop"))
                    Hited(col.GetComponent<Element>().damage);

                if (_attack && col.CompareTag("Virus"))
                    GameModel.Instance.Score += 2;

                if (col.CompareTag("Alcohol"))
                    StartCoroutine(AttackMode());

                if (col.CompareTag("ProtectiveSuit"))
                    StartCoroutine(InvincibleMode());

                if (col.CompareTag("QA"))
                    EventManager.Instance.Trigger(EventNameHelper.StartAnswer);

                Destroy(col.gameObject);
            }
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
            yield return new WaitForSeconds(5f);
            _attack = false;
            StopCoroutine(AttackMode());
        }
        
        IEnumerator InvincibleMode()
        {
            _invincible = true;
            yield return new WaitForSeconds(5f);
            _invincible = false;
            StopCoroutine(InvincibleMode());
        }
    }
}