using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AudioButton : MonoBehaviour
    {
        private Button _btn;

        private void Start()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener((() => 
                    EventManager.Instance.Trigger(EventNameHelper.AudioPause)));
        }
    }
}