//This script is used to read grip and trigger values from controllers. Those values are then sued to set the floats in the Animator so it can play the correct animations.
//Attach this script to the root of the hand Prefab that has the hand models.

//Written by Don Hileman July 13, 2026

using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    [Tooltip("Set this to an input that measures the Grip button value for your controller. Can use Unity's XRI Right Interaction/Select Value or Left Interaction/Select Value depending on which hand this Component is attahced to")]
    [SerializeField] private InputActionReference gripActionReference;

    [Tooltip("Set this to an input that measures the Trigger button value for your controller. Can use Unity's XRI Right Activate Value or XRI Left Activate Value depending on which hand this Component is attahced to")]
    [SerializeField] private InputActionReference TriggerActionReference;

    private Animator animator;

    //Here we are creating avariable for the float names in the animator
    private string gripActionName = "Grip";
    private string triggerActionName = "Pinch";



    //Here we get a reference to the Animator Component
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
            return; //Exit if animatgor is not set
        }

        //Getting the float value of the controller grip when pressed
        float gripValue = gripActionReference.action.ReadValue<float>();

        //Getting the float value of the controller trigger when pressed
        float triggerValue = TriggerActionReference.action.ReadValue<float>();

        //Setting the Animator's Grip float value from the controller's grip value when pressed
        animator.SetFloat(gripActionName, gripValue);
        
        //Setting the Animator's Pinch float value from the controller's trigger value when pressed
        animator.SetFloat(triggerActionName, triggerValue);
    }
}
