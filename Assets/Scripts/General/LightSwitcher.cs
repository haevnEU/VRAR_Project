using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightSwitcher : MonoBehaviour
{
    [Header("Controlled Lightsource")]
    [SerializeField]
    GameObject[] lightSources;

    [Header("Controlled lightbulbs")]
    [SerializeField]
    GameObject[] lightBulbs;

    [Header("Indicator which shows the ligh state")]
    [SerializeField]
    GameObject indicator;

    [Header("Desired color of the light source")]
    [SerializeField]
    Color color;

    [Header("True if the lightswitch is remote, a remote switch never changes his color")]
    [SerializeField]
    bool isRemote = false;

    private bool isOn = false;

    public void Awake()
    {
        lightOn();
    }


    public void toggleLighlight()
    {
        if (isOn)
        {
            lightOff();
        }
        else
        {
            lightOn();
        }
    }

    public void lightOn()
    {
        isOn = true;
        lighting();
    }

    public void lightOff()
    {
        isOn = false;
        lighting();
    }

    private void lighting()
    { 
        if (!isRemote)
        {
            GetComponent<Renderer>().material.color = isOn ? color : Color.gray;
        }
        if (indicator != null)
        {
            indicator.GetComponent<Renderer>().material.color = isOn ? Color.green : Color.gray;
        }
        foreach (var obj in lightSources)
        {
            obj.SetActive(isOn);
        }
        foreach (var obj in lightBulbs)
        {
            obj.GetComponent<Renderer>().material.color = isOn ? color : Color.gray;
        }
    }

    public void changeLightColor()
    {
        foreach (var obj in lightSources)
        {
            obj.GetComponent<Light>().color = color;
        }
        foreach(var obj in lightBulbs)
        {
            obj.GetComponent<Renderer>().material.color = color;
        }

    }


    public void makeLightBrighter()
    {
        foreach (var obj in lightSources)
        {
            obj.GetComponent<Light>().intensity = obj.GetComponent<Light>().intensity + 1;
        }
    }

    public void makeLightDarker()
    {
        foreach (var obj in lightSources)
        {
            obj.GetComponent<Light>().intensity = obj.GetComponent<Light>().intensity - 1;
            if (obj.GetComponent<Light>().intensity <= 0)
            {
                lightOff();
            }
        }
    }
}
