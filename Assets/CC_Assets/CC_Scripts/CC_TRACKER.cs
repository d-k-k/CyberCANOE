using UnityEngine;
using System.Collections;

/*
Keeps track of the head and wand positions and rotations by interfacing with MotiveDirect.
The user should not interface with this class directly except to disable/enable simulator mode.
Simulator mode allows the user to move around the scene without the need for Innovator's tracking system.
The Simulator controls can be seen ingame with '?' key.

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 14th, 2016.
 */

/// <summary> Retrives information of the head and wand positions and rotations by interfacing with MotiveDirect. </summary>
public class CC_TRACKER : MonoBehaviour
{

    [Tooltip("Enable/Disable the tracking. When tracking information is unavailable this should be disabled to allow you to use the simulator controls. Simulator Control's Help Menu Key : '?' ")]
    public bool enableTracking = true;
    [Tooltip("The wand you are in control of in simulator mode.")]
    public int simulatorActiveWand;

    private GameObject CC_HEAD_TRACKER;
    private GameObject CC_WAND0_TRACKER;
    private GameObject CC_WAND1_TRACKER;

    private Vector3 headPosition;
    private Quaternion headRotation;

    private Vector3[] wandPosition;
    private Quaternion[] wandRotation;

    private float WAND_X_SPAN = 2.0f;
    private float WAND_Y_SPAN = 1.5f;
    private float WAND_Z_SPAN = 2.0f;
    private float SHOULDER_HEIGHT = 1.5f;
    private float ARM_DISTANCE_IN_FRONT = 1.0f;
    private int twistStarted = 0;
    private int twistHeadStarted = 0;
    private int rotateStarted = 0;
    private Quaternion saveRotation;
    private Quaternion saveHeadRotation;

    private float HEAD_SPAN = 360;
    private float HEAD_X_SPAN = 2;
    private float HEAD_Y_SPAN = 1;
    private float HEAD_Z_SPAN = 1.83f; //6 feet

    void Awake()
    {
        CC_HEAD_TRACKER = transform.GetChild(0).FindChild("CC_FLAT_HEAD").gameObject;
        CC_WAND0_TRACKER = transform.GetChild(0).FindChild("CC_FLAT_WAND0").gameObject;
        CC_WAND1_TRACKER = transform.GetChild(0).FindChild("CC_FLAT_WAND1").gameObject;

        //Check For Tracking setting
        if(!Application.isEditor)
        {
            enableTracking = CC_COMMANDLINE.isTracking();
        }
    }

    void Start()
    {
        wandPosition = new Vector3[2];
        wandRotation = new Quaternion[2];

        //Simulator Mode - for testing purposes.
        if (!enableTracking)
        {
            setDefaultPositions();
        }
        //Get the tracker information.
        else
        {
            getTrackerInformation();
        }

    }

    void Update()
    {
        //Select wand 0 or 1.
        if (Input.GetKeyDown("1")) simulatorActiveWand = 0;
        if (Input.GetKeyDown("2")) simulatorActiveWand = 1;

        if (!enableTracking)
        {
            //Mouse movement.
            float mouseX = Input.mousePosition.x / Screen.width;
            float mouseY = Input.mousePosition.y / Screen.height;

            //Wand Movement/Roll.
            wandSimulation(mouseX, mouseY);

            //Head Movement/Roll.
            headSimulation(mouseX, mouseY);

            //Reset simulator positions
            if (Input.GetKeyDown(KeyCode.Return))
            {
                setDefaultPositions();
            }

        }
        else
        {
            getTrackerInformation();
        }

    }


    //Set the default positions for the head and wand when in simulatorm ode
    private void setDefaultPositions()
    {
        headPosition = new Vector3(0f, 1.75f, 0f);
        headRotation = Quaternion.identity;
        wandPosition[0] = new Vector3(-0.2f, 1.35f, 1f);
        wandRotation[0] = Quaternion.identity;
        wandPosition[1] = new Vector3(0.2f, 1.35f, 1f);
        wandRotation[1] = Quaternion.identity;

    }

    //Obtain tracker information from MotiveDirect.
    private void getTrackerInformation()
    {
        //Head rotation and position.
        headPosition = convertToLeftHandPosition(CC_HEAD_TRACKER.transform.position);
        headRotation = convertToLeftHandRotation(CC_HEAD_TRACKER.transform.rotation);

        //Moving the Character Controller with the head movement
        float same = CC_CANOE.CanoeCharacterController().center.y;
        CC_CANOE.CanoeCharacterController().center = new Vector3(headPosition.x, same, headPosition.z);

        //Wands rotation and position.
        wandPosition[0] = convertToLeftHandPosition(CC_WAND0_TRACKER.transform.position);
        wandPosition[1] = convertToLeftHandPosition(CC_WAND1_TRACKER.transform.position);
        wandRotation[0] = convertToLeftHandRotation(CC_WAND0_TRACKER.transform.rotation);
        wandRotation[1] = convertToLeftHandRotation(CC_WAND1_TRACKER.transform.rotation);

    }

    //Inputs for head movements in simulator mode.
    private void headSimulation(float mouseX, float mouseY)
    {

        float same;

        //Turn head.
        if (Input.GetKey(KeyCode.Q))
        {
            headRotation = Quaternion.AngleAxis(mouseX * HEAD_SPAN - (HEAD_SPAN / 2), Vector3.up) * Quaternion.AngleAxis(-(mouseY * HEAD_SPAN / 2 - (HEAD_SPAN / 4)), Vector3.right);

        }

        //Move the head left/right and forward/backward.
        if (Input.GetKey(KeyCode.Z))
        {
            same = headPosition.z;
            headPosition = new Vector3(mouseX * HEAD_X_SPAN - (HEAD_X_SPAN / 2), mouseY * HEAD_Y_SPAN / 2 + SHOULDER_HEIGHT, same);
            //Moving the Character Controller with the head movement
            same = CC_CANOE.CanoeCharacterController().center.y;
            CC_CANOE.CanoeCharacterController().center = new Vector3(headPosition.x, same, headPosition.z);
        }

        //Move the head left/right and up/down.
        if (Input.GetKey(KeyCode.C))
        {
            same = headPosition.y;
            headPosition = new Vector3(mouseX * HEAD_X_SPAN - (HEAD_X_SPAN / 2), same, mouseY * HEAD_Z_SPAN / 2);
            //Moving the Character Controller with the head movement
            same = CC_CANOE.CanoeCharacterController().center.y;
            CC_CANOE.CanoeCharacterController().center = new Vector3(headPosition.x, same, headPosition.z);
        }

        //Roll the head
        if (Input.GetKey(KeyCode.E))
        {
            if (twistHeadStarted == 0)
            {
                twistHeadStarted = 1;
                saveHeadRotation = headRotation;
            }
            else
            {
                headRotation = saveHeadRotation * Quaternion.AngleAxis(-(mouseX * 180 - 90), Vector3.forward);
            }
        }
        else
        {
            twistHeadStarted = 0;
        }

    }


    //Inputs for wand movements in simulator mode.
    private void wandSimulation(float mouseX, float mouseY)
    {
        //Move wand left/right and up/down.
        if (Input.GetKey(KeyCode.N))
        {
            wandPosition[simulatorActiveWand] = new Vector3(mouseX * WAND_X_SPAN - (WAND_X_SPAN / 2), mouseY * WAND_Y_SPAN - (WAND_Y_SPAN / 2) + SHOULDER_HEIGHT, ARM_DISTANCE_IN_FRONT);
        }

        //Move wand left/right and forward/backward.
        if (Input.GetKey(KeyCode.Comma))
        {
            wandPosition[simulatorActiveWand] = new Vector3(mouseX * WAND_X_SPAN - (WAND_X_SPAN / 2), SHOULDER_HEIGHT, mouseY * WAND_Z_SPAN);
        }

        //Yaw/pitch wand.
        if (Input.GetKey(KeyCode.Y))
        {
            if (rotateStarted == 0)
            {
                rotateStarted = 1;
                saveRotation = wandRotation[simulatorActiveWand];
            }
            else
            {
                wandRotation[simulatorActiveWand] = saveRotation * Quaternion.AngleAxis(mouseX * 180 - 90, Vector3.up) * Quaternion.AngleAxis(-(mouseY * 180 - 90), Vector3.right);

            }
        }
        else
        {
            rotateStarted = 0;
        }

        //Roll Wand.
        if (Input.GetKey(KeyCode.I))
        { 
            if (twistStarted == 0)
            {
                twistStarted = 1;
                saveRotation = wandRotation[simulatorActiveWand];
            }
            else
            {
                wandRotation[simulatorActiveWand] = saveRotation * Quaternion.AngleAxis(-(mouseX * 180 - 90), Vector3.forward);
            }
        }
        else
        {
            twistStarted = 0;
        }
    }

    public Vector3 getHeadPosition()
    {
        return headPosition;
    }

    public Quaternion getHeadRotation()
    {
        return headRotation;
    }

    public Vector3 getWandPosition(int x)
    {
        return wandPosition[x];
    }

    public Quaternion getWandRotation(int x)
    {
        return wandRotation[x];
    }

    private Vector3 convertToLeftHandPosition(Vector3 p)
    {
        return new Vector3(p.x, p.y, p.z);
    }

    private Quaternion convertToLeftHandRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }
}
