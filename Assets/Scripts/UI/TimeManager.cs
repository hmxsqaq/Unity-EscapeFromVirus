using System.Collections;
using Framework;
using Game;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);
    
    private void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TimeCount());
    }

    IEnumerator TimeCount()
    {
        while (true)
        {
            string sMinutes = GameModel.Instance.Minutes < 10 ? $"0{GameModel.Instance.Minutes}" : $"{GameModel.Instance.Minutes}";
            string sSeconds = GameModel.Instance.Seconds < 10 ? $"0{GameModel.Instance.Seconds}" : $"{GameModel.Instance.Seconds}";
            _tmp.text = sMinutes + ":" + sSeconds;
            GameModel.Instance.Seconds += 1;
            if (GameModel.Instance.Seconds > 59)
            {
                GameModel.Instance.Seconds = 0;
                GameModel.Instance.Minutes += 1;
            }
            yield return _waitForSeconds;
        }
    }
}
