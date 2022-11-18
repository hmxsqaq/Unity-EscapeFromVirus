using System;
using System.Collections;
using Framework;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Game
{
    public class ElementGenerator : MonoBehaviour
    {
        [Header("List")]
        public GameObject[] harmfulList;
        public GameObject[] beneficialList;
        
        [Header("Probability")]
        [Range(0f, 1f)] public float harmProbability;

        [Header("Difficulty")] 
        public float[] difficultyList = new[] { 1f, 0.8f, 0.5f };

        private WaitForSeconds _waitForSeconds;
        private const float Radius = 8f;
        private Vector2 _generatePosition;

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(difficultyList[0]);
            EventManager.Instance.AddEventListener(EventNameHelper.OnMinuteChange,DifficultySelect);
            
            StartCoroutine(ElementGenerate());
        }

        private void DifficultySelect()
        {
            if (GameModel.Instance.Minutes <= difficultyList.Length)
            {
                _waitForSeconds = new WaitForSeconds(difficultyList[GameModel.Instance.Minutes]);
            }
            else
            {
                _waitForSeconds = new WaitForSeconds(difficultyList[difficultyList.Length]);
            }
        }

        IEnumerator ElementGenerate()
        {
            while (true)
            {
                float r_angle = Random.Range(0, Mathf.PI * 2);
                _generatePosition = new Vector3(Radius * Mathf.Sin(r_angle), Radius * Mathf.Cos(r_angle), 0);
                int r_list = Random.Range(0, 100);
                if (r_list < 100 * harmProbability)
                {
                    int index = Random.Range(0, harmfulList.Length);
                    Instantiate(harmfulList[index], _generatePosition, Quaternion.identity, transform);
                }
                else
                {
                    int index = Random.Range(0, beneficialList.Length);
                    Instantiate(beneficialList[index], _generatePosition, Quaternion.identity, transform);
                }
                yield return _waitForSeconds;
            }
        }
    }
}