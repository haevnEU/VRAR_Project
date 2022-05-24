using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerInteraction : MonoBehaviour
{
    [Header("Shower Head")]
    [SerializeField]
    GameObject showerHead;

    private Vector3 oldPosition = new Vector3(0, 0, 0);
    private bool isShowering = false;

    private void Awake()
    {
    }
  
    public void enterExitShower()
    {
        var XrOrigin = GameObject.Find("XR Origin");
        Vector3 pos;
        if (!isShowering)
        {
            isShowering = true;
            oldPosition = XrOrigin.transform.position;
            pos = GetComponent<MeshCollider>().transform.position;
        }
        else
        {
            isShowering = false;
            pos = oldPosition;
        }
        XrOrigin.transform.position = pos;
 //       var rotations = XrOrigin.transform.rotation;
 //       XrOrigin.transform.Rotate(-90, rotations.y, rotations.z);
    }
    public void startStopShower()
    {
        var ps = showerHead.GetComponent<ParticleSystem>();
        if (ps.isPlaying)   
        {

            ps.Stop();
        }
        else
        {
            ps.Play();  
        }

    }


}
