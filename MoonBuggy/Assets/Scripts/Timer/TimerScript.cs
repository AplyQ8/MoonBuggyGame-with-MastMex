using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
    [SerializeField] private int milliseconds;
    [SerializeField] private bool isAlive = true;


    private void Awake()
    {
        minutes = 0;
        seconds = 0;
        milliseconds = 0;
        StartCoroutine(TimeFlow());
    }

    IEnumerator TimeFlow()
    {
        while (isAlive)
        {
            if (milliseconds == 99)
            {
                seconds++;
                milliseconds = -1;
            }

            if (seconds == 59)
            {
                minutes++;
                seconds = -1;
            }
            milliseconds += 1;
            text.text = $"{minutes}:{seconds}:{milliseconds}";
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ReceiveDeath()
    {
        isAlive = false;
        StopCoroutine(TimeFlow());
    }
}
