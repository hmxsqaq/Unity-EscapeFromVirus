using System;
using System.Collections;
using Framework;
using TMPro;
using UnityEngine;
using UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("TMP Object")]
        public TextMeshProUGUI lifeTMP;
        public TextMeshProUGUI scoreTMP;
        
        [Header("Effect UI")]
        public GameObject pausePanel;
        public GameObject qAPanel;
        public GameObject readyStart;
        public GameObject end;
        public GameObject sceneManager;

        [Header("Background and Protagonist")]
        public SpriteRenderer background;
        public Sprite[] backgrounds;
        public SpriteRenderer protagonist;
        public Sprite[] nomalProtagonist;

        private void Awake()
        {
            EventManager.Instance.AddEventListener(EventNameHelper.OnLifeChange,LifeUpdate);
            EventManager.Instance.AddEventListener(EventNameHelper.OnScoreChange,ScoreUpdate);
            EventManager.Instance.AddEventListener(EventNameHelper.GamePause,GamePause);
            EventManager.Instance.AddEventListener(EventNameHelper.GameOver,GamePause);
            EventManager.Instance.AddEventListener(EventNameHelper.GameOver,GameOver);
            EventManager.Instance.AddEventListener(EventNameHelper.GameReady,GamePause);
            EventManager.Instance.AddEventListener(EventNameHelper.GameReady,GameReady);
            EventManager.Instance.AddEventListener(EventNameHelper.StartAnswer,StartAnswer);
            EventManager.Instance.AddEventListener(EventNameHelper.EndAnswer,EndAnswer);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            Time.timeScale = 1;
            GameModel.Instance.Seconds -= 1; 
            EventManager.Instance.RemoveEventListener(EventNameHelper.OnLifeChange,LifeUpdate);
            EventManager.Instance.RemoveEventListener(EventNameHelper.OnScoreChange,ScoreUpdate);
            EventManager.Instance.RemoveEventListener(EventNameHelper.GamePause,GamePause);
            EventManager.Instance.RemoveEventListener(EventNameHelper.GameOver,GamePause);
            EventManager.Instance.RemoveEventListener(EventNameHelper.GameOver,GameOver);
            EventManager.Instance.RemoveEventListener(EventNameHelper.GameReady,GamePause);
            EventManager.Instance.RemoveEventListener(EventNameHelper.GameReady,GameReady);
            EventManager.Instance.RemoveEventListener(EventNameHelper.StartAnswer,StartAnswer);
            EventManager.Instance.RemoveEventListener(EventNameHelper.EndAnswer,EndAnswer);
        }

        private void Start()
        {
            EventManager.Instance.Trigger(EventNameHelper.GameReady);
            RandomBackground();
        }

        private void LifeUpdate()
        {
            lifeTMP.text = $"{GameModel.Instance.Life}";
        }

        private void ScoreUpdate()
        {
            scoreTMP.text = $"{GameModel.Instance.Score}";
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
            end.SetActive(true);
            StartCoroutine(ToEnd());
        }

        IEnumerator ToEnd()
        {
            yield return new WaitForSecondsRealtime(1.5f);
            sceneManager.GetComponent<SceneManager>().SceneSwitch("4End");
            StopCoroutine(ToEnd());
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

        private void RandomBackground()
        {
            int index = Random.Range(0, backgrounds.Length);
            GameModel.Instance.Index = index;
            background.sprite = backgrounds[index];
            index = Random.Range(0, nomalProtagonist.Length);
            protagonist.sprite = nomalProtagonist[index];
        }
    }
}