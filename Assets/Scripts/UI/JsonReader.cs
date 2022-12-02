using System;
using Game;
using UnityEngine;

namespace UI
{
    public class JsonReader : MonoBehaviour
    {
        private DataRoot _data;
        private void Awake()
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSON/Test");
            if(jsonTextAsset != null)
            {
                _data = JsonUtility.FromJson<DataRoot>(jsonTextAsset.text);
            }
            else
            {
                Debug.Log("Cont find jsonfile");
            }
            GameModel.Instance.DataRoot = _data;
        }
    }

    [Serializable]
    public class Test
    {
        public string Question;
        public bool CorrectAnswer;
        public string Introducation;
    }

    [Serializable]
    public class DataRoot
    {
        public Test[] TestData;
    }
}