using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

/* 
The main class the user will interface with to retrieve Wand, Head, and CharacterController information.

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 5th, 2016.
 */

/// <summary> The main class to interface with to retrieve Wand, Head, and CharacterController information. </summary>
public class CC_CANOE : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("Enable or disable gravity the Canoe/Player experiences. Does not affect other objects in the scene.")]
    public bool applyGravity = true;

    [Header("Navigation Settings (Simulator)")]
    [Tooltip("Movement speed. Only affects simulator controls.")]
    public float navigationSpeed = 5.0f;
    [Tooltip("Rotation speed. Only affects simulator controls.")]
    public float navigationRotationSpeed = 1.25f;

    [Header("Wand Settings")]
    [Tooltip("Which wand models you wish to be visable")]
    public WandModel wandModel;
    public enum WandModel { None, Hand, Axis };
    private WandModel savedWandModel;

    [Header("Misc Settings")]
    [Tooltip("Enable/Disable the visibility of the CyberCANOE's Screens. Typically you want set to none unless you are debugging in the editor.")]
    public ShowScreen showScreen;
    public enum ShowScreen { None, Innovator, Destiny };
    private ShowScreen savedSelScreen;
    [HideInInspector]
    public string productName;


    private static GameObject CC_CANOEOBJ;
    private static GameObject CC_INNOVATOR_SCREENS;
    private static GameObject CC_DESTINY_SCREENS;
    private static GameObject CC_GUI;
    private static GameObject[] CC_WAND;
    private static GameObject CC_HEAD;
    private static CharacterController charController;

    //GLOBAL GET METHODS
    /// <summary>
    /// The transform of the specified wand.
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    /// <returns>The transform of this wand.</returns>
    public static Transform WandTransform(int wandNum) { return CC_WAND[wandNum].transform; }

    /// <summary>
    /// The gameobject of the specified wand.
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    /// <returns>The gameobject of this wand.</returns>
    public static GameObject WandGameObject(int wandNum) { return CC_WAND[wandNum]; }

    /// <summary>
    /// The collider of the specified wand. (SphereCollider)
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    /// <returns>The collider of this wand. (SphereCollider)</returns>
    public static SphereCollider WandCollider(int wandNum) { return CC_WAND[wandNum].GetComponent<SphereCollider>(); }

    /// <summary>
    /// The transform of the head.
    /// </summary>
    /// <returnsThe transform of the head.</returns>
    public static Transform HeadTransform() { return CC_HEAD.transform; }

    /// <summary>
    /// The game object of the head.
    /// </summary>
    /// <returns>The game object of the head.</returns>
    public static GameObject HeadGameObject() { return CC_HEAD; }

    /// <summary>
    /// The game object of the Canoe.
    /// </summary>
    /// <returns>The game object of the Canoe.</returns>
    public static GameObject CanoeGameObject() { return CC_CANOEOBJ; }

    /// <summary>
    /// The character controller attached to the Canoe.
    /// </summary>
    /// <returns>The character controller attached to the Canoe.</returns>
    public static CharacterController CanoeCharacterController() { return charController; }

    void Awake()
    {
        //Get the GameObjects attached to the canoe.
        CC_WAND = new GameObject[2];
        CC_WAND[0] = transform.FindChild("CC_WAND_LEFT").gameObject;
        CC_WAND[1] = transform.FindChild("CC_WAND_RIGHT").gameObject;
        CC_HEAD = transform.FindChild("CC_HEAD").gameObject;
        charController = GetComponent<CharacterController>();
        CC_CANOEOBJ = gameObject;
        CC_INNOVATOR_SCREENS = transform.FindChild("CC_INNOVATOR_SCREENS").gameObject;
        CC_DESTINY_SCREENS = transform.FindChild("CC_DESTINY_SCREENS").gameObject;
        CC_GUI = transform.FindChild("CC_GUI").gameObject;

        //Set up the windows according to the desired platform.
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string windowName;
            IntPtr windowPtr = CC_COMMANDLINE.FindWindow(null, productName);

            //Local Cluster
            if (CC_COMMANDLINE.isLocalCluster())
            {
                if (CC_COMMANDLINE.GetCameraIndex() == 0)
                {
                    windowName = productName + "-LocalCluster-1-Master";
                }
                else
                {
                    windowName = productName + "-LocalCluster-" + (CC_COMMANDLINE.GetCameraIndex() + 1) + "-Client";
                }
                CC_COMMANDLINE.SetWindowText(windowPtr, windowName);
                CC_COMMANDLINE.SetPosition((7 - CC_COMMANDLINE.GetCameraIndex()) * CC_COMMANDLINE.GetResWidth(), 0, windowName);
            }
            //Innovator Platform
            else if (CC_COMMANDLINE.isInnovator())
            {
                windowName = productName + "-Innovator";
                CC_COMMANDLINE.SetWindowText(windowPtr, windowName);
                CC_COMMANDLINE.SetPosition(0, 0, windowName);
            }
            //Destiny Platform
            else if (CC_COMMANDLINE.isDestiny())
            {
                if (CC_COMMANDLINE.GetCameraIndex() == 0)
                {
                    windowName = productName + "-Destiny-Kanaloa-1-Master";
                }
                else
                {
                    windowName = productName + "-Destiny-Kanaloa-" + (CC_COMMANDLINE.GetCameraIndex() + 1) + "-Client";
                }
                CC_COMMANDLINE.SetWindowText(windowPtr, windowName);
                CC_COMMANDLINE.SetPosition(0, 0, windowName);
            }
            //Fallback
            else
            {
                CC_COMMANDLINE.SetPosition(0, 0, productName);
            }

        }

    }

    void Start()
    {

        //Set the scale of Destiny's screens to account or not account for bezel. 
        if (Application.isEditor || CC_COMMANDLINE.isLocalCluster() || CC_COMMANDLINE.isInnovator())
        {
            foreach (Transform child in CC_DESTINY_SCREENS.transform)
            {
                child.localScale = new Vector3(0.701675f, 1.2255f, 1.0f);
            }
        }
        else
        {
            foreach (Transform child in CC_DESTINY_SCREENS.transform)
            {
                child.localScale = new Vector3(0.6797f, 1.208f, 1.0f);
            }
        }

        //Set Screen Resolution
        if (!Application.isEditor)
        {
            int width = CC_COMMANDLINE.GetResWidth();
            int height = CC_COMMANDLINE.GetResHeight();
            Debug.Log(width);
            Debug.Log(height);


            if (width != 0 && height != 0)
            {
                Screen.SetResolution(width, height, false, 60);
            }
                
            else Screen.SetResolution(1024, 768, false, 60);

        }
        changeWandModels();
        changeScreens();

    }

    void Update()
    {

        //Move forward and backward - Keyboard input only.
        float curSpeed = 0.0f;
        if (Input.GetKey("w")) curSpeed += navigationSpeed;
        if (Input.GetKey("s")) curSpeed -= navigationSpeed;
        Vector3 forward = CC_WAND[0].transform.TransformDirection(Vector3.forward);
        charController.Move(forward * curSpeed * Time.deltaTime);

        //Rotate around y - axis - Keyboard input only.
        float rotation = 0.0f;
        if (Input.GetKey("d")) rotation += navigationRotationSpeed;
        if (Input.GetKey("a")) rotation -= navigationRotationSpeed;
        charController.transform.Rotate(0, rotation, 0);

        //Gravity
        if (applyGravity)
        {
            //SimpleMove applies gravity automatically
            charController.SimpleMove(Vector3.zero);
        }

        // Show and hide the Innovator's screen.
        if (Input.GetKeyDown("x"))
        {
            showScreen++;
            if ((int)showScreen == 3) showScreen = 0;
        }
        if (savedSelScreen != showScreen) changeScreens();


        //Show and hide Simulator Mode help screen.
        if (Input.GetKeyDown("/"))
        {
            CC_GUI.SetActive(!CC_GUI.activeInHierarchy);
        }

        //Change wand models
        if (Input.GetKeyDown("m"))
        {
            wandModel++;
            if ((int)wandModel == 3) wandModel = 0;
            changeWandModels();
        }
        if (wandModel != savedWandModel)
        {
            changeWandModels();
        }

        // Press the escape key to quit application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }


    //Change the wand models
    private void changeWandModels()
    {
        switch (wandModel)
        {
            case WandModel.None:
                CC_WAND[0].transform.FindChild("CC_LEFTHAND_MODEL").gameObject.SetActive(false);
                CC_WAND[1].transform.FindChild("CC_RIGHTHAND_MODEL").gameObject.SetActive(false);
                CC_WAND[0].transform.FindChild("CC_AXIS_MODEL").gameObject.SetActive(false);
                CC_WAND[1].transform.FindChild("CC_AXIS_MODEL").gameObject.SetActive(false);
                savedWandModel = wandModel;
                break;

            case WandModel.Hand:
                CC_WAND[0].transform.FindChild("CC_LEFTHAND_MODEL").gameObject.SetActive(true);
                CC_WAND[1].transform.FindChild("CC_RIGHTHAND_MODEL").gameObject.SetActive(true);
                CC_WAND[0].transform.FindChild("CC_AXIS_MODEL").gameObject.SetActive(false);
                CC_WAND[1].transform.FindChild("CC_AXIS_MODEL").gameObject.SetActive(false);
                savedWandModel = wandModel;
                break;
            case WandModel.Axis:
                CC_WAND[0].transform.FindChild("CC_LEFTHAND_MODEL").gameObject.SetActive(false);
                CC_WAND[1].transform.FindChild("CC_RIGHTHAND_MODEL").gameObject.SetActive(false);
                CC_WAND[0].transform.FindChild("CC_AXIS_MODEL").gameObject.SetActive(true);
                CC_WAND[1].transform.FindChild("CC_AXIS_MODEL").gameObject.SetActive(true);
                savedWandModel = wandModel;
                break;
        }
    }

    //Change the visiblity of the screens.
    private void changeScreens()
    {
        switch (showScreen)
        {
            case ShowScreen.None:
                CC_INNOVATOR_SCREENS.SetActive(false);
                CC_DESTINY_SCREENS.SetActive(false);
                savedSelScreen = showScreen;
                break;
            case ShowScreen.Innovator:
                CC_INNOVATOR_SCREENS.SetActive(true);
                CC_DESTINY_SCREENS.SetActive(false);
                savedSelScreen = showScreen;
                break;
            case ShowScreen.Destiny:
                CC_INNOVATOR_SCREENS.SetActive(false);
                CC_DESTINY_SCREENS.SetActive(true);
                savedSelScreen = showScreen;
                break;
        }
    }

}
