using System;
using UnityEngine;

namespace UI
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        private AudioSource _audioSource;
        public AudioClip[] audioClips;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void AudioPlay(int audioIndex,bool isloop)
        {
            _audioSource.clip = audioClips[audioIndex];
            _audioSource.loop = isloop;
            _audioSource.Play();
        }

        public void AudioPause()
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