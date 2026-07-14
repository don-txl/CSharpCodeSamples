/*This script is our insanity meter. It adjusts our sanity up and down when it enters triggers with the Insanity_Zone tag.
 * When the sanity reaches a level determined by the trigger, an FMOD event will play.
 * This script should always start off disabled since we are using OnTriggerStay. OnTriggerStay doesn't seem to want to play FMOD events properly
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
using UnityStandardAssets;
using UnityEngine.VR;
using FMODUnity;

namespace Sanity
{

    public class Insanity_Manager_ENYX : MonoBehaviour
    {
        public static float currentSanity;
        [Tooltip("This is the maximum amount the sanity can rise to")]
        public float maxSanity;                 //The maximum amount the sanity can go ex. 100
        [Tooltip("This is the minumum amount the sanity can drop to")]
        public float minSanity;                 //The minimum amount the sanity can go ex. 0
        public static bool inTrigger;           //This is used to determine whether the sanity level should go up or down
        public static float decreaseSanity;     //This gets set by our triggers. it is how much the sanity goes down per second.
        public static float increaseSanity;     //This gets set by our triggers. it is how much the sanity goes up per second.
        public static float beginPlayingAudio;  //This gets set by our triggers. it determiens at what point the FMOD Event should begin playing.

        void Update()
        {
                       
            if (currentSanity >= 0)
            {
                if (!inTrigger)
                {
                    currentSanity -= decreaseSanity * Time.deltaTime;
                }
            }

            if (currentSanity >= 100)
            {
                currentSanity = maxSanity;
            }

            if (currentSanity <= 0)
            {
                currentSanity = minSanity;
            }

            if (currentSanity < beginPlayingAudio)
            {
                
                Insanity_FMOD_Player_ENYX audio = gameObject.GetComponent<Insanity_FMOD_Player_ENYX>();
                audio.enabled = false;
            }


           // Debug.Log("Increase Amount =" + currentSanity);

        }

        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Insanity_Zone")
            {
                inTrigger = true;
                currentSanity += increaseSanity * Time.deltaTime;

                if (currentSanity > beginPlayingAudio)
                {
                    Insanity_FMOD_Player_ENYX audio = gameObject.GetComponent<Insanity_FMOD_Player_ENYX>();
                    audio.enabled = true;
                }

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Insanity_Zone")
            {
                inTrigger = false;
            }
        }

    }
}

