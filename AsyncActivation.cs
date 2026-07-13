using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncActivation : MonoBehaviour
{

    public float timeToLoad = 1f;           //The amount of time in seconds to wait before activating mainGame and deactivting tempCam

    void Start()
    {
        
        StartCoroutine(deactivateTempCam());
    }

    //This activates the mainGame Game Object and disables the tempCam Game Object
    IEnumerator deactivateTempCam()
    {
        yield return new WaitForSeconds(timeToLoad);

        // Search the scene for a GameObject named "GameRoot".
        GameObject gameRoot = GameObject.Find("GameRoot");

        // Make sure GameRoot was found before trying to use it.
        if (gameRoot == null)
        {
            Debug.LogError("AsyncActivation: Could not find 'GameRoot' in the scene.");
            yield break;   // Stop the coroutine because we can't continue.
        }

        // Look for a child object named "Game" underneath GameRoot.
        Transform gameTransform = gameRoot.transform.Find("Game");
        
        // Verify that the child object exists.
        if (gameTransform == null)
        {
            Debug.LogError("AsyncActivation: 'GameRoot' does not contain a child named 'Game'.");
            yield break;   // Stop the coroutine because the required object is missing.
        }

        // Convert the Transform reference into its corresponding GameObject. This is needed so we can use SetActive
        GameObject mainGame = gameTransform.gameObject;

        // Search the scene for the temporary loading camera.
        GameObject tempCam = GameObject.Find("TempCamera");

        // If the temporary camera isn't found, log a warning.
        // The game can still continue even if the camera doesn't exist.
        if (tempCam == null)
        {
            Debug.LogWarning("AsyncActivation: Could not find 'TempCamera'. Continuing without disabling it.");
        }

        yield return new WaitForSeconds(timeToLoad);

        // Enable the main game object.
        mainGame.SetActive(true);

        // Disable the temporary camera if it exists.
        if (tempCam != null)
        {
            tempCam.SetActive(false);
        }
        
    }
}
