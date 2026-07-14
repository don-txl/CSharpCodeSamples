//////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Settings Script                                                                                     //
//  Description: This script sets variables in the Character Controller                                 //
//  Written by: Don Hileman                                                                             //
// Copyright Enyx Studios, LLC                                                                          //
// Written On: Novemenr 30, 2016                                                                        //
//////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEngine.VR;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class SettingsManager_Singleton : MonoBehaviour
    {
        public bool vrEnabled;
        public float brightness;
        public static bool setVREnabled;
        [Space(15)]
        public string connectedHeadset;

        [Space(15)]
        public static string setConnectedHeadset;

        [Space(15)]
        [Tooltip("Setting this to true freezes all movements")]
        public bool movementsFrozen = false;
        public static bool setMovementsFrozen = false;

        
        [Header("** Non-VR Settings **")]

        // Use these varaibles for setting varibales in other scripts
        public static float setPlayerWalkSpeed_NonVR;
        public static float setPlayerMaxWalkSpeed_NonVR;
        public static float setPlayerStairWalkSpeed_NonVR;
        public static float setPlayerWalkAcceleration_NonVR;
        public static float setPlayerAccelerationTime_NonVR;
        public static float setPlayerRunSpeed_NonVR;

        // Use these variables for getting values from the Inspector Window
        public float playerWalkSpeed_NonVR = 2.5f;
        public float playerMaxWalkSpeed_NonVR = 2.5f;
        public float playerStairWalkSpeed_NonVR = 1.5f;
        public float playerWalkAcceleration_NonVR = 0.5f;
        public float playerAccelerationTime_NonVR = 2.5f;
        public float playerRunSpeed_NonVR =4f;


        // Use these varaibles for setting varibales in other scripts
        public static float setXSensitivity_NonVR = 4f;
        public static float setXSensitivityIncrease_NonVR = .4f;
        public static float setXSensRamp_NonVR = 1f;
        public static float setXSensInit_NonVR = .8f;
        public static float setYSensitivity_NonVR = 2f;
        public static float setMinimumX_NonVR = -90F;
        public static float setMaximumX_NonVR = 90F;
        public static float setsmoothTime_NonVR = 5f;

        // Use these variables for getting values from the Inspector Window
        [Header("** Mouse Sensitivity **")]
        [Space(20)]
        public float XSensitivity_NonVR = 4f;
		[Tooltip("Increase is used as a multiplier used to increase speed over the course of time. Smaller numbers ramp up slower")]
        public float XSensitivityIncrease_NonVR = .4f;
        public float XSensRamp_NonVR = 1f;
        public float XSensInit_NonVR = .8f;
        public float YSensitivity_NonVR = 2f;
        public float MinimumX_NonVR = -90F;
        public float MaximumX_NonVR = 90F;
        public float smoothTime_NonVR = 5f;

        // Use these varaibles for setting varibales in other scripts
        public static float setPlayerWalkSpeed_VR;
        public static float setPlayerMaxWalkSpeed_VR;
        public static float setPlayerStairWalkSpeed_VR;
        public static float setPlayerWalkAcceleration_VR;
        public static float setPlayerAccelerationTime_VR;
        public static float setPlayerRunSpeed_VR;

        // Use these variables for getting values from the Inspector Window
        [Header("** VR Settings **")]
        [Space(20)]
        public float playerWalkSpeed_VR = 2.4f;
        public float playerMaxWalkSpeed_VR = 2.4f;
        public float playerStairWalkSpeed_VR = 1.5f;
        public float playerWalkAcceleration_VR = 0.5f;
        public float playerAccelerationTime_VR = 2.5f;
        public float playerRunSpeed_VR = 3f;

        // Use these varaibles for setting varibales in other scripts
        public static float setXSensitivity_VR = .1f;
		[Tooltip("Increase is used as a multiplier used to increase speed over the course of time. Smaller numbers ramp up slower")]
        public static float setXSensitivityIncrease_VR = .2f;
        public static float setXSensRamp_VR = .01f;
        public static float setXSensInit_VR = .1f;
        public static float setYSensitivity_VR = .5f;
        public static float setMinimumX_VR = -90F;
        public static float setMaximumX_VR = 90F;
        public static float setsmoothTime_VR = 5f;

        // Use these variables for getting values from the Inspector Window
        [Header("** VR Mouse Sensitivity **")]
        [Space(20)]
        public float XSensitivity_VR = 1f;
        public float XSensitivityIncrease_VR = .2f;
		[Tooltip("Ramps are a multiplier used to increase speed over the course of time. Smaller numbers ramp up slower")]
        public float XSensRamp_VR = .01f;
        public float XSensInit_VR = .1f;
        public float YSensitivity_VR = 1f;
        public float MinimumX_VR = -90F;
        public float MaximumX_VR = 90F;
        public float smoothTime_VR = 5f;

        void Awake ()
        {
            setVREnabled = VRSettings.enabled;
        }

        void Start()
        {
            connectedHeadset = VRSettings.loadedDeviceName;
            setConnectedHeadset = connectedHeadset;
            setMovementsFrozen = movementsFrozen;
            updateStaticVars();
   
        }

        

        void GetInitNonVrSettings()
        {
            //setVREnabled = vrEnabled;               //Don changed this to line below
            setVREnabled = VRSettings.enabled;
            setPlayerWalkSpeed_NonVR = playerWalkSpeed_NonVR;
            setPlayerMaxWalkSpeed_NonVR = playerMaxWalkSpeed_NonVR;
            setPlayerStairWalkSpeed_NonVR = playerStairWalkSpeed_NonVR;
            setPlayerWalkAcceleration_NonVR = playerWalkAcceleration_NonVR;
            setPlayerAccelerationTime_NonVR = playerAccelerationTime_NonVR;
            setPlayerRunSpeed_NonVR = playerRunSpeed_NonVR;


            setXSensitivity_NonVR = XSensitivity_NonVR;
            setXSensitivityIncrease_NonVR = XSensitivityIncrease_NonVR;
            setXSensRamp_NonVR = XSensRamp_NonVR;
            setXSensInit_NonVR = XSensInit_NonVR;
            setYSensitivity_NonVR = YSensitivity_NonVR;
            setMinimumX_NonVR = MinimumX_NonVR;
            setMaximumX_NonVR = MaximumX_NonVR;
            setsmoothTime_NonVR = smoothTime_NonVR;

            setMovementsFrozen = movementsFrozen;

        }

        void GetInitVrSettings()
        {
            //setVREnabled = vrEnabled;                   //Don changed this to line below
            setVREnabled = VRSettings.enabled;
            setPlayerWalkSpeed_VR = playerWalkSpeed_VR;
            setPlayerMaxWalkSpeed_VR = playerMaxWalkSpeed_VR;
            setPlayerStairWalkSpeed_VR = playerStairWalkSpeed_VR;
            setPlayerWalkAcceleration_VR = playerWalkAcceleration_VR;
            setPlayerAccelerationTime_VR = playerAccelerationTime_VR;
            setPlayerRunSpeed_VR = playerRunSpeed_VR;


            setXSensitivity_VR = XSensitivity_VR;
            setXSensitivityIncrease_VR = XSensitivityIncrease_VR;
            setXSensRamp_VR = XSensRamp_VR;
            setXSensInit_VR = XSensInit_VR;
            setYSensitivity_VR = YSensitivity_VR;
            setMinimumX_VR = MinimumX_VR;
            setMaximumX_VR = MaximumX_VR;
            setsmoothTime_VR = smoothTime_VR;
          
            setMovementsFrozen = movementsFrozen;

        }



        //public function to update the state of the vr through our vr manager
       public void TurnVrOn(bool state)
        {

            vrEnabled = state;
            //setVREnabled = vrEnabled;       //Don changed this to line below
            setVREnabled = VRSettings.enabled;
            //updateStaticVars();    
            updateEnyxControllers();
        }


        // this function allows the movements to be frozen mid game
        // the input manager reads these values to know whether or not to allow movement 
        public void freezeMovements(bool state)
        {
            if (state)
            {
                movementsFrozen = true;
                setMovementsFrozen = true;
            }
            else
            {
                movementsFrozen = false;
                setMovementsFrozen = false;
            }
        }


        // the menu system is setting the settings manager's public variables. However, the character controller etc is pulling its values from the corresponding static variables
        // this function updates the static variables to be the same as the public variables. call this function after making a change to the globals outside this script.
        public void updateStaticVars()
        {
            GetInitNonVrSettings();
            GetInitVrSettings();
            updateEnyxControllers();
        }



        public void updateEnyxControllers() {
            StartCoroutine(updtEnyxControllers());
        }


        //THIS IS A PROBLEM AREA BECAUSE FPS CONTROLLER IS NOT ALWAYS PRESENT WHEN MANAGERS ARE


        // if the static vars have been updated, we call this function to force an update on our fps controller and everything else that needs it
        IEnumerator updtEnyxControllers()
        {
            yield return null;
            
            GameObject enyxController = GameObject.Find("FPS_Character_Controller_ENYX/Character_Controller");
            if (enyxController != null)
            {
                FirstPersonController_Enyx fpsScript = enyxController.GetComponent<FirstPersonController_Enyx>();
                if (VRSettings.enabled)
                {
                    fpsScript.SetVRInitSettings();
                    fpsScript.updateMouseLook();


                }
                else
                {
                    fpsScript.SetNonVRInitSettings();
                    fpsScript.updateMouseLook();

                }
                
            }

    
        }






    }//end class

}//end namestate

