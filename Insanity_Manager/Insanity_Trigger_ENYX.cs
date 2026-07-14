/*This script is is used to begin our Insanity Meter going up
 * This script can set the amount the sanity goes up, how fast, and at what point the FMOD Event should start playing.
 * This script is also used to set the FMOD Event, Parameter, and Value of the Paramter. This allows different triggers to use different Events.
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
using Sanity;
using FMODUnity;



    public class Insanity_Trigger_ENYX : MonoBehaviour
    {
        //[Space(20)]
        [Header("Sanity Meter Timer Settings")]
        [Tooltip("This sets how much the sanity meter goes up per second")]
        public float insanityAmount;
        [Tooltip("This is at what number on the sanity meter does the sounds begin playing")]
        public float beginPlayingAt;                            // This is the value to begin playing whispers

        [Tooltip("Here we set the FMOD Event to use for this trigger")]   
        [FMODUnity.EventRef]
        public string fmodEventName;
        [Tooltip("Here we set the FMOD Event Parameter Name to use for this trigger")]
        public string fmodParamName;
        [Tooltip("Here we set the FMOD Paramter Value to use for this trigger")]
        public float fmodParamValue;

        public float insanityCoolDownAmount = 5;

        // Use this for initialization
        void OnEnable()
        {

            GameObject sanityMngr = GameObject.Find("FPS_Character_Controller_ENYX/Character_Controller/Sanity_Manager_ENYX");

            if (sanityMngr != null)
            {
                Insanity_Manager_ENYX scrpt = sanityMngr.GetComponent<Insanity_Manager_ENYX>();
            }


        }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Insanity_Manager_ENYX.increaseSanity = insanityAmount;
            Insanity_Manager_ENYX.decreaseSanity = insanityCoolDownAmount;
            Insanity_Manager_ENYX.beginPlayingAudio = beginPlayingAt;

            Insanity_FMOD_Player_ENYX audio = gameObject.GetComponent<Insanity_FMOD_Player_ENYX>();
            Insanity_FMOD_Player_ENYX.fmodEvent = fmodEventName;
            Insanity_FMOD_Player_ENYX.fmodParameter = fmodParamName;
            Insanity_FMOD_Player_ENYX.parameterValue = fmodParamValue;
        }

    }
}
