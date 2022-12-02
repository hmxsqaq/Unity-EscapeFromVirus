using System;
using Framework;
using UnityEngine;

namespace UI
{
    public class ColliderAudioEffect : MonoBehaviour
    {
        public AudioClip[] audioClips;

        private AudioSource _audioSource;

        private void OnEnable()
        {
            EventManager.Instance.AddEventListener(EventNameHelper.AudioGetProps,GetProps);
            EventManager.Instance.AddEventListener(EventNameHelper.AudioVirusAttack,VirusAttack);
            EventManager.Instance.AddEventListener(EventNameHelper.GameOver,GameFailureAudio);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener(EventNameHelper.AudioGetProps,GetProps);
            EventManager.Instance.RemoveEventListener(EventNameHelper.AudioVirusAttack,VirusAttack);
            EventManager.Instance.RemoveEventListener(EventNameHelper.GameOver,GameFailureAudio);
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void GetProps()
        {
            _audioSource.clip = audioClips[0];
            _audioSource.Play();
        }
        
        private void VirusAttack()
        {
            _audioSource.clip = audioClips[1];
            _audioSource.Play();
        }
        
        private void GameFailureAudio()
        {
            _audioSource.clip = audioClips[2];
            _audioSource.Play();
        }
    }
}