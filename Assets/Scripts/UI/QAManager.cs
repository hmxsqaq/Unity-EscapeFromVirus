using System;
using Framework;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace UI
{
    public class QAManager : MonoBehaviour
    {
        public TextMeshProUGUI question;
        public GameObject introducation;
        private string _correctAnswer;
        private bool _introducationStart;

        private void OnEnable()
        {
            int questionNumber = Random.Range(0,GameModel.Instance.DataRoot.TestData.Length);
            GameModel.Instance.CurrentQuestionIndex = questionNumber;
            
            question.text = GameModel.Instance.DataRoot.TestData[questionNumber].Question;
            
            if (GameModel.Instance.DataRoot.TestData[questionNumber].CorrectAnswer)
            {
                _correctAnswer = "A";
            }
            else
            {
                _correctAnswer = "B";
            }
        }

        public void AnswerSelect(string ansewer)
        {
            if (ansewer == _correctAnswer)
            {
                AnswerCorrect();
            }
            else
            {
                AnswerWrong();
            }
        }
        
        private void AnswerCorrect()
        {
            EventManager.Instance.Trigger(EventNameHelper.EndAnswer);
        }

        private void AnswerWrong()
        {
            introducation.SetActive(true);
            _introducationStart = true;
        }

        private void Update()
        {
            if (_introducationStart)
            {
                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    _introducationStart = false;
                    EventManager.Instance.Trigger(EventNameHelper.EndAnswer);
                    introducation.SetActive(false);
                }
                if (Touchscreen.current != null && Touchscreen.current.press.wasPressedThisFrame)
                {
                    _introducationStart = false;
                    EventManager.Instance.Trigger(EventNameHelper.EndAnswer);
                    introducation.SetActive(false);
                }
            }
        }
    }
}