using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class TrailManager : MonoBehaviour
    {
        private MyInputAction _myInputAction;
        private Vector3 _inputPosition;
        private TrailRenderer _trailRenderer;

        private void Awake()
        {
            _myInputAction = new MyInputAction();
            _trailRenderer = GetComponent<TrailRenderer>();
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
            _myInputAction.Game.Act.performed += context =>
            {
                _trailRenderer.emitting = !context.ReadValueAsButton();
                if (Mouse.current != null)
                {
                    _inputPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                }
                if (Touchscreen.current != null)
                {
                    _inputPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
                }
                _inputPosition.z = 0;
                transform.position = _inputPosition;
            };
        }
    }
}