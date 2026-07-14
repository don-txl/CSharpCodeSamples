/*  This script plays an FMOD Event when activated by our Insanity Meter
 *  This script should always start off disabled since we are using OnTriggerStay. OnTriggerStay doesn't seem to want to play FMOD events properly
 *  When the Insanity meter reaches the determined value, this script will become active and play the FMOD sound.
 *  
 *  The Insanity Meter is compromised of 3 scripts:
 *  1.) Insanity_FMOD_Player: Plays the FMOD sound when activated
 *  2.) Insanity Manager: Controls what value the sanity is at and activates the appropriate FMOD Player script when audio is needed.
 *  3.) Sanity_Trigger: A trigger that gets set throughout the environment. The Insanity_Trigger tells the Insanity_Manager how much insanity to apply and how fast.
 * 
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using UnityStandardAssets.Characters.FirstPerson;

public class Insanity_FMOD_Player_ENYX : MonoBehaviour {

	[Space(20)]
	[Header("OneShot Sound Event Name")]
	//[FMODUnity.EventRef] 											
	public static string fmodEvent;
	[Tooltip("This will alow you to set the parameter at runtime. Useful to find the right setting, but not as optimised for final build.")]
	public bool debugParameter = false;
    [Tooltip("This is the parameter name used in the FMOD Event")]
    public static string fmodParameter = "Probability";
    [Tooltip("This is the parameter value of the FMOD Event")]
    public static float parameterValue = 50f;

    FMOD.Studio.EventInstance oneShotEvent; 

	// Use this for initialization
	void OnEnable () {

        if (fmodEvent != "")
        {
            GameObject settingsMNGR = GameObject.Find("Managers/Settings_Manager_Singleton");
            SettingsManager_Singleton sttingsScript = settingsMNGR.GetComponent<SettingsManager_Singleton>();
            if (sttingsScript != null)
            {
                if (sttingsScript.vrEnabled)
                {
                    #if UNITY_STANDALONE
                    oneShotEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEvent + "_PCVR");
                    #endif

                    #if UNITY_PS4
                    oneShotEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEvent + "_PSVR");
                    #endif
                }

                else
                {
                    oneShotEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
                }
            }

            

            //oneShotEvent = FMODUnity.RuntimeManager.CreateInstance (fmodEvent);
			oneShotEvent.set3DAttributes (FMODUnity.RuntimeUtils.To3DAttributes (gameObject));
			setFMODParam (fmodParameter, parameterValue);

			playSound ();
		}
	}

	void Update()
    {
		oneShotEvent.set3DAttributes (FMODUnity.RuntimeUtils.To3DAttributes (gameObject));

        if (debugParameter)
        {
			setFMODParam (fmodParameter, parameterValue);
		}
	}

	void OnDisable()
    {
		oneShotEvent.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
		oneShotEvent.release ();
	}

	//allows the user to set parameters to prepare an event for playback
	//you can one parameter directly within the playSound() function. However,
	//this is useful if there are a lot of parameters to set before playback
	public void setFMODParam(string param, float val)
	{
		oneShotEvent.setParameterValue (param,val);
	}


	//Plays an fmod sounds based on the fmodEvent Path 
	//can take a parameter. String = fmod paramter, Float = value to set
	//this is useful for certain events that require a parameter to select a sound i.e footsteps
	public void playSound(string param = "", float val = 0.0f)
	{
		if (fmodEvent != "")
		{
			if (param == "")
            {
				oneShotEvent.start ();
			}

            else
            {
				oneShotEvent.setParameterValue (param,val);
				oneShotEvent.start ();
			}
		}
	}
}
