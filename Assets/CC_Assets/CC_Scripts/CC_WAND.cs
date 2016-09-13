using UnityEngine;
using System.Collections;

/* 
Updates the wand position and rotation by interfacing with CC_TRACKER.

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 14th, 2016.
 */

/// <summary> Keeps track of the wand information. </summary>
public class CC_WAND : MonoBehaviour
{
    [Tooltip("The device number of this wand. You should leave this to the default setting. Left = 0, Right = 1.")]
    public int wandDeviceNumber;
    private CC_TRACKER scr;

    void Awake()
    {
        scr = GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>();
    }

    void Update()
    {
        //Set the location of the wand from the tracker information.
        transform.localPosition = scr.getWandPosition(wandDeviceNumber);
        transform.localRotation = scr.getWandRotation(wandDeviceNumber);
    }
}

