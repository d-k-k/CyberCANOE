#CyberCANOE API Documentation

+ **wandNum** refers to the Wand Number. 
 + The **left wand is 0**, the **right wand is 1**.

<h2 id="canoe">CC_CANOE</h2>
```c#
//Returns the Transform of the specified wand.
CC_CANOE.WandTransform(int wandNum);  

//Returns the GameObject of the specified wand. 
CC_CANOE.WandGameObject(int wandNum);  

//Returns the SphereCollider of the specified wand.
CC_CANOE.WandCollider(int wandNum);  

//Returns the Transform of the head.
CC_CANOE.HeadTransform(); 
 
//Returns the GameObject of the head.
CC_CANOE.HeadGameObject(); 

//Returns the GameObject of the Canoe.
CC_CANOE.CanoeGameObject();  

//Returns the CharacterController attached to the Canoe.
CC_CANOE.CanoeCharacterController(); 
```

<h2 id="controls">CC_WANDCONTROLS</h2>
```c#
//Returns true at the frame the 'X' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Button_X_DOWN(int wandNum); 

//Returns true at the frame the 'X' button is released by the specified wand.
CC_WANDCONTROLS.Button_X_UP(int wandNum);

//Returns true if the 'X' button is held down by the specified wand.
CC_WANDCONTROLS.Button_X_PRESS(int wandNum); 

//Returns true at the frame the 'O' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Button_O_DOWN(int wandNum);  

//Returns true at the frame the 'O' button is released by the specified wand.
CC_WANDCONTROLS.Button_O_UP(int wandNum); 

//Returns true if the 'O' button is held down by the specified wand.
CC_WANDCONTROLS.Button_O_PRESS(int wandNum);  

//Returns true at the frame the 'Shoulder' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Button_Shoulder_DOWN(int wandNum); 

//Returns true at the frame the 'Shoulder' button is released by the specified wand.
CC_WANDCONTROLS.Button_Shoulder_UP(int wandNum);  

//Returns true if the 'Shoulder' button is held down by the specified wand.
CC_WANDCONTROLS.Button_Shoulder_PRESS(int wandNum);  

//Returns true at the frame the 'Joystick' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Button_Joystick_DOWN(int wandNum);  

//Returns true at the frame the 'Joystick' button is released by the specified wand.
CC_WANDCONTROLS.Button_Joystick_UP(int wandNum); 

//Returns true if the 'Joystick' button is held down by the specified wand.
CC_WANDCONTROLS.Button_Joystick_PRESS(int wandNum);  

//Returns true at the frame the 'Dpad UP' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Dpad_Up_DOWN(int wandNum); 

//Returns true at the frame the 'Dpad UP' button is released by the specified wand.
CC_WANDCONTROLS.Dpad_Up_UP(int wandNum);

//Returns true if the 'Dpad Up' button is held down by the specified wand.
CC_WANDCONTROLS.Dpad_Up_PRESS(int wandNum);  

//Returns true at the frame the 'Dpad DOWN' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Dpad_Down_DOWN(int wandNum);  

//Returns true at the frame the 'Dpad DOWN' button is released by the specified wand.
CC_WANDCONTROLS.Dpad_Down_UP(int wandNum);

//Returns true if the 'Dpad DOWN' button is held down by the specified wand
CC_WANDCONTROLS.Dpad_Down_PRESS(int wandNum);  

//Returns true at the frame the 'Dpad LEFT' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Dpad_Left_DOWN(int wandNum);  

//Returns true at the frame the 'Dpad LEFT' button is released by the specified wand.
CC_WANDCONTROLS.Dpad_Left_UP(int wandNum);  

//Returns true if the 'Dpad LEFT' button is held down by the specified wand.
CC_WANDCONTROLS.Dpad_Left_PRESS(int wandNum);  

//Returns true at the frame the 'Dpad RIGHT' button starts to press down (not held down) by the specified wand.
CC_WANDCONTROLS.Dpad_Right_DOWN(int wandNum);  

//Returns true at the frame the 'Dpad RIGHT' button is released by the specified wand.
CC_WANDCONTROLS.Dpad_Right_UP(int wandNum);  

//Returns true if the 'Dpad RIGHT' button is held down by the specified wand.
CC_WANDCONTROLS.Dpad_Right_PRESS(int wandNum);  

//Returns the float number for the 'X-Axis' for the specified wand.
CC_WANDCONTROLS.Axis_Joystick_X(int wandNum);  

//Returns the float number for the 'Y-Axis' for the specified wand.
CC_WANDCONTROLS.Axis_Joystick_Y(int wandNum);  

//Returns the float number for the 'Trigger' for the specified wand.
CC_WANDCONTROLS.Axis_Trigger(int wandNum);  
```
