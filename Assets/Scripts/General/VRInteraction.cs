using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteraction : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHover()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void onHoverExit()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); ;
    }

    public void quitGame()
    {
        Application.Quit(); 
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
