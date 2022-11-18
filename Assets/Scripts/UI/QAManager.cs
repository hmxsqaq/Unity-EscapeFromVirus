using System.Threading;
using Framework;
using UnityEngine;

namespace UI
{
    public class QAManager : MonoBehaviour
    {
        public string correctAnswer;
        
        public void AnswerSelect(string ansewer)
        {
            if (ansewer == correctAnswer)
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