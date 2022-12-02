using System;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class QAIntroducation : MonoBehaviour
    {
        private TextMeshProUGUI _introducationText;
        private void OnEnable()
        {
            _introducationText = GetComponentInChildren<TextMeshProUGUI>();
            _introducationText.text =
                GameModel.Instance.DataRoot.TestData[GameModel.Instance.CurrentQuestionIndex].Introducation;
        }
    }
}