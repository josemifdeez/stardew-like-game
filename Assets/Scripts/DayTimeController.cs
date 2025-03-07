﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum DayOfWeek 
{ 
    DOMINGO,
    LUNES,
    MARTES,
    MIERCOLES,
    JUEVES,
    VIERNES,
    SABADO
}

public enum Season 
{ 
    INVIERNO,
    PRIMAVERA,
    VERANO,
    OTOÑO
}

public class DayTimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;
    const float phaseLength = 900f; // 15 minutes chunk of time
    const float phasesInDay = 96f; //secondsInDay divided by phaseLength

    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;

    float time;
    [SerializeField] float timeScale = 60f;
    [SerializeField] float startAtTime = 28800f; // in seconds.
    [SerializeField] float morningTime = 28800f;

    DayOfWeek dayOfWeek;

    [SerializeField] Text hourText;
    [SerializeField] Text dayOfTheWeekText;
    [SerializeField] Text seasonText;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D globalLight;
    public int days;

    Season currentSeason;
    const int seasonLength = 30;

    List<TimeAgent> agents;

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }

    private void Start()
    {
        time = startAtTime;
        UpdateDayText();
        UpdateSeasonText();
    }

    public void Subscribe(TimeAgent timeAgent) 
    {
        agents.Add(timeAgent);
    }

    public void Unsubscribe(TimeAgent timeAgent) 
    {
        agents.Remove(timeAgent);
    }

    float Hours 
    {
        get { return time / 3600f; }
    }

    float Minutes
    {
        get { return time % 3600f / 60f; }
    }

    private void Update()
    {
        time += Time.deltaTime * timeScale;

        TimeValueCalculation();
        DayLight();

        if (time > secondsInDay)
        {
            NextDay();
        }

        TimeAgents();

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            SkipTime(hours: 4);
        }
    }

    private void TimeValueCalculation()
    {
        int hh = (int)Hours;
        int mm = (int)Minutes;
        hourText.text = hh.ToString("00") + ":" + mm.ToString("00");
    }

    private void DayLight()
    {
        if (SceneManager.GetActiveScene().name == "CaveScene")
        {
            return; // Do not update light in CaveScene
        }
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
    }

    int oldPhase = -1;
    private void TimeAgents()
    {
        if (oldPhase == -1) 
        {
            oldPhase = CalculatePhase();
        }

        int currentPhase = CalculatePhase();

        while (oldPhase < currentPhase) 
        {
            oldPhase += 1;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke(this);
            }
        }
    }

    private int CalculatePhase()
    {
        return (int)(time / phaseLength) + (int)(days * phasesInDay);
    }

    private void NextDay()
    {
        time -= secondsInDay;
        days += 1;

        int dayNum = (int)dayOfWeek;
        dayNum += 1;
        if (dayNum >= 7)
        {
            dayNum = 0;
        }
        dayOfWeek = (DayOfWeek)dayNum;
        UpdateDayText();

        if (days >= seasonLength) 
        {
            NextSeason();
        }
    }

    private void NextSeason()
    {
        days = 0;
        int seasonNum = (int)currentSeason;
        seasonNum += 1;

        if (seasonNum >= 4)
        {
            seasonNum = 0;
        }

        currentSeason = (Season)seasonNum;
        UpdateSeasonText();
    }

    private void UpdateSeasonText()
    {
        seasonText.text = currentSeason.ToString();
    }

    private void UpdateDayText()
    {
        dayOfTheWeekText.text = dayOfWeek.ToString();
    }

    public void SkipTime(float seconds = 0, float minute = 0, float hours = 0) 
    {
        float timeToSkip = seconds;
        timeToSkip += minute * 60f;
        timeToSkip += hours * 3600f;

        time += timeToSkip;
    }

    public void SkipToMorning()
    {
        float secondsToSkip = 0f;

        if (time > morningTime)
        {
            secondsToSkip += secondsInDay - time + morningTime;
        }
        else {
            secondsToSkip += morningTime - time;
        }

        SkipTime(secondsToSkip);
    }
}
