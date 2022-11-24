using System;
using Framework;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GoalShow : MonoBehaviour
    {
        public AnimationClip[] animationClips;

        private Animation _animation;
        public TextMeshProUGUI damge;
        public TextMeshProUGUI heal;

        private void Start()
        {
            _animation = GetComponent<Animation>();
            EventManager.Instance.AddEventListener(EventNameHelper.ShowHitedNumber,Hited);
            EventManager.Instance.AddEventListener(EventNameHelper.ShowScoreNumber,Scored);
            EventManager.Instance.AddEventListener(EventNameHelper.ShowHealNumber,Healing);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener(EventNameHelper.ShowHitedNumber,Hited);
            EventManager.Instance.RemoveEventListener(EventNameHelper.ShowScoreNumber,Scored);
            EventManager.Instance.RemoveEventListener(EventNameHelper.ShowHealNumber,Healing);
        }

        private void Hited()
        {
            damge.text = $"-{GameModel.Instance.CurrentDamage}";
            _animation.clip = animationClips[0];
            _animation.Play();
        }

        private void Healing()
        {
            int heal = GameModel.Instance.CurrentDamage;
            this.heal.text = $"+{heal}";
            _animation.clip = animationClips[2];
            _animation.Play();
        }

        private void Scored()
        {
            _animation.clip = animationClips[1];
            _animation.Play();
        }
    }
}