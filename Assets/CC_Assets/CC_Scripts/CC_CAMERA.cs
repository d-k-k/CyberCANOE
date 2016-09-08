using UnityEngine;

/* 
Manages the cameras of the CyberCANOE.
 
CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 5th, 2016.
*/

/// <summary> Manages all the cameras for Destiny and Innovator required for stereocropic and off-axis projection. </summary>
public class CC_CAMERA : MonoBehaviour
{
    [Header("Camera Select")]
    [Tooltip("Select which camera to use. You can also change camera's with the 'P' key.")]
    public SelectedCamera selectCamera;
    public enum SelectedCamera { Simulator, Innovator, Destiny };
    private SelectedCamera savedSelCam;

    [Header("Destiny Camera Rig")]
    [SerializeField]
    private CC_CAMERARIG m_DestinyCameraRig;
    [Range(0, 7)]
    [Tooltip("Only change while the in the editor to debug and to view the different cameras of Destiny.")]
    public int destinyCameraIndex;
    [Space(5)]

    private Camera[] destinyCameras;
    private Camera innovatorCamera;

    private GameObject innovatorCameraGroup;
    private GameObject destinyCameraGroup;
    private GameObject simulatorCameraGroup;

    [Header("Stereo Settings and Materials")]
    [Tooltip("Enable/Disable stereoscopic.")]
    public bool enableStereo;
    private bool savedStereo;
    [Tooltip("Interpupillary distance in meters.")]
    public float interaxial = 0.055f;
    private float savedInteraxial;

    [Space(5)]
    public Material destinyStereoMaterial;
    public Material innovatorStereoMaterial;

    private float savedAspectRatio;
    private float interaxialTimeChange;
    private GUIStyle style;

    void Start()
    {
      
        //If in editor set the starting destiny camera to 4. It is near the middle. 
        if (Application.isEditor) destinyCameraIndex = 4;

        //Get camera groups
        innovatorCameraGroup = transform.FindChild("CC_INNOVATOR_CAMERAS").gameObject;
        destinyCameraGroup = transform.FindChild("CC_DESTINY_CAMERAS").gameObject;
        simulatorCameraGroup = transform.FindChild("CC_SIM_CAMERA").gameObject;

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
            destinyCameras[i].GetComponent<CC_CAMERASTEREO>().createStereoCameras(true);

        //Update Cameras
        updateCamerasInteraxials();
        updateCamerasStereo();
        updateCamerasAspectRatio();

        //Save current settings
        savedAspectRatio = GetComponent<Camera>().aspect;
        savedStereo = enableStereo;
        savedInteraxial = interaxial;

        //INTERAXIAL GUI SETUP
        interaxialTimeChange = -3;
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        //Set startup camera according to platform
        if (!Application.isEditor)
        {
            if (CC_COMMANDLINE.isInnovator())
                selectCamera = SelectedCamera.Innovator;
            else if (CC_COMMANDLINE.isDestiny() || CC_COMMANDLINE.isLocalCluster())
                selectCamera = SelectedCamera.Destiny;
            else
                selectCamera = SelectedCamera.Simulator;
        }
        changeCameras();

    }

    void Update()
    {
        //Change cameras
        if (Input.GetKeyDown(KeyCode.P))
        {
            selectCamera++;
            if ((int)selectCamera == 3) selectCamera = 0;
        }
        if (savedSelCam != selectCamera) changeCameras();

        //Change interaxial
        if (Input.GetKeyDown(KeyCode.Equals)) interaxial += .001f;
        if (Input.GetKeyDown(KeyCode.Minus)) interaxial -= .001f;

        //Enable/disable stereoscopic.
        if (Input.GetKeyDown("o"))
            enableStereo = !enableStereo;

        //Checks to see if the stereo setting has changed.
        if (savedStereo != enableStereo)
            updateCamerasStereo();

        //Checks for changes to interaxial and updates camera's interaxial accordingly.
        if (savedInteraxial != interaxial)
            updateCamerasInteraxials();

        //Checkes for changes to the aspect ratio for innovator and upadtes camera's accordingly.
        if (savedAspectRatio != GetComponent<Camera>().aspect)
            updateCamerasAspectRatio();

    }
    
    void LateUpdate()
    {
        SetDestinyPerspective();
    }

    //Stereoscopic
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
    }

    //Set the perspective of the Destiny camera rig.
    void SetDestinyPerspective()
    {
        if (Application.isEditor)
        {
            m_DestinyCameraRig.updateCameraPerspective(destinyCameras, destinyCameraIndex);
        }
        else
        {
            int cameraIndex = CC_COMMANDLINE.GetCameraIndex();

            if (cameraIndex == -1) return;

            m_DestinyCameraRig.updateCameraPerspective(destinyCameras, cameraIndex);
        }

    }

    //Updates innovator's camera when aspect ratio changes.
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
    }

    //Updates each camera child's interaxial setting.
    private void updateCamerasInteraxials()
    {
        interaxialTimeChange = Time.time;
        GameObject head = GameObject.Find("CC_HEAD");

        innovatorCamera.GetComponent<CC_CAMERASTEREO>().updateInteraxial(head, interaxial);
        m_DestinyCameraRig.updateCameraInteraxials(destinyCameras, interaxial);
        savedInteraxial = interaxial;
    }

    //Displays the current interaxial setting to the screen.
    void OnGUI()
    {
        string value = (interaxial * 1000).ToString("00.");

        if (Time.time - interaxialTimeChange < 3)
        {
            GUI.Label(new Rect(Screen.width / 2 - 130, Screen.height / 2, 200, 100), "Interpupillary Distance: " + value + "mm", style);
        }

    }

}
