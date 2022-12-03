using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
    [SerializeField] private bool isAlive = true;


    private void Awake()
    {
        minutes = 0;
        seconds = 0;
        StartCoroutine(TimeFlow());
    }

    IEnumerator TimeFlow()
    {
        while (isAlive)
        {
            if (seconds == 59)
            {
                minutes++;
                seconds = -1;
            }
            seconds += 1;
            text.text = $"{minutes}:{seconds}";
            yield return new WaitForSeconds(1f);
        }
    }

    public void ReceiveDeath()
    {
        isAlive = false;
        StopCoroutine(TimeFlow());
        text.text = $"Living time: {minutes}:{seconds}";

    }
}
