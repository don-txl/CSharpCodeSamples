using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Scene_Handler_Don : MonoBehaviour {


	private int fadeTimes = 0;                  // Tracks how many times the camera fade has occurred.
    public GameObject vrTutorialCanvas = null;  // Reference to the VR tutorial UI canvas.
    public GameObject autosaveCanvas = null;    // Reference to the autosave notification canvas.
    public GameObject fadeCanvas = null;        // Reference to the full-screen fade canvas used for fade-in/fade-out effects.


    public void fadeCamera()
    {
		StartCoroutine (fadeCameraSequence ());
	}


    // Performs the camera fade sequence.
    IEnumerator fadeCameraSequence ()
    {
        // Check if this is the first time the fade sequence has been run.
        if (fadeTimes == 0)
        {
			fadeCanvas.SetActive (true);
			yield return new WaitForSeconds (1.5f);
			fadeCanvas.SetActive (false);
			vrTutorialCanvas.SetActive (false);
			autosaveCanvas.SetActive (true);
			fadeTimes++;
		}

        // For all future calls, simply enable the fade canvas.
        else
        {
			fadeCanvas.SetActive (true);
		}
	}

}
