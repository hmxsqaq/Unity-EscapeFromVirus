using Framework;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class QAManager : MonoBehaviour
    {
        public TextMeshProUGUI question;
        public TextMeshProUGUI optionA;
        public TextMeshProUGUI optionB;
        
        private string _correctAnswer;

        private void OnEnable()
        {
            int questionNumber = Random.Range(0,GameModel.Instance.DataRoot.TestData.Length);
            question.text = GameModel.Instance.DataRoot.TestData[questionNumber].Question;
            optionA.text = GameModel.Instance.DataRoot.TestData[questionNumber].OptionA;
            optionB.text = GameModel.Instance.DataRoot.TestData[questionNumber].OptionB;
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
            Debug.Log("Correct");
            EventManager.Instance.Trigger(EventNameHelper.EndAnswer);
        }

        private void AnswerWrong()
        {
            Debug.Log("Wrong");
            EventManager.Instance.Trigger(EventNameHelper.EndAnswer);
        }
    }
}