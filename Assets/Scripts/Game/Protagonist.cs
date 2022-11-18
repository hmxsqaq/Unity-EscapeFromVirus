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

        public float operationRange;
        public float playerSpeed;
        
        private void Awake()
        {
            _myInputAction = new MyInputAction();
        }

        private void OnEnable()
        {
            _myInputAction.Enable();
        }

        private void OnDisable()
        {
            _myInputAction.Disable();
        }

        private void Start()
        {
            _myInputAction.Game.Position.performed += context =>
            {
                if (_pressed && Camera.main != null)
                {
                    _inputPosition = Camera.main.ScreenToWorldPoint((Vector3)context.ReadValue<Vector2>());
                    Debug.Log(Vector3.Distance(_inputPosition,transform.position));
                    if (Vector3.Distance(_inputPosition,transform.position) < operationRange)
                    {
                        transform.Translate(((Vector3)_inputPosition - transform.position) * playerSpeed / 100);
                    }
                }
            };
            _myInputAction.Game.Act.performed += context =>
            {
                _pressed = context.ReadValueAsButton();
            };
        }

        private void OnTriggerEnter2D(Collider2D col)
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

        private void Hited(int damage)
        {
            GameModel.Instance.Life -= damage;
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