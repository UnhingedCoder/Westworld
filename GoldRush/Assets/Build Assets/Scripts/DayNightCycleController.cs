using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycleController : MonoBehaviour
{
    #region INSPECTOR_REG
    [Header ("TIME"), Range(0, 24)]
    [SerializeField] private float timeOfDay;

    [SerializeField] private float orbitSpeed = 1.0f;

    [Header ("CELESTIALS")]
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;

    //System
    private bool isNight;

    #endregion

    #region UNITY_REG
    private void OnValidate()
    {

        SetCelestialRotation();
    }

    private void Update()
    {
        
        UpdateTime();
        SetCelestialRotation();
    }
    #endregion

    #region CLASS_REG
    private void UpdateTime()
    {
        timeOfDay += Time.deltaTime * orbitSpeed;
        if (timeOfDay > 24)
            timeOfDay = 0f;
    }
    
    private void SetCelestialRotation()
    {
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180f;

        sun.transform.rotation = Quaternion.Euler(sunRotation, -150f, 0f);
        moon.transform.rotation = Quaternion.Euler(moonRotation, -150f, 0f);

        CheckNightDayTransition();
    }

    private void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }

    private void StartNight()
    {
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
    }

    private void StartDay()
    {
        isNight = false;
        sun.shadows = LightShadows.Hard;
        moon.shadows = LightShadows.None;
    }
    
    
    #endregion
}
