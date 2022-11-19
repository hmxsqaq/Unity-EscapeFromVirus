using System;
using Framework;
using UnityEngine;

namespace UI
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioSource _audioSource;
        public AudioClip[] audioClips;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            AudioPlay(0,true);
            EventManager.Instance.AddEventListener(EventNameHelper.AudioPause,AudioPause);
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