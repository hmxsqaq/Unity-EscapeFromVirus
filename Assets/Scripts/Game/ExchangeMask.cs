using System.Collections;
using Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class ExchangeMask : MonoBehaviour
    {
        private bool _successfulExchange = true;
        public int minTriggerTime;
        public int maxTriggerTime;
        public int exchangeTime;

        private void Start()
        {
            StartCoroutine(Counter());
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Protagonist"))
            {
                _successfulExchange = true;
            }
        }

        IEnumerator Counter()
        {
            int triggerTime = Random.Range(minTriggerTime, maxTriggerTime);
            Debug.Log($"TriggerTime={triggerTime}");
            yield return new WaitForSeconds(triggerTime);
            Debug.Log("Start Exchange");
            StartCoroutine(ExchangeMode(exchangeTime));
            StopCoroutine(Counter());
        }

        IEnumerator ExchangeMode(int lastingTime)
        {
            _successfulExchange = false;
            while (!_successfulExchange)
            {
                Debug.Log(lastingTime);
                yield return new WaitForSeconds(1f);
                lastingTime -= 1;
                if (lastingTime < 0)
                {
                    EventManager.Instance.Trigger(EventNameHelper.GameOver);
                }
            }
            StartCoroutine(Counter());
            StopCoroutine(ExchangeMode(exchangeTime));
        }
    }
}