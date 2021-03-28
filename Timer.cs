using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public TMP_Text text;
    void Start()
    {
        text.text = "00:00.00";
    }

    public void UpdateTimer(){
        float minutes = Mathf.FloorToInt(Time.time / 60);
        float seconds = Mathf.FloorToInt(Time.time % 60);
        float miliseconds = Mathf.FloorToInt(Time.time * 1000) % 1000;

        text.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, miliseconds);
    }
}
