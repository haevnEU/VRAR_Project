using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteableGameLights : MonoBehaviour
{

    [Header("Color of the Light")]
    [SerializeField]
    Color lightColor;


    [Header("Lightsource to interact with")]
    [SerializeField]
    Light lightSource;


    [Header("List of all objects which can activate/deactivate this source")]
    [SerializeField]
    List<GameObject> activatingObject;

    public void lightOn()
    {
        lightSource.enabled = true;
        activatingObject.ForEach(obj => { });
    }

    public void lightOff()
    {
        lightSource.enabled = false;
        activatingObject.ForEach(obj => { });
    }

}
