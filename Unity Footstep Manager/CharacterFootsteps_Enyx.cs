/* 
This script should be attached on the main root of the character, 
on the GameObject the Rigidbody / CharacterController script is attached.

The purpose of this script is to gather data from the ground below the character and use the
data to find a user-defined sound for the type of ground found.
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

namespace Footsteps 
{
	public enum TriggeredBy 
	{
		COLLISION_DETECTION,	// The footstep sound will be played when the physical foot collides with the ground.
		TRAVELED_DISTANCE		// The footstep sound will be played after the character has traveled a certain distance
	}

	public enum ControllerType 
	{
		RIGIDBODY,
		CHARACTER_CONTROLLER
	}

	public class CharacterFootsteps_Enyx : MonoBehaviour 
	{ 

		[Tooltip("The method of triggering footsteps.")]
		[SerializeField] TriggeredBy triggeredBy;

		[Tooltip("This is used to determine what distance has to be traveled in order to play the footstep sound.")]
		[SerializeField] float distanceBetweenSteps = 1.8f;

		[Tooltip("To know how much the character moved, a reference to a rigidbody / character controller is needed.")]
		[SerializeField] ControllerType controllerType;
		[SerializeField] Rigidbody characterRigidbody;
		[SerializeField] CharacterController characterController;

		[Tooltip("If this is enabled, you can see how far the script will check for ground, and the radius of the check.")]
		[SerializeField] bool debugMode = true;

		[Tooltip("How high, relative to the character's pivot point the start of the ray is.")]
		public float groundCheckHeight = 0.5f;

		[Tooltip("What is the radius of the ray.")]
		[SerializeField] float groundCheckRadius = 0.5f;

		[Tooltip("How far the ray is casted.")]
		[SerializeField] float groundCheckDistance = 0.3f;

		[Tooltip("What are the layers that should be taken into account when checking for ground.")]
		[SerializeField] LayerMask groundLayers;

		Transform thisTransform;
		RaycastHit currentGroundInfo;
		float stepCycleProgress;
		float lastPlayTime;
		bool previouslyGrounded;
		bool isGrounded;

		FMOD.Studio.EventInstance oneShotEvent; //fmod player instance

        private bool hasStarted = false; //since our start function is delayed this bool is used to tell our update when to continue


        IEnumerator Start() 
		{

            yield return null;
            yield return null;

			if (SurfaceManager_Enyx.singleton.getFmodFootstepPath () != "") 
			{
				oneShotEvent = FMODUnity.RuntimeManager.CreateInstance (SurfaceManager_Enyx.singleton.getFmodFootstepPath ());
				oneShotEvent.set3DAttributes (FMODUnity.RuntimeUtils.To3DAttributes (gameObject));
			}

			if(groundLayers.value == 0) 
			{
				groundLayers = 1;
			}

			thisTransform = transform;
			string errorMessage = "";

			if (triggeredBy == TriggeredBy.TRAVELED_DISTANCE && !characterRigidbody && !characterController)
			{
				errorMessage = "Please assign a Rigidbody or CharacterController component in the inspector, footsteps cannot be played";
			}

			else if (!FindObjectOfType<SurfaceManager_Enyx>())
			{
				errorMessage = "Please create a Footstep Database, otherwise footsteps cannot be played, you can create a database" + " by clicking 'FootstepsCreator' in the main menu";
			}

			if(errorMessage != "") 
			{
				Debug.LogError(errorMessage);
				enabled = false;
			}

            hasStarted = true;
		}

		void Update() {

            if (!hasStarted) 
			{
                return;
            }

			CheckGround();

			if(triggeredBy == TriggeredBy.TRAVELED_DISTANCE) 
			{
				float speed = (characterController ? characterController.velocity : characterRigidbody.velocity).magnitude;

				if (isGrounded) 
				{
					// Advance the step cycle only if the character is grounded.
					AdvanceStepCycle (speed * Time.deltaTime);
				} 
				
				else 
				{
					//Debug.Log ("not grounded");
				}
			}
		}

		public void TryPlayFootstep() 
		{
			if(isGrounded) 
			{
				PlayFootstep();
				//Debug.Log ("Trying to play footstep");
			}
		}

		//----------------------------------------------------------------changed to use fmod
		void PlayLandSound() 
		{
			PlayFootstep ();  //it ended up that playing the land sound would be the same function. Rather than finding every piece of code that referenced this func, I just call PlayFootstep() from here.
		}



		void AdvanceStepCycle(float increment) 
		{
			stepCycleProgress += increment;

			if(stepCycleProgress > distanceBetweenSteps) 
			{
				stepCycleProgress = 0f;
				PlayFootstep();
			}
		}



		//-----------------------------------------------------------------Changed to use fmod
		void PlayFootstep() 
		{
			float randomFootstep = SurfaceManager_Enyx.singleton.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point);

			if(randomFootstep != -1f) 
			{
				//Debug.Log("selected sound = " + randomFootstep);
				oneShotEvent.setParameterValue (SurfaceManager_Enyx.singleton.getFmodParamName(),randomFootstep);
				oneShotEvent.start ();
			}
		}

		void OnDrawGizmos() 
		{
			if(debugMode) 
			{
				Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
				Gizmos.color = Color.red;
				Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
			}
		}

		void CheckGround() 
		{
			previouslyGrounded = isGrounded;
			Ray ray = new Ray(thisTransform.position + Vector3.up * groundCheckHeight, Vector3.down);

			if(Physics.SphereCast(ray, groundCheckRadius, out currentGroundInfo, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore)) 
			{
				isGrounded = true;
			}
			
			else 
			{
				isGrounded = false;
			}

			if(!previouslyGrounded && isGrounded) 
			{
				PlayLandSound();
			}
		}
	}
}
