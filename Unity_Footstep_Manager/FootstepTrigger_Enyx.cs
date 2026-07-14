// This script is a trigger that is added to an object to cause the footstep sounds to play using FMOD
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

namespace Footsteps 
{											
	[RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class FootstepTrigger_Enyx : MonoBehaviour 
	{
		Collider thisCollider;
		CharacterFootsteps_Enyx footsteps;


		void Start() 
		{
			thisCollider = GetComponent<Collider>();
			footsteps = GetComponentInParent<CharacterFootsteps_Enyx>();
			Rigidbody thisRigidbody = GetComponent<Rigidbody>();

			if(thisCollider) 
			{
				thisCollider.isTrigger = true;
				SetCollisions();
			}

			if(thisRigidbody) thisRigidbody.isKinematic = true;

			string errorMessage = "";

			if(!footsteps) errorMessage = "No 'CharacterFootsteps' script found as a parent, this footstep trigger will not work";
			else if(!thisCollider) errorMessage = "Please attach a collider marked as a trigger to this gameobject, this footstep trigger will not work";
			else if(!thisRigidbody) errorMessage = "Please attach a rigidbody to this gameobject, this footstep trigger will not work";

			if(errorMessage != "") 
			{
				Debug.LogError(errorMessage);
				enabled = false;

				return;
			}
		}

		void OnEnable() 
		{
			SetCollisions();
		}

		void OnTriggerEnter(Collider other) 
		{
			Debug.Log ("entered the trigger");
			if(footsteps) 
			{
				footsteps.TryPlayFootstep();
				print ("trying to play footstep");
			}
		}

		void SetCollisions() 
		{
			if(!footsteps) return;

			Collider[] allColliders = footsteps.GetComponentsInChildren<Collider>();

			foreach(var collider in allColliders) 
			{
				if(collider != GetComponent<Collider>()) 
				{
					Physics.IgnoreCollision(thisCollider, collider);
				}
			}
		}
	}
}
