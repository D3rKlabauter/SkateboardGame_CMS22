using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour

{

    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("TimerSettings")]
    public float currentTime;
    public bool countDown;
    void Start()
    {

    }


    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("0.0");
    }
}