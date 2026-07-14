//////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Input Manager Script                                                                                //
//  Description: This script monitors input from all joysticks and controllers                          //
//  Written by: Don Hileman   
// 	Copyright Enyx Studios, LLC                                                                         //
// 	Written On: November 30, 2016                                                                       //
//////////////////////////////////////////////////////////////////////////////////////////////////////////



/* to use Input Axis' type InputManager_Enyx.xAxis or whatever other axis you may be using.
 * to us button clicks type if(InputManager_Enyx.aButton) or whatever other button you want to check. 
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_PS4            
using UnityEngine.PS4;
#endif
//using UnityEngine.PS4.IODevices;
//using UnityEngine.PS4.Engines;
using System.ComponentModel;
using System.IO;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.VR;


public class InputManager_Enyx: MonoBehaviour
{
	public delegate void TimescaleAction(int scale); // true mean menu is visible. false means it isnt.
	public static event TimescaleAction OnTimescaleChange;


    public static bool usingJoystick = false;
    public static bool usingViveControllers = false;
    public static bool usingOculusControllers = false;
	public static bool usingPs4TrackedDualshock = false;

    public static float xAxis;
    public static float yAxis;
    public static bool xAxisDown;   //this modification allows us to tell if the joystick was moved in this direction. Only true for the first frame
    public static bool yAxisDown;   //this modification allows us to tell if the joystick was moved in this direction. Only true for the first frame
    public static float mouseX;
    public static float mouseY;
    public static float leftTrigger;
    public static float rightTrigger;
    public static float dPadX;
    public static float dPadY;
    public static bool aButtonDown;
	public static bool aButton;
    public static bool bButtonDown;
    public static bool xButtonDown;
    public static bool yButtonDown;
    public static bool leftBumper;
    public static bool rightBumper;
    public static bool startButton; //return true for one frame during initial press
    public static bool startButtonPressed; // is true for as long as the user holds the button
    public static bool startButtonReleased = false; //is true when the user lets up the start button
    public static bool recenterButton = false; //used to tell when user is trying to recenter
    public static bool pauseMenuButton = false; //used to tell when the user is trying to enter the pause menu
    public static bool backButton;
    public static bool leftStickClick;
    public static bool rightStickClick;
	public static Vector3 leftControllerPosition;
	public static Vector3 rightControllerPosition;
	public static Vector3 leftControllerRotation;
	public static Vector3 rightControllerRotation;

	public static bool failedToFindViveControllers = false; // used to send up a flag to whatevers listening (usually the menu) to tell if vive controllers failed

    [Header("** Which Type of Controller To Use **")]
    [Tooltip("Check this box to use a joystick for character control")]
    public bool useJoystick = true;                     // Will set the controller type to joystick

    [Tooltip("Check this box to use a Vive Controller for character control")]
    public bool useViveController;                      // Will set the controller type to Vive Controllers

	[Tooltip("Check this box to use a Oculus Controller for character control")]
	public bool useOculusController;

	[Tooltip("Check this box if you want to use dualshock motion tracking. this is usually set from within a menu")]
	public bool useTrackingOnDualshock;

    [Header("** Input Manager Names **")]
    [Space(30)]

    [Tooltip("Type in the name of the Horizontal Axis to Use")]
    public string horizontalInput = "Horizontal";                               // This is the name of the Horizontal input Axis

    [Tooltip("Type in the name of the Vertical Axis to Use")]
    public string verticalInput = "Vertical";                                   // This is the name of the Vertical input Axis

    [Tooltip("Type in the name of the Mouse X Axis to Use")]
    public string mouseXInput = "Mouse X";                                   // This is the name of the Vertical input Axis

    [Tooltip("Type in the name of the Mouse Y Axis to Use")]
    public string mouseYInput = "Mouse Y";

    [Tooltip("Type in the name of the Xbox A or PS4 Cross button input")]
    public string aButtonInput = "A_Button";                                    // This is the name of the A button Input

    [Tooltip("Type in the name of the Xbox B or PS4 Circle button input")]
    public string bButtonInput = "B_Button";                                    // This is the name of the B Button Input

    [Tooltip("Type in the name of the Xbox X or PS4 Square button input")]
    public string xButtonInput = "X_Button";                                    // This is the name of the X Button Input

    [Tooltip("Type in the name of the Xbox Y or PS4 Triangle button input")]
    public string yButtonInput = "Y_Button";                                    // This is the name of the X Button Input

    [Tooltip("Type in the name of the Xbox or PS4 Left Bumper input")]
    public string leftBumperInput = "Left_Bumper";                              // This is the name of the Xbox or PS4 Left Bumper

    [Tooltip("Type in the name of the Xbox or PS4 Right Bumper input")]
    public string rightBumperInput = "Right_Bumper";                              // This is the name of the Xbox or PS4 Left Bumper

    [Tooltip("Type in the name of the Xbox or PS4 Left Trigger input")]
    public string leftTriggerInput = "Left_Trigger";                              // This is the name of the Xbox or PS4 Left Trigger

    [Tooltip("Type in the name of the Xbox or PS4 Right Trigger input")]
    public string rightTriggerInput = "Right_Trigger";                              // This is the name of the Xbox or PS4 Right Trigger

    [Tooltip("Type in the name of the Xbox Back Button or PS4 Touchpad input")]
    public string backButtonInput = "Back_Button";                              // This is the name of the Xbox or PS4 Right Trigger

    [Tooltip("Type in the name of the Xbox Start Button or PS4 Options input")]
    public string startButtonInput = "Start_Button";                              // This is the name of the Xbox or PS4 Right Trigger

    [Tooltip("Type in the name of the Xbox or PS4 DPad X input")]
    public string dPadXInput = "DPad_X";                                            // This is the name of the Xbox or PS4 DPad X Input

    [Tooltip("Type in the name of the Xbox or PS4 DPad Y input")]
    public string dPadYInput = "DPad_Y";                                            // This is the name of the Xbox or PS4 DPad Y Input


    [Tooltip("Type in the name of the Xbox or PS4 Left Stick Click input")]
    public string leftStickClickInput = "Left_Stick_Click";                          // This is the name of the Xbox or PS4 Left Stick Click Input

    [Tooltip("Type in the name of the Xbox or PS4 Right Stick Click input")]
    public string rightStickClickInput = "Right_Stick_Click";                          // This is the name of the Xbox or PS4 Right Stick Click Input

	[Space (25)]

	[Tooltip("Since the oculus remotes don't go through unity's input manager, we can adjust their input sensitivities here. 1= no change. Less than 1 slows down input. Greater than 1 increases input.")]
	public float oculusXSensitivity = 1f;
	public float oculusYSensitivity = 1f;
	[Space(10)]
	public float oculusMouseXSensitivity = 1f;
	public float oculusMouseYSensitivity = 1f;
	[Space(25)]



	#if UNITY_STANDALONE || UNITY_EDITOR
	[Tooltip("Vive controllers populate automatically. Don't manually set these.")]
	public GameObject leftViveController;
	private Enyx_Vive_Controllers leftViveScript;

	public GameObject rightViveController;
	private Enyx_Vive_Controllers rightViveScript;

    private bool findingLeftVive = false; //used to tell if coroutine has already been started to find this controller
    private bool findingRightVive = false; //used to tell if coroutine has already been started to find this controller
#endif

#if UNITY_PS4
	private GameObject dualShock;
#endif

    private float startButtonTimer = 0.0f; //used to calculate a pause menu press or a recenter press
    private bool startButtonTimerActive = false; //tells if the timer is active
	[Space (15)]
    [Tooltip("Time it takes (in seconds) to recenter headset when button is held")]
    public float recenterTime = 2.0f;

	[Space(25)]
	[Tooltip("These will log the value of the input to the console. This is useful to tell if the sensitivies are matching between controller types")]
	public bool debugAxisSensitivities = false;
	public bool debugMouseAxisSensitivities = false;


    private static InputManager_Enyx instance;

    public static InputManager_Enyx Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }





    void OnEnable(){
		StartCoroutine ("checkTimescale");
	}



	IEnumerator checkTimescale(){
		Debug.Log ("monitoring timescale now");
		int currentTimescale = (int)Time.timeScale;
		while (true) {
			if ((int)Time.timeScale != currentTimescale) 
			{
				//time scale has changed
				Debug.Log("Timescale has changed");
				mouseX = 0;
				mouseY = 0;
				if (OnTimescaleChange != null)
				{
					OnTimescaleChange ((int)Time.timeScale);		//broadcast event so subscribers know the timescale has changed;
				}

				currentTimescale = (int)Time.timeScale;
			}
			yield return null;
		}
	}




    void LateUpdate()
    {
		if (debugMouseAxisSensitivities) 
		{
			Debug.Log ("mouseXaxis = " + mouseX);
			Debug.Log ("mouseYAxis = " + mouseY);
		}

		if (debugAxisSensitivities) 
		{
			Debug.Log ("Xaxis = " + xAxis);
			Debug.Log ("YAxis = " + yAxis);
		}

		handleControllers ();
        calculatePauseRecenter();    
    }



    //find vive controllers
    // vive controlls are objects attatched to the character controllers.
    public void initializeViveControllers()
    {
        //this is used to initialize the vive controller objects so they can be read
#if UNITY_STANDALONE || UNITY_EDITOR
        if (findingLeftVive == false)
        {
            startViveControllerManager();
            StartCoroutine("findLeftViveController");
        }
        if (findingRightVive == false)
        {
            StartCoroutine("findRightViveController");
        }
        Debug.Log("finding vive controllers");
#endif
    }


    //in order to start the game without headsets connected, and then later be able to turn on vive motion controllers, we need to first start the vive manager object
    void startViveControllerManager() 
	{
#if UNITY_STANDALONE || UNITY_EDITOR
        GameObject cam = GameObject.Find("FPS_Character_Controller_ENYX/CameraParent/Main_Camera"); //tries to find the camera componenet. Unfortunately, our camera can have two paths depending on vr or non vr setting. due to childing and unchilding of camera parent
        if (cam == null)
        { // if camera was not found in the first path
            cam = GameObject.Find("FPS_Character_Controller_ENYX/Character_Controller/CameraParent/Main_Camera");
        }
        
		if (cam != null)
        {
            SteamVR_ControllerManager mngr = cam.GetComponent<SteamVR_ControllerManager>();
            mngr.enabled = true;
        }
#endif
    }



    // The start button can be used to either enter the pause menu(short press) or recenter the headset (long press)
    // this function logically determines what the user is trying to do when they hit the start button
    void calculatePauseRecenter() 
	{
        recenterButton = false; //this is initially set to false each time the calculation starts to prevent the bool from being true for longer than a frame. if it was true last frame.
        pauseMenuButton = false; //initially set to false to prevent being true for longer than a frame if it was true last frame. Quick reset to insure only ever true for 1 frame.
        if (startButton) 
		{ //if the startbutton is initially pressed
            startButtonTimer = 0.0f; //reset the timer back to 0 incase it was to begin with
            startButtonTimerActive = true; //start a timer
        }

        if (startButtonPressed) 
		{ //if the start button is being held
            if (startButtonTimerActive) 
			{ //if the timer is active
                startButtonTimer += Time.deltaTime; //increase the timer
				
				//check if the button has been held long enough to recenter
                if (startButtonTimer >= recenterTime) 
                {
                    recenterButton = true; //time to recenter
                    startButtonTimer = 0.0f; //reset the timer back to 0
                    startButtonTimerActive = false; //shut off the timer so we dont have cascading recenters
                }
            }
        }

        if (startButtonReleased) 
		{
            //if the timer is active and the held time was less than needed to recenter
            if ((startButtonTimerActive) && (startButtonTimer < recenterTime/2)) 
            {
                pauseMenuButton = true; //user can enter the pause menu
            }

            startButtonTimer = 0.0f; //reset the timer back to 0
            startButtonTimerActive = false; //shut off the timer 
        }
    }




	//function calculates the logical value of the active axis. 
	float logicalAxis(float joystick, float vive, float oculus){
		float returnValue = 0f;
		float[] axis = {joystick,vive,oculus};

		for (int i = 0; i < axis.Length; i++) {
			if (axis [i] != 0) {
				returnValue = axis [i];
			}
		}
			
		return returnValue;
	}




	// used to calculate logical vectors for positions and rotations
	Vector3 logicalVector(Vector3 joystick, Vector3 vive, Vector3 oculus)
	{
		Vector3 returnValue = new Vector3 (0f,0f,0f);
		Vector3[] controllerVecs = { joystick, vive, oculus };

		for (int i = 0; i < controllerVecs.Length; i++) 
		{
			if ((controllerVecs [i].x != 0) || (controllerVecs [i].y != 0) || (controllerVecs [i].z != 0)) 
			{
				returnValue = controllerVecs [i];
			}
		}

		return returnValue;
	}





    public void handleControllers()
	{

#if UNITY_PS4
		if (dualShock != null){
			if (useTrackingOnDualshock){
				usingPs4TrackedDualshock = true;
				leftControllerPosition = dualShock.transform.localPosition;
				leftControllerRotation = dualShock.transform.localEulerAngles;
				//rightControllerPosition = leftControllerPosition; //for the dualshock there is no left and right controller. so both will be the same
				//rightControllerRotation = leftControllerRotation; 
			}
		}else{
			usingPs4TrackedDualshock = false;
		}
#endif


        // motion controller position

        leftControllerPosition = logicalVector(joystickGetPosition(),viveGetPosition("left"),oculusGetPosition("left"));
		leftControllerRotation = logicalVector (joystickGetRotation(), viveGetRotation ("left"), oculusGetRotation ("left"));
		rightControllerPosition = logicalVector(joystickGetPosition(),viveGetPosition("right"),oculusGetPosition("right"));
		rightControllerRotation = logicalVector (joystickGetRotation(), viveGetRotation ("right"), oculusGetRotation ("right"));


		//axis
		xAxis = logicalAxis(Input.GetAxis(horizontalInput), viveGetAxis("x"), 0f); //oculusGetAxis("x"));
		yAxis = logicalAxis(Input.GetAxis(verticalInput), viveGetAxis("y"), 0f);//oculusGetAxis("y"));
		mouseX = logicalAxis(Input.GetAxis(mouseXInput), viveGetAxis("mouseX"), 0f);//oculusGetAxis("mouseX"));
		mouseY = logicalAxis(Input.GetAxis(mouseYInput), viveGetAxis("mouseY"), 0f);//oculusGetAxis("mouseY"));
		leftTrigger = logicalAxis(Input.GetAxis(leftTriggerInput), viveGetAxis("leftTrigger"), oculusGetAxis("leftTrigger"));
		rightTrigger = logicalAxis(Input.GetAxis(rightTriggerInput), viveGetAxis("rightTrigger"), oculusGetAxis("rightTrigger"));

		//bools
		xAxisDown = Input.GetButtonDown(horizontalInput) || viveGetButtonDown("xAxis") || oculusGetButtonDown("xAxis");
		yAxisDown = Input.GetButtonDown(verticalInput) || viveGetButtonDown("yAxis") || oculusGetButtonDown("yAxis");

		aButton = Input.GetButton (aButtonInput) || viveGetButton("a") || oculusGetButton("a");
		aButtonDown = Input.GetButtonDown (aButtonInput) || viveGetButtonDown ("a") || oculusGetButtonDown ("a");     //Gets input from Xbox A or PS4 Cross Button
		bButtonDown = Input.GetButtonDown (bButtonInput) || viveGetButtonDown ("b") || oculusGetButtonDown("b");
		xButtonDown = Input.GetButtonDown (xButtonInput) || viveGetButtonDown("x") || oculusGetButtonDown("x");
		yButtonDown = Input.GetButtonDown(yButtonInput) || viveGetButtonDown("y") || oculusGetButtonDown("y");
		startButton = Input.GetButtonDown(startButtonInput) || viveGetButtonDown("start") || oculusGetButtonDown("start");
		startButtonPressed = Input.GetButton(startButtonInput) || viveGetButton("start") || oculusGetButton("start");
		startButtonReleased = Input.GetButtonUp(startButtonInput) || viveGetButtonReleased("start") || oculusGetButtonReleased("start");


    }



	Vector3 joystickGetPosition()
	{
		Vector3 returnValue = new Vector3 (0f, 0f, 0f);
		#if UNITY_PS4
		if (dualShock != null)
		{
			returnValue = dualShock.transform.localPosition;

			leftControllerRotation = dualShock.transform.localEulerAngles;
		}
		#endif
		return returnValue;
	}

	Vector3 joystickGetRotation()
	{
		Vector3 returnValue = new Vector3 (0f, 0f, 0f);
		#if UNITY_PS4
		if (dualShock != null)
		{
			returnValue = dualShock.transform.localEulerAngles;
		}
		#endif
		return returnValue;
	}



    bool viveGetButton(string button)
	{
		bool returnValue = false;
#if !UNITY_PS4

        if ((leftViveController != null) && (leftViveScript != null) && (rightViveController != null) && (rightViveScript != null)) 
		{ 
			if (button == "start")
			{
				returnValue = rightViveScript.menuButtonPressed;
			}
			if (button == "a") 
			{
				returnValue = rightViveScript.downButtonPressed;
			}
		}
#endif
		return returnValue;
	}


    bool viveGetButtonDown(string button)
	{
		bool returnValue = false;
#if !UNITY_PS4

        if ((leftViveController != null) && (leftViveScript != null) && (rightViveController != null) && (rightViveScript != null)) 
		{ 
			if (button == "start")
			{
				returnValue = rightViveScript.menuButtonDown;
			}
			
			if ((button == "a") || (button == "A")) 
			{
				returnValue = rightViveScript.downButtonDown;
			}
			
			if ((button == "b") || (button == "B"))
			{
				returnValue = rightViveScript.rightButtonDown;
			}
			
			if ((button == "x") || (button == "X"))
			{
				returnValue = rightViveScript.leftButtonDown;
			}
			
			if ((button == "y") || (button == "Y"))
			{
				returnValue = rightViveScript.upButtonDown;
			}

			if (button == "xAxis")
			{
				returnValue = leftViveScript.rightButtonDown || leftViveScript.leftButtonDown;
			}
			
			if (button == "yAxis")
			{
				returnValue = leftViveScript.upButtonDown || leftViveScript.downButtonDown;
			}
		}
#endif
		return returnValue;
	}


	bool viveGetButtonReleased(string button)
    {
		bool returnValue = false;

# if !UNITY_PS4
        if ((leftViveController != null) && (leftViveScript != null) && (rightViveController != null) && (rightViveScript != null))
        { 
			if (button == "start")
            {
				returnValue = rightViveScript.menuButtonUp;
			}
		}
#endif

        return returnValue;

    }


    float viveGetAxis(string Axis)
	{
		float returnValue = 0f;

#if !UNITY_PS4
        if ((leftViveController != null) && (leftViveScript != null) && (rightViveController != null) && (rightViveScript != null))
        { 
			if ((Axis == "x")||(Axis == "X"))
			{
				returnValue = leftViveScript.touchPadXAxis;
			}
			
			if ((Axis == "y")||(Axis == "Y"))
			{
				returnValue = leftViveScript.touchPadYAxis;
			}

			if (Axis == "dPadX")
			{
				returnValue = leftViveScript.touchPadXAxis; //dpad on the vive is the same as the joystick. I'm setting the values also so all other code wont have to be changed if it calls dpad.
			}

			if (Axis == "dPadY")
			{
				returnValue = leftViveScript.touchPadYAxis;
			}

			if (Axis == "mouseX")
			{
				returnValue = rightViveScript.touchPadXAxis;
			}

			if (Axis == "mouseY")
			{
				returnValue = rightViveScript.touchPadYAxis;
			}
			
			if (Axis == "leftTrigger")
			{
				if (leftViveScript.triggerButtonPressed)
				{ //input manager needs trigger info as a float. Vive returns a bool at this point. This block converts bool to float.
					returnValue = 1f;
				}
				
				else
				{
					returnValue = 0f;
				}
			}

		}
#endif
#endif
        return returnValue;
	}


	Vector3 viveGetPosition(string motionController)
	{
		Vector3 returnValue = new Vector3 (0f, 0f, 0f);
		#if UNITY_STANDALONE || UNITY_EDITOR
		if ((leftViveController != null) && (leftViveScript != null) && (rightViveController != null) && (rightViveScript != null))
		{
			if ((motionController == "left") || (motionController == "Left"))
			{
				returnValue = leftViveController.transform.localPosition;
			}
			
			if ((motionController == "right") || (motionController == "Right"))
			{
				returnValue = rightViveController.transform.localPosition;
			}
		}
		#endif
		return returnValue;
	}

	Vector3 viveGetRotation(string motionController)
	{
		Vector3 returnValue = new Vector3 (0f, 0f, 0f);
		#if UNITY_STANDALONE || UNITY_EDITOR
		if ((leftViveController != null) && (leftViveScript != null) && (rightViveController != null) && (rightViveScript != null))
		{
			if ((motionController == "left") || (motionController == "Left"))
			{
				returnValue = leftViveController.transform.localEulerAngles;
			}
			
			if ((motionController == "right") || (motionController == "Right"))
			{
				returnValue = rightViveController.transform.localEulerAngles;
			}
		}
		#endif
		return returnValue;
	}




	//oculus controller getters

	bool oculusGetButton(string button)
	{
		bool returnValue = false;
		
		#if ((UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_PS4))
		if (button == "start")
		{
			returnValue = OVRInput.Get(OVRInput.RawButton.Start);
		}
		
		if (button == "a") 
		{
			returnValue = OVRInput.Get(OVRInput.RawButton.A);
		}
		#endif
		return returnValue;
	}


	bool oculusGetButtonDown(string button)
	{
		bool returnValue = false;
		#if ((UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_PS4))
		
		if (button == "start")
		{
			returnValue =  OVRInput.GetDown(OVRInput.RawButton.Start);
		}
		
		if ((button == "a") || (button == "A")) 
		{
			returnValue = OVRInput.GetDown(OVRInput.RawButton.A);
		}
		
		if ((button == "b") || (button == "B"))
		{
			returnValue = OVRInput.GetDown(OVRInput.RawButton.B);
		}
		
		if ((button == "x") || (button == "X"))
		{
			returnValue = OVRInput.GetDown(OVRInput.RawButton.X);
		}
		
		if ((button == "y") || (button == "Y"))
		{
			returnValue = OVRInput.GetDown(OVRInput.RawButton.Y);
		}

		if (button == "xAxis")
		{
			returnValue = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight);
		}
		
		if (button == "yAxis")
		{
			returnValue = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown);
		}
		#endif
		return returnValue;
	}


	bool oculusGetButtonReleased(string button)
	{
		bool returnValue = false;
		#if ((UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_PS4))
		
		if (button == "start")
		{
			returnValue = OVRInput.GetUp(OVRInput.RawButton.Start);
		}
		#endif
		return returnValue;
	}


	float oculusGetAxis(string axis)
	{
		float returnValue = 0f;
#if ((UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_PS4))
		
		if (axis == "xAxis")
		{
			Vector2 primaryJoystick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
			returnValue = primaryJoystick.x;
		}
		
		if (axis == "yAxis")
		{
			Vector2 primaryJoystick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
			returnValue = primaryJoystick.y;
		}
		
		if (axis == "mouseX")
		{
			Vector2 secondaryJoystick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
			returnValue = secondaryJoystick.x;
		}
		
		if (axis == "mouseY")
		{
			Vector2 secondaryJoystick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
			returnValue = secondaryJoystick.y;
		}
		
		if (axis == "dPadX")
		{
			Vector2 primaryJoystick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
			returnValue = primaryJoystick.x;
		}
		
		if (axis == "dPadY")
		{
			Vector2 primaryJoystick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
			returnValue = primaryJoystick.y;
		}
		
		if (axis == "leftTrigger")
		{
			if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))  //input manager needs trigger info as a float. Vive returns a bool at this point. This block converts bool to float.
			{ 
				returnValue = 1f;
			}
			
			else
			{
				returnValue = 0f;
			}
		}
		
		if (axis == "rightTrigger")
		{
			if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))		//input manager needs trigger info as a float. Vive returns a bool at this point. This block converts bool to float.
			{ 
				returnValue = 1f;
			}
			
			else
			{
				returnValue = 0f;
			}
		}
#endif
        return returnValue;
	}

	Vector3 oculusGetPosition(string motionController)
	{
		Vector3 returnValue = new Vector3 (0f,0f,0f);
		
		#if ((UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_PS4))
		
		if ((motionController == "left") || (motionController == "Left"))
		{
			returnValue = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		}

		if ((motionController == "right") || (motionController == "Right"))
		{
			returnValue = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
		}

		#endif
		return returnValue;
	}

	Vector3 oculusGetRotation(string motionController)
	{
		Vector3 returnValue = new Vector3 (0f,0f,0f);
		#if ((UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_PS4))
		
		if ((motionController == "left") || (motionController == "Left"))
		{
			returnValue = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch).eulerAngles;
		}
		
		if ((motionController == "right") || (motionController == "Right"))
		{
			returnValue = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles;
		}
		#endif
		return returnValue;
	}


#if UNITY_STANDALONE || UNITY_EDITOR
	// because the vive controllers can become active at any time, not just at start, 
	//these coroutines are needed to "catch" the activation of the controller objects
	//Because of the way the objects work, Making static variables on the controller scripts
	//to be read here in the input manager also doesnt work properly. 
	//the best way to read those values is by referencing those objects here
	
	IEnumerator findLeftViveController()
	{
		float startTime = Time.time;
		findingLeftVive = true;
		
		// if this coroutine hasn't found the  vive controllers within 3 seconds, stop looking
		while (leftViveController == null)	
		{ 
			if (Time.time >= startTime + 3) 
			{
				StartCoroutine ("activateViveControllerFailsafe");
			}
			
			leftViveController = GameObject.Find("FPS_Character_Controller_ENYX/Vive_Controllers/Vive Controller (left)");
			
			yield return null;
		}
		
		leftViveScript = leftViveController.GetComponent<Enyx_Vive_Controllers> ();
		
		findingLeftVive = false;
		
		if (leftViveController.activeSelf == false) 
		{
			StartCoroutine(activateViveControllerFailsafe());
		}
		
		Debug.Log("left vive found");
	}


	IEnumerator findRightViveController()
	{
		float startTime = Time.time;
		findingRightVive = true;
		while (rightViveController == null) 
		{
			if (Time.time >= startTime + 3) 
			{
				StartCoroutine ("activateViveControllerFailsafe");
			}
			
			rightViveController = GameObject.Find("FPS_Character_Controller_ENYX/Vive_Controllers/Vive Controller (right)");
			yield return null;
		}
		
		rightViveScript = rightViveController.GetComponent<Enyx_Vive_Controllers> ();
		
		findingRightVive = false;
		
		if (rightViveController.activeSelf == false) 
		{
			StartCoroutine("activateViveControllerFailsafe");
		}
		
		Debug.Log("right vive found");
	}

	// if vive controllers can't be found, this function reactivates joystick so we dont lose all movement
	// also sends up a notification variable for a couple seconds to alert any listening objects (usually menu)
	// that vive controllers failed
	bool failsafeActivated = false;

	IEnumerator activateViveControllerFailsafe(){
		if (failsafeActivated == false)
		{
			failsafeActivated = true;
			Debug.Log("Vive controllers not found. Failsafe activated");
			StopCoroutine("findLeftViveController"); // stops active searching coroutines
			StopCoroutine("findRightViveController");
			findingLeftVive = false; //allows us to tell elsewhere that these coroutines are not running
			findingRightVive = false;
			useJoystick = true;// enabled the joystick again so controlls will not be lost completely
			failedToFindViveControllers = true;  //send up a flag to listening objects telling them that vive controllers failed

			// you can't yield wait for seconds when the game is paused, so this is a hacky way of pausing for a period of time without relying on timescale
			for (int i = 0; i< 120; i++)
			{ 
				yield return null;
			}
			
			failedToFindViveControllers = false; //take down flag.
			failsafeActivated = false;
		}
	}


#endif

#if UNITY_PS4
	IEnumerator findDualShock()
	{
		while (dualShock == null) 
		{
			dualShock = GameObject.Find ("DualShock 4");
			yield return null;
		}
	}
#endif

    public void findTrackedPS4Controller(){

		#if UNITY_PS4 
		StartCoroutine(findDualShock ());
		#endif
	}


}//end script



