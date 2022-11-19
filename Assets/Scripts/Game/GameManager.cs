using System;
using Framework;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI lifeTMP;
        public TextMeshProUGUI scoreTMP;
        public GameObject pausePanel;
        public GameObject qAPanel;
        public GameObject readyStart;

        private void Awake()
        {
            EventManager.Instance.AddEventListener(EventNameHelper.OnLifeChange,LifeUpdate);
            EventManager.Instance.AddEventListener(EventNameHelper.OnScoreChange,ScoreUpdate);
            
            EventManager.Instance.AddEventListener(EventNameHelper.GamePause,GamePause);
            EventManager.Instance.AddEventListener(EventNameHelper.GameOver,GameOver);
            EventManager.Instance.AddEventListener(EventNameHelper.GameReady,GamePause);
            EventManager.Instance.AddEventListener(EventNameHelper.GameReady,GameReady);

            EventManager.Instance.AddEventListener(EventNameHelper.StartAnswer,StartAnswer);
            EventManager.Instance.AddEventListener(EventNameHelper.EndAnswer,EndAnswer);
        }

        private void Start()
        {
            EventManager.Instance.Trigger(EventNameHelper.GameReady);
        }

        private void LifeUpdate()
        {
            lifeTMP.text = $"生命:{GameModel.Instance.Life}";
        }

        private void ScoreUpdate()
        {
            scoreTMP.text = $"金钱:{GameModel.Instance.Score}";
        }

        private void GamePause()
        {
            if (Time.timeScale >= 1)
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
            }
        }

        private void GameOver()
        {
            Time.timeScale = 0;
        }

        private void GameReady()
        {
            if (readyStart.activeSelf)
            {
                readyStart.SetActive(false);
            }
            else
            {
                readyStart.SetActive(true);
            }
        }

        private void StartAnswer()
        {
            qAPanel.SetActive(true);
            EventManager.Instance.Trigger(EventNameHelper.GamePause);
        }

        private void EndAnswer()
        {
            qAPanel.SetActive(false);
            EventManager.Instance.Trigger(EventNameHelper.GamePause);
        }
    }
}