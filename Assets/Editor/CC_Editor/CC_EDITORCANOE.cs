using UnityEngine;
using System.Collections;
using UnityEditor;


/* 
Unity Editor script that performs miscellaneous tasks before buidling project.
Must click the "CLICK HERE BEFORE BUIDLING" button on the CC_CANOE GameObject.

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 8th, 2016.
 */

/// <summary> Performs miscellaneous tasks before buidling project. </summary>
[CustomEditor(typeof(CC_CANOE))]
public class CC_EDITORCANOE : Editor
{
    public override void OnInspectorGUI()
    {
      
        DrawDefaultInspector();

        GUILayout.Space(20);
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        if (GUILayout.Button("CLICK HERE BEFORE BUILDING", style, GUILayout.Height(25)))
        {
            BeforeBuild();
        }
        
    }

    private void BeforeBuild()
    {
        GameObject.Find("CC_CANOE").GetComponent<CC_CANOE>().productName = Application.productName;
        GameObject.Find("CC_CANOE").GetComponent<CC_CANOE>().showScreen = CC_CANOE.ShowScreen.None;
        PlayerSettings.defaultIsFullScreen = false;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
        PlayerSettings.resizableWindow = true;

    }

}
