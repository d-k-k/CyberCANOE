using UnityEngine;

/* 
Manages the cameras of the CyberCANOE.
 
CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 14th, 2016.
*/

/// <summary> Manages all the cameras for Destiny and Innovator. </summary>
public class CC_CAMERA : MonoBehaviour
{
    [Header("Camera Select")]
    [Tooltip("Select which camera to use. Keyboard Shortcut: 'P'")]
    public SelectedCamera selectCamera;
    public enum SelectedCamera { Simulator, Innovator, Destiny };
    private SelectedCamera savedSelCam;

    [Header("Destiny Camera Rig")]
    [SerializeField]
    private CC_CAMERARIG m_DestinyCameraRig;
    [Range(0, 7)]
    [Tooltip("Change view to different cameras of Destiny. Keyboard Shortcut: '[' and ']'")]
    public int destinyCameraIndex;
    [Space(5)]

    [Header("Stereo Settings and Materials")]
    [Tooltip("Enable/Disable stereoscopic. Keyboard Shortcut: '9'")]
    public bool enableStereo;
    private bool savedStereo;
    [Tooltip("Interpupillary distance in meters. Keyboard Shortcut: '-' and '+'")]
    public float interaxial = 0.055f;
    private float savedInteraxial;

    [Space(5)]
    [Tooltip("Stereo Material. DO NOT CHANGE.")]
    public Material destinyStereoMaterial;
    [Tooltip("Stereo Material. DO NOT CHANGE.")]
    public Material innovatorStereoMaterial;

    private Camera[] destinyCameras;
    private Camera innovatorCamera;
    private Camera simCam;
    private GameObject innovatorCameraGroup;
    private GameObject destinyCameraGroup;
    private GameObject simulatorCameraGroup;

    private float savedAspectRatio;
    private bool panOptic;
    private float guiTimeChange;
    private string guiDisplay;
    private GUIStyle style;


    void Start()
    {

        //Get camera groups
        innovatorCameraGroup = transform.FindChild("CC_INNOVATOR_CAMERAS").gameObject;
        destinyCameraGroup = transform.FindChild("CC_DESTINY_CAMERAS").gameObject;
        simulatorCameraGroup = transform.FindChild("CC_SIM_CAMERA").gameObject;

        //Simulator Camera Setup
        simCam = simulatorCameraGroup.GetComponent<Camera>();
        simCam.rect = GetComponent<Camera>().rect;
        simCam.nearClipPlane = GetComponent<Camera>().nearClipPlane;
        simCam.farClipPlane = GetComponent<Camera>().farClipPlane;
        simCam.clearFlags = GetComponent<Camera>().clearFlags;
        simCam.backgroundColor = GetComponent<Camera>().backgroundColor;
        simCam.cullingMask = GetComponent<Camera>().cullingMask;

        //Innovator Camera Setup
        innovatorCamera = innovatorCameraGroup.transform.GetChild(0).gameObject.GetComponent<Camera>();
        innovatorCamera.GetComponent<CC_CAMERASTEREO>().createStereoCameras(false);
        
        //Destiny Camera Setup
        destinyCameras = new Camera[4];
        destinyCameras[0] = destinyCameraGroup.transform.GetChild(0).GetComponent<Camera>();
        destinyCameras[1] = destinyCameraGroup.transform.GetChild(1).GetComponent<Camera>();
        destinyCameras[2] = destinyCameraGroup.transform.GetChild(2).GetComponent<Camera>();
        destinyCameras[3] = destinyCameraGroup.transform.GetChild(3).GetComponent<Camera>();
        for (int i = 0; i < 4; i++)
        {
            destinyCameras[i].GetComponent<CC_CAMERASTEREO>().createStereoCameras(true);
        }

        //Update Cameras
        updateCamerasInteraxials();
        updateCamerasStereo();
        updateCamerasAspectRatio();

        //Save current settings
        savedAspectRatio = GetComponent<Camera>().aspect;
        savedStereo = enableStereo;
        savedInteraxial = interaxial;
        panOptic = false;

        //GUI Setup
        style = new GUIStyle();
        if (CC_COMMANDLINE.isDestiny() || CC_COMMANDLINE.isInnovator())
        {
            style.fontSize = 100;
        }
        else
        {
            style.fontSize = 25;
        }
        style.normal.textColor = Color.white;

        //Set startup camera according to platform
        if (!Application.isEditor)
        {
            if (CC_COMMANDLINE.isInnovator())
                selectCamera = SelectedCamera.Innovator;
            else if (CC_COMMANDLINE.isDestiny())
                selectCamera = SelectedCamera.Destiny;
            else
                selectCamera = SelectedCamera.Simulator;
        }
        changeCameras();

    }

    void Update()
    {
        //Change cameras
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectCamera++;
            if ((int)selectCamera == 3)
            {
                selectCamera = 0;
            }
        }
        if (savedSelCam != selectCamera)
        {
            changeCameras();
        }

        //Change interaxial
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            interaxial += .001f;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            interaxial -= .001f;
        }
        if (savedInteraxial != interaxial)
        {
            updateCamerasInteraxials();
        }

        //Enable/disable stereoscopic.
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            enableStereo = !enableStereo;
        }
        if (savedStereo != enableStereo)
        {
            updateCamerasStereo();
        }

        //Enable/disable Panoptic for Destiny.
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            panOptic = !panOptic;
            guiTimeChange = Time.time;
            guiDisplay = "panopticGUI";
        }

        //Checkes for changes to the aspect ratio for innovator and upadtes camera's accordingly.
        if (savedAspectRatio != GetComponent<Camera>().aspect)
        {
            updateCamerasAspectRatio();
        }

        //Change the Destiny Camera Index
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            destinyCameraIndex++;
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            destinyCameraIndex--;
        }
        destinyCameraIndex = Mathf.Clamp(destinyCameraIndex, 0, 7);

    }
    
    void LateUpdate()
    {
        SetDestinyPerspective();
    }

    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (selectCamera == SelectedCamera.Destiny)
        {

            if (enableStereo)
            {
                destinyStereoMaterial.SetTexture("leftTopLeft", destinyCameras[0].GetComponent<CC_CAMERASTEREO>().getLeftRenderTexture());
                destinyStereoMaterial.SetTexture("leftBottomLeft", destinyCameras[1].GetComponent<CC_CAMERASTEREO>().getLeftRenderTexture());
                destinyStereoMaterial.SetTexture("leftTopRight", destinyCameras[2].GetComponent<CC_CAMERASTEREO>().getLeftRenderTexture());
                destinyStereoMaterial.SetTexture("leftBottomRight", destinyCameras[3].GetComponent<CC_CAMERASTEREO>().getLeftRenderTexture());

                destinyStereoMaterial.SetTexture("rightTopLeft", destinyCameras[0].GetComponent<CC_CAMERASTEREO>().getRightRenderTexture());
                destinyStereoMaterial.SetTexture("rightBottomLeft", destinyCameras[1].GetComponent<CC_CAMERASTEREO>().getRightRenderTexture());
                destinyStereoMaterial.SetTexture("rightTopRight", destinyCameras[2].GetComponent<CC_CAMERASTEREO>().getRightRenderTexture());
                destinyStereoMaterial.SetTexture("rightBottomRight", destinyCameras[3].GetComponent<CC_CAMERASTEREO>().getRightRenderTexture());

                destinyStereoMaterial.SetFloat("resX", Screen.width);
                destinyStereoMaterial.SetFloat("resY", Screen.height);

                Graphics.Blit(destination, destinyStereoMaterial, 0);

            }
            else
            {
                destinyStereoMaterial.SetTexture("centerTopLeft", destinyCameras[0].GetComponent<CC_CAMERASTEREO>().getCenterRenderTexture());
                destinyStereoMaterial.SetTexture("centerBottomLeft", destinyCameras[1].GetComponent<CC_CAMERASTEREO>().getCenterRenderTexture());
                destinyStereoMaterial.SetTexture("centerTopRight", destinyCameras[2].GetComponent<CC_CAMERASTEREO>().getCenterRenderTexture());
                destinyStereoMaterial.SetTexture("centerBottomRight", destinyCameras[3].GetComponent<CC_CAMERASTEREO>().getCenterRenderTexture());

                destinyStereoMaterial.SetFloat("resX", Screen.width);
                destinyStereoMaterial.SetFloat("resY", Screen.height);

                Graphics.Blit(destination, destinyStereoMaterial, 1);
            }
        }

        else if (selectCamera == SelectedCamera.Innovator)
        {
            if (enableStereo)
            {
                innovatorStereoMaterial.SetFloat("InterlaceValue", Screen.height);

                innovatorStereoMaterial.SetTexture("LeftTex", innovatorCamera.GetComponent<CC_CAMERASTEREO>().getLeftRenderTexture());
                innovatorStereoMaterial.SetTexture("RightTex", innovatorCamera.GetComponent<CC_CAMERASTEREO>().getRightRenderTexture());

                Graphics.Blit(destination, innovatorStereoMaterial, 0);

            }
            else
            {
                Graphics.Blit(innovatorCamera.GetComponent<CC_CAMERASTEREO>().getCenterRenderTexture(), destination);
            }
        }

    }

    //Change which camera is in use.
    private void changeCameras()
    {
        switch (selectCamera)
        {
            case SelectedCamera.Simulator:
                simulatorCameraGroup.SetActive(true);
                innovatorCameraGroup.SetActive(false);
                destinyCameraGroup.SetActive(false);
                savedSelCam = selectCamera;
                break;
            case SelectedCamera.Innovator:
                simulatorCameraGroup.SetActive(false);
                innovatorCameraGroup.SetActive(true);
                destinyCameraGroup.SetActive(false);
                savedSelCam = selectCamera;
                break;
            case SelectedCamera.Destiny:
                simulatorCameraGroup.SetActive(false);
                innovatorCameraGroup.SetActive(false);
                destinyCameraGroup.SetActive(true);
                savedSelCam = selectCamera;
                break;
        }
        guiTimeChange = Time.time;
        guiDisplay = "cameraGUI";
    }

    //Set the perspective of the Destiny camera rig.
    void SetDestinyPerspective()
    {
        if (!CC_COMMANDLINE.isDestiny())
        {
            m_DestinyCameraRig.updateCameraPerspective(destinyCameras, destinyCameraIndex, panOptic);
        }
        else
        {
            int cameraIndex = CC_COMMANDLINE.GetCameraIndex();

            if (cameraIndex == -1) return;

            m_DestinyCameraRig.updateCameraPerspective(destinyCameras, cameraIndex, panOptic);
        }

    }

    //Updates each camera's aspect ratio.
    private void updateCamerasAspectRatio()
    {
        innovatorCamera.GetComponent<CC_CAMERASTEREO>().updateScreenAspect(false);
        m_DestinyCameraRig.updateCameraScreenAspect(destinyCameras);
        savedAspectRatio = GetComponent<Camera>().aspect;
    }

    //Disables or enables center, left, or right cameras depending on if stereo is enabled.
    private void updateCamerasStereo()
    {
        if (enableStereo)
        {
            innovatorCamera.GetComponent<CC_CAMERASTEREO>().disableCenterCamera();
            m_DestinyCameraRig.updateCameraStereo(destinyCameras, enableStereo);
        }
        else
        {
            innovatorCamera.GetComponent<CC_CAMERASTEREO>().enableCenterCamera();
            m_DestinyCameraRig.updateCameraStereo(destinyCameras, enableStereo);
        }

        savedStereo = enableStereo;
        guiTimeChange = Time.time;
        guiDisplay = "stereoGUI";
    }

    //Updates each camera's interaxial setting.
    private void updateCamerasInteraxials()
    {
        GameObject head = gameObject;
        innovatorCamera.GetComponent<CC_CAMERASTEREO>().updateInteraxial(head, interaxial);
        m_DestinyCameraRig.updateCameraInteraxials(destinyCameras, interaxial);

        savedInteraxial = interaxial;
        guiTimeChange = Time.time;
        guiDisplay = "interaxialGUI";
    }

    //Displays the information to the screen.
    void OnGUI()
    {
        string value = (interaxial * 1000).ToString("00.");

        if (Time.time - guiTimeChange < 3)
        {
            Rect textRect = new Rect();
            if (CC_COMMANDLINE.isInnovator() || CC_COMMANDLINE.isDestiny())
                textRect = new Rect(15, Screen.height - 115, 200, 100);
            else
                textRect = new Rect(5, Screen.height - 30, 200, 100);

            if (guiDisplay.Equals("interaxialGUI"))
                GUI.Label(textRect, "Pupillary Distance: " + value + "mm", style);
            else if (guiDisplay.Equals("stereoGUI"))
                GUI.Label(textRect, "Stereo: " + savedStereo, style);
            else if (guiDisplay.Equals("panopticGUI"))
                GUI.Label(textRect, "Panoptic: " + panOptic, style);
            else if (guiDisplay.Equals("cameraGUI"))
                GUI.Label(textRect, "Camera: " + selectCamera.ToString(), style);
        }

    }

}
