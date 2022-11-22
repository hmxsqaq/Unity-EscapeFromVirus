using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class EndTime : MonoBehaviour
{
    public SpriteRenderer background;
    public Sprite[] backgrounds;
    private void Start()
    {
        string sMinutes = GameModel.Instance.Minutes < 10 ? $"0{GameModel.Instance.Minutes}" : $"{GameModel.Instance.Minutes}";
        string sSeconds = GameModel.Instance.Seconds < 10 ? $"0{GameModel.Instance.Seconds}" : $"{GameModel.Instance.Seconds}";
        GetComponent<TextMeshProUGUI>().text = sMinutes + ":" + sSeconds;
        background.sprite = backgrounds[GameModel.Instance.BackgroundIndex];
    }
}
