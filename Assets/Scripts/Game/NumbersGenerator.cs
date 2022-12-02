using System;
using Framework;
using UnityEngine;

namespace Game
{
    public class NumbersGenerator : MonoBehaviour
    {
        public GameObject[] hitList;
        public GameObject[] healList;
        public GameObject[] scoreList;

        private Transform _protagonist;
        private GameObject _current;

        private void OnEnable()
        {
            EventManager.Instance.AddEventListener(EventNameHelper.ShowHitNumber,HitGenerate);
            EventManager.Instance.AddEventListener(EventNameHelper.ShowHealNumber,HealGenerate);
            EventManager.Instance.AddEventListener(EventNameHelper.ShowScoreNumber,ScoreGenerate);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener(EventNameHelper.ShowHitNumber,HitGenerate);
            EventManager.Instance.RemoveEventListener(EventNameHelper.ShowHealNumber,HealGenerate);
            EventManager.Instance.RemoveEventListener(EventNameHelper.ShowScoreNumber,ScoreGenerate);
        }

        private void Start()
        {
            _protagonist = GameObject.FindWithTag("Protagonist").transform;
        }

        private void HitGenerate()
        {
            if (GameModel.Instance.CurrentDamage == 1)
            {
                _current = hitList[0];
            }
            if (GameModel.Instance.CurrentDamage == 2)
            {
                _current = hitList[1];
            }
            if (GameModel.Instance.CurrentDamage == 3)
            {
                _current = hitList[2];
            }
            if (GameModel.Instance.CurrentDamage == 4)
            {
                _current = hitList[3];
            }
            Instantiate(_current, _protagonist.position + Vector3.up, Quaternion.identity, transform);
        }
        
        private void HealGenerate()
        {
            if (GameModel.Instance.CurrentDamage == 0)
            {
                _current = healList[0];
            }
            if (GameModel.Instance.CurrentDamage == 1)
            {
                _current = healList[1];
            }
            if (GameModel.Instance.CurrentDamage == 2)
            {
                _current = healList[2];
            }
            if (GameModel.Instance.CurrentDamage == 3)
            {
                _current = healList[3];
            }
            Instantiate(_current, _protagonist.position + Vector3.up, Quaternion.identity, transform);
        }
        
        private void ScoreGenerate()
        {
            _current = scoreList[0];
            Instantiate(_current, _protagonist.position + 2*Vector3.up, Quaternion.identity, transform);
        }
    }
}