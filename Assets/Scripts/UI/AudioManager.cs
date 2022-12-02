using System;
using Framework;
using UnityEngine;

namespace UI
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioSource _audioSource;
        public AudioClip[] audioClips;

        private void OnEnable()
        {
            EventManager.Instance.AddEventListener(EventNameHelper.AudioPause,AudioPause);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener(EventNameHelper.AudioPause,AudioPause);
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            AudioPlay(0,true);
        }

        private void AudioPlay(int audioIndex,bool isloop)
        {
            _audioSource.clip = audioClips[audioIndex];
            _audioSource.loop = isloop;
            _audioSource.Play();
        }

        private void AudioPause()
        {
            if (_audioSource.pitch == 0)
            {
                _audioSource.pitch = 1;
            }
            else
            {
                _audioSource.pitch = 0;
            }
        }
    }
}