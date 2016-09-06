using System;
using System.Runtime.InteropServices;

/* 
Class to obtain command line arguments and position Unity Windows.

CyberCANOE Virtual Reality API for Unity3D
Modified by Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 5th, 2016.
 */

/// <summary> Class to obtain command line arguments and position Unity Windows. </summary>
public static class CC_COMMANDLINE {

    //Get Command Arguments
    private static string GetCmdArguments(string arg)
    {
        string[] arguments = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; i++)
        {
            if (arguments[i] == arg)
            {
                if (i + 1 < arguments.Length)
                    return arguments[i + 1];
            }
        }
        // default to null
        return null;
    }

    //Get the camera index of this client
    public static int GetCameraIndex()
    {
        // load from command line
        string cmdIndex = GetCmdArguments("-client");
        // if there is an index given from the command line (slave node)
        if (cmdIndex != null)
            return int.Parse(cmdIndex);
        // if node is master node and option has been chosen to integrate it
        else return 0;

    }


    //Check to see if this is a local cluster setup that will run the master and all clients on a single computer
    public static bool isLocalCluster()
    {
        string cmdIndex = GetCmdArguments("-platform");

        if (cmdIndex != null)
        {
            if (cmdIndex.Equals("localcluster"))
                return true;
            else
                return false;
        }
        else return false;
    }

    //Check to see if this is a Destiny setup
    public static bool isDestiny()
    {
        string cmdIndex = GetCmdArguments("-platform");

        if (cmdIndex != null)
        {
            if (cmdIndex.Equals("destiny"))
                return true;
            else
                return false;
        }
        else return false;
    }

    //Check to see if this is a Innovator setup
    public static bool isInnovator()
    {
        string cmdIndex = GetCmdArguments("-platform");

        if (cmdIndex != null)
        {
            if (cmdIndex.Equals("innovator"))
                return true;
            else
                return false;
        }
        else return false;
    }

    //Get the resolution height of the application
    public static int GetResHeight()
    {
        int x;
        Int32.TryParse(GetCmdArguments("-height"), out x);
        return x;
    }

    //Get the resolution height of the application
    public static int GetResWidth()
    {
        int x;
        Int32.TryParse(GetCmdArguments("-width"), out x);
        return x;
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    public static void SetPosition(int x, int y, string windowName, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, windowName), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }
#endif


}
