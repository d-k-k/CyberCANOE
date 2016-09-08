using UnityEngine;
using System.Collections;

/* 
Each part of the cluster renderer has it's own camera rig.
Each camera rigg is composed of four cameras, one for each screen.

CyberCANOE Virtual Reality API for Unity3D
Modified by Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 5th, 2016.
 */

/// <summary> Destiny's Camera Rig for each cluster. </summary>
[System.Serializable]
public class CC_CAMERARIG
{
    public GameObject Screens;

    public void updateCameraPerspective(Camera[] cameras, int cameraIndex)
    {
        //Get this camera's projection screen
        GameObject[] projScreens = new GameObject[4];
        projScreens[0] = Screens.transform.GetChild(cameraIndex * 4).gameObject;
        projScreens[1] = Screens.transform.GetChild(cameraIndex * 4 + 1).gameObject;
        projScreens[2] = Screens.transform.GetChild(cameraIndex * 4 + 2).gameObject;
        projScreens[3] = Screens.transform.GetChild(cameraIndex * 4 + 3).gameObject;

        //Set each camera's projection
        PerspectiveOffCenter(cameras[0], projScreens[0]);
        PerspectiveOffCenter(cameras[0].transform.GetChild(0).GetComponent<Camera>(), projScreens[0]);
        PerspectiveOffCenter(cameras[0].transform.GetChild(1).GetComponent<Camera>(), projScreens[0]);

        PerspectiveOffCenter(cameras[1], projScreens[1]);
        PerspectiveOffCenter(cameras[1].transform.GetChild(0).GetComponent<Camera>(), projScreens[1]);
        PerspectiveOffCenter(cameras[1].transform.GetChild(1).GetComponent<Camera>(), projScreens[1]);

        PerspectiveOffCenter(cameras[2], projScreens[2]);
        PerspectiveOffCenter(cameras[2].transform.GetChild(0).GetComponent<Camera>(), projScreens[2]);
        PerspectiveOffCenter(cameras[2].transform.GetChild(1).GetComponent<Camera>(), projScreens[2]);

        PerspectiveOffCenter(cameras[3], projScreens[3]);
        PerspectiveOffCenter(cameras[3].transform.GetChild(0).GetComponent<Camera>(), projScreens[3]);
        PerspectiveOffCenter(cameras[3].transform.GetChild(1).GetComponent<Camera>(), projScreens[3]);
    }

    //Updates each camera child's interaxial setting.
    public void updateCameraInteraxials(Camera[] cameras, float interaxial)
    {
        GameObject head = GameObject.Find("CC_HEAD");

        foreach (Camera camera in cameras)
        {
            camera.GetComponent<CC_CAMERASTEREO>().updateInteraxial(head, interaxial);
        }

    }

    //Updates each camera when aspect ratio changes.
    public void updateCameraScreenAspect(Camera[] cameras)
    {
        foreach (Camera camera in cameras)
        {
            camera.GetComponent<CC_CAMERASTEREO>().updateScreenAspect(true);
        }

    }

    //Disables or enables center, left, or right cameras depending on if stereo is enabled.
    public void updateCameraStereo(Camera[] cameras, bool enableStereo)
    {
        if (enableStereo)
        {
            foreach (Camera camera in cameras)
            {
                camera.GetComponent<CC_CAMERASTEREO>().disableCenterCamera();
            }


        }
        else
        {
            foreach (Camera camera in cameras)
            {
                camera.GetComponent<CC_CAMERASTEREO>().enableCenterCamera();
            }

        }

    }

    private void PerspectiveOffCenter(Camera camera, GameObject projectionScreen)
    {

        //Lower left corner of projection screen in world coordinates
        Vector3 PSLL = projectionScreen.transform.TransformPoint(new Vector3(-0.5f, -0.5f, 0.0f));
        //Lower right corner of projection screen in world coordinates
        Vector3 PSLR = projectionScreen.transform.TransformPoint(new Vector3(0.5f, -0.5f, 0.0f));
        //Upper left corner of projection screen in world coordinates
        Vector3 PSUL = projectionScreen.transform.TransformPoint(new Vector3(-0.5f, 0.5f, 0.0f));

        //Eye Position
        Vector3 eye = camera.transform.position;
        //Distance to near clipping plane
        float nearClipDistance = camera.nearClipPlane;
        //Distance to far clipping plane
        float farClipDistance = camera.farClipPlane;

        //Distances from Eye to Projection Screen Corners
        Vector3 eyeToPSLL = PSLL - eye;
        Vector3 eyeToPSLR = PSLR - eye;
        Vector3 eyeToPSUL = PSUL - eye;

        //right axis of screen
        Vector3 psRight = PSLR - PSLL;
        psRight.Normalize();
        //up axis of screen
        Vector3 psUp = PSUL - PSLL;
        psUp.Normalize();
        //normal vector of screen
        Vector3 psNormal = -Vector3.Cross(psRight, psUp);

        //distance from eye to screen
        float d = -Vector3.Dot(eyeToPSLL, psNormal);
        //distance to left screen edge
        float l = Vector3.Dot(psRight, eyeToPSLL) * nearClipDistance / d;
        //ditance to right screen edge 
        float r = Vector3.Dot(psRight, eyeToPSLR) * nearClipDistance / d;
        //ditance to bottom screen edge
        float b = Vector3.Dot(psUp, eyeToPSLL) * nearClipDistance / d;
        //distance to top screen edge
        float t = Vector3.Dot(psUp, eyeToPSUL) * nearClipDistance / d;

        //Projection matrix 
        Matrix4x4 p = new Matrix4x4();
        p[0, 0] = 2.0f * nearClipDistance / (r - l);
        p[0, 1] = 0.0f;
        p[0, 2] = (r + l) / (r - l);
        p[0, 3] = 0.0f;

        p[1, 0] = 0.0f;
        p[1, 1] = 2.0f * nearClipDistance / (t - b);
        p[1, 2] = (t + b) / (t - b);
        p[1, 3] = 0.0f;

        p[2, 0] = 0.0f;
        p[2, 1] = 0.0f;
        p[2, 2] = (farClipDistance + nearClipDistance) / (nearClipDistance - farClipDistance);
        p[2, 3] = 2.0f * farClipDistance * nearClipDistance / (nearClipDistance - farClipDistance);

        p[3, 0] = 0.0f;
        p[3, 1] = 0.0f;
        p[3, 2] = -1.0f;
        p[3, 3] = 0.0f;

        //Rotation matrix;
        Matrix4x4 rm = new Matrix4x4();
        rm[0, 0] = psRight.x;
        rm[0, 1] = psRight.y;
        rm[0, 2] = psRight.z;
        rm[0, 3] = 0.0f;

        rm[1, 0] = psUp.x;
        rm[1, 1] = psUp.y;
        rm[1, 2] = psUp.z;
        rm[1, 3] = 0.0f;

        rm[2, 0] = psNormal.x;
        rm[2, 1] = psNormal.y;
        rm[2, 2] = psNormal.z;
        rm[2, 3] = 0.0f;

        rm[3, 0] = 0.0f;
        rm[3, 1] = 0.0f;
        rm[3, 2] = 0.0f;
        rm[3, 3] = 1.0f;

        //Translation matrix;
        Matrix4x4 tm = new Matrix4x4();
        tm[0, 0] = 1.0f;
        tm[0, 1] = 0.0f;
        tm[0, 2] = 0.0f;
        tm[0, 3] = -eye.x;

        tm[1, 0] = 0.0f;
        tm[1, 1] = 1.0f;
        tm[1, 2] = 0.0f;
        tm[1, 3] = -eye.y;

        tm[2, 0] = 0.0f;
        tm[2, 1] = 0.0f;
        tm[2, 2] = 1.0f;
        tm[2, 3] = -eye.z;

        tm[3, 0] = 0.0f;
        tm[3, 1] = 0.0f;
        tm[3, 2] = 0.0f;
        tm[3, 3] = 1.0f;

        // set matrices
        camera.projectionMatrix = p;
        camera.worldToCameraMatrix = rm * tm;
    }

}

