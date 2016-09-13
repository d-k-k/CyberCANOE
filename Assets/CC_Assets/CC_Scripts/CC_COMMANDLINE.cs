using System;
using System.Runtime.InteropServices;

/* 
Class to obtain command line arguments.

CyberCANOE Virtual Reality API for Unity3D
Modified by Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 14th, 2016.
 */

/// <summary> Class to obtain command line arguments. </summary>
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

    //Check to see if tracking is enabled
    public static bool isTracking()
    {
        string cmdIndex = GetCmdArguments("-tracking");

        if (cmdIndex != null)
        {
            int parseInt = int.Parse(cmdIndex);
            if (parseInt == 1)
                return true;
            else
                return false;
        }
        else return false;
    }

}
