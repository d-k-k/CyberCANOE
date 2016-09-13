﻿using UnityEngine;
using System.Collections;

/* 
This is an example script that allows the user to grab objects.
This script should be attached to the CC_WAND_LEFT or CC_WAND_RIGHT game objects under the CC_CANOE game object.
Shoulder Button - Grab object

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 14th, 2016.
 */

public class CCaux_Grabber : MonoBehaviour
{
    private int wandNum = 0;

    private GameObject currentObject = null;
    private GameObject grabbedObject = null;
    private Transform grabbedObjectParent = null;
    private bool wasKinematic = false;

    void Start()
    {
        wandNum = GetComponent<CC_WAND>().wandDeviceNumber;
    }

    void Update()
    {

        if (CC_WANDCONTROLS.Button_Shoulder_PRESS(wandNum))
        {
            if (grabbedObject == null)
            {

                if (currentObject != null)
                {
                    grabbedObject = currentObject;

                    // If object had a rigidbody, grabbed save the rigidbody's kinematic state
                    // so it can be restored on release of the object
                    Rigidbody body = null;
                    body = grabbedObject.GetComponent<Rigidbody>();
                    if (body != null)
                    {
                        wasKinematic = body.isKinematic;
                        body.isKinematic = true;
                    }

                    // Save away to original parentage of the grabbed object
                    grabbedObjectParent = grabbedObject.transform.parent;

                    // Make the grabbed object a child of the wand
                    grabbedObject.transform.parent = CC_CANOE.WandGameObject(wandNum).transform;
                    currentObject = null;

                    // Disable collision between yourself and the grabbed object so that the grabbed object
                    // does not apply its physics to you and push you off the world
                    Physics.IgnoreCollision(CC_CANOE.CanoeCharacterController(), grabbedObject.GetComponent<Collider>(), true);

                }
            }
        }
        else
        {

            if (grabbedObject != null)
            {

                // Restore the original parentage of the grabbed object
                grabbedObject.transform.parent = grabbedObjectParent;

                // If object had a rigidbody, restore its kinematic state
                Rigidbody body = null;
                body = grabbedObject.GetComponent<Rigidbody>();
                if (body != null)
                {
                    body.isKinematic = wasKinematic;
                }

                //Re-enstate collision between self and object
                Physics.IgnoreCollision(CC_CANOE.CanoeCharacterController(), grabbedObject.GetComponent<Collider>(), false);

                grabbedObject = null;
                currentObject = null;
            }

        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if (grabbedObject == null)
            currentObject = collision.gameObject;
    }

    void OnTriggerExit(Collider collision)
    {
        currentObject = null;
    }
}
