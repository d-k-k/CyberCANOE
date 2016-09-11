using UnityEngine;
using System.Collections;

/* 
This is an example script that allows the user to move around the scene. Controls are similar to a modern First Person Shooter.
Left Wand Joystick - Forward/Backward and Strafe
Right Wand Joystick - Look around

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 9th, 2016.
*/

public class CCaux_Navigator : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float lookSpeed = 1.25f;

    private CharacterController charCont;
    private CC_CANOE canoeScript;


    void Start()
    {
        //Get the character controller from CC_CANOE.
        charCont = CC_CANOE.CanoeCharacterController();
        canoeScript = CC_CANOE.CanoeGameObject().GetComponent<CC_CANOE>();
    }

    void Update()
    {
        //Directions obtained from CC_CANOE class. We use the Left wand for orientation.
        //Where the left wand is pointed the player will move in that direction when they push the 
        //left wand joystick up.
        Vector3 forwardDir = CC_CANOE.WandTransform(0).forward;
        Vector3 rightDir = CC_CANOE.WandTransform(0).right;

        //The input from the X and Y axis from the left wand joystick.
        float forward = CC_WANDCONTROLS.Axis_Joystick_Y(0);
        float strafe = CC_WANDCONTROLS.Axis_Joystick_X(0);

        //Move the CharacterController attached to the CC_CANOE. 
        Vector3 movement = (forwardDir * forward) + (rightDir * strafe);
        charCont.Move(movement * Time.deltaTime * moveSpeed);

        //The input from the X and Y axis of the right wand joystick.
        canoeScript.yaw += lookSpeed * CC_WANDCONTROLS.Axis_Joystick_X(1);
        canoeScript.pitch -= lookSpeed * CC_WANDCONTROLS.Axis_Joystick_Y(1);

        //Yaw and Pitch Correction
        if (canoeScript.yaw >= 360 || canoeScript.yaw <= -360) canoeScript.yaw = 0;
        if (canoeScript.pitch > 85) canoeScript.pitch = 85;
        if (canoeScript.pitch < -85) canoeScript.pitch = -85;

        //Change the direction the CharacterController is facing.
        charCont.transform.eulerAngles = new Vector3(canoeScript.pitch, canoeScript.yaw, 0.0f);

    }
}
