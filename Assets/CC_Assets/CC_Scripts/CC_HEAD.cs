using UnityEngine;
using System.Collections;

/* 
Updates the head position and rotation by interfacing with CC_TRACKER.
Also used to switch between the simulator and Innovator's cameras.

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 5th, 2016.
 */

/// <summary> Keeps track of the head information. </summary>
public class CC_HEAD : MonoBehaviour
{
    private CC_TRACKER tracker;

    void Awake()
    {
        tracker = GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>();
    }

    void Update()
    {
        //Set the location of the head from the tracker information.
        transform.localPosition = tracker.getHeadPosition();
        transform.localRotation = tracker.getHeadRotation();

    }

}
