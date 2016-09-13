using UnityEngine;
using XboxCtrlrInput;

/*
The main class the user will interface with to obtain controller information.
This class interfaces with XboxCtrlInput (https://github.com/JISyed/Unity-XboxCtrlrInput)

CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: September 14th, 2016.
 */

/// <summary> Interface with the wand controllers. </summary>
public static class CC_WANDCONTROLS
{
    private static XboxController WAND0 = XboxController.First;
    private static XboxController WAND1 = XboxController.Second;
    
    /// <summary>
    /// Returns true at the frame the 'X' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_X_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.G))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.A, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.A, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
        
    }
    /// <summary>
    /// Returns true at the frame the 'X' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_X_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.G))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.A, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.A, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'X' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_X_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.G))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.A, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.A, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'O' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_O_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.L))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.B, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.B, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'O' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_O_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.L))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.B, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.B, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'O' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_O_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.L))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.B, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.B, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'Shoulder' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_Shoulder_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.B))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.LeftBumper, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.LeftBumper, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'Shoulder' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_Shoulder_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.B))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.LeftBumper, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.LeftBumper, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'Shoulder' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_Shoulder_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.B))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.LeftBumper, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.LeftBumper, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'Joystick' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_Joystick_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.M))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.LeftStick, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.LeftStick, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'Joystick' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_Joystick_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.M))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.LeftStick, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.LeftStick, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'Joystick' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Button_Joystick_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.M))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.LeftStick, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.LeftStick, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'Dpad UP' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Up_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.DPadUp, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.DPadUp, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'Dpad UP' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Up_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.UpArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.DPadUp, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.DPadUp, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'Dpad Up' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Up_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.UpArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.DPadUp, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.DPadUp, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'Dpad DOWN' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Down_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.DPadDown, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.DPadDown, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'Dpad DOWN' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Down_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.DownArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.DPadDown, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.DPadDown, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'Dpad DOWN' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Down_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.DownArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.DPadDown, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.DPadDown, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'Dpad LEFT' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Left_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.DPadLeft, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.DPadLeft, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'Dpad LEFT' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Left_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.LeftArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.DPadLeft, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.DPadLeft, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'Dpad LEFT' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Left_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.DPadLeft, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.DPadLeft, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns true at the frame the 'Dpad RIGHT' button starts to press down (not held down) by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Right_DOWN(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyDown(KeyCode.RightArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonDown(XboxButton.DPadRight, WAND0);
            else if (wandNum == 1) return XCI.GetButtonDown(XboxButton.DPadRight, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true at the frame the 'Dpad RIGHT' button is released by the specified wand. 
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Right_UP(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKeyUp(KeyCode.RightArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButtonUp(XboxButton.DPadRight, WAND0);
            else if (wandNum == 1) return XCI.GetButtonUp(XboxButton.DPadRight, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }
    /// <summary>
    /// Returns true if the 'Dpad RIGHT' button is held down by the specified wand.  
    /// </summary>
    /// <param name="wandNum">Wand number.  Left = 0  Right = 1</param>
    public static bool Dpad_Right_PRESS(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        else
        {
            if (wandNum == 0) return XCI.GetButton(XboxButton.DPadRight, WAND0);
            else if (wandNum == 1) return XCI.GetButton(XboxButton.DPadRight, WAND1);
            else return wandNumberErrorBool(wandNum);
        }
    }


    /// <summary>
    /// Returns the float number for the 'X-Axis' for the specified wand. 
    /// </summary>
    /// <param name="wandNum">>Wand number.  Left = 0  Right = 1</param>
    public static float Axis_Joystick_X(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && (Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.K)))
        {
            if (Input.GetKey(KeyCode.H)) return -1.0f;
            else return 1.0f;
        }
        else
        {
            if (wandNum == 0) return XCI.GetAxis(XboxAxis.LeftStickX, WAND0);
            else if (wandNum == 1) return XCI.GetAxis(XboxAxis.LeftStickX, WAND1);
            else return wandNumberErrorFloat(wandNum);
        }
    }
    /// <summary>
    /// Returns the float number for the 'Y-Axis' for the specified wand. 
    /// </summary>
    /// <param name="wandNum">>Wand number.  Left = 0  Right = 1</param>
    public static float Axis_Joystick_Y(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && (Input.GetKey(KeyCode.U) || Input.GetKey(KeyCode.J)))
        {
            if (Input.GetKey(KeyCode.J)) return -1.0f;
            else return 1.0f;
        }
        else
        {
            if (wandNum == 0) return XCI.GetAxis(XboxAxis.LeftStickY, WAND0);
            else if (wandNum == 1) return XCI.GetAxis(XboxAxis.LeftStickY, WAND1);
            else return wandNumberErrorFloat(wandNum);
        }
    
    }
    /// <summary>
    /// Returns true the float number for the 'Trigger' for the specified wand. 
    /// </summary>
    /// <param name="wandNum">>Wand number.  Left = 0  Right = 1</param>
    public static float Axis_Trigger(int wandNum)
    {
        if (GameObject.Find("CC_TRACKER").GetComponent<CC_TRACKER>().simulatorActiveWand == wandNum && Input.GetKey(KeyCode.Space))
        {
            return 1.0f;
        }
        else
        {
            if (wandNum == 0) return XCI.GetAxis(XboxAxis.LeftTrigger, WAND0);
            else if (wandNum == 1) return XCI.GetAxis(XboxAxis.LeftTrigger, WAND1);
            else return wandNumberErrorFloat(wandNum);
        }
    }

    private static bool wandNumberErrorBool(int x)
    {
        Debug.Log("Wand number: " + x + " is not supported. Use either '0' for left wand, or '1' for right wand.");
        return false;
    }

    private static float wandNumberErrorFloat(int x)
    {
        Debug.Log("Wand number: " + x + " is not supported. Use either '0' for left wand, or '1' for right wand.");
        return 0.0f;
    }


}
