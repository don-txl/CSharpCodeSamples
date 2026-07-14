using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour {
    [Header("OneShot Sound Event Name")]
    [FMODUnity.EventRef]
    public string fmodEvent;
    FMOD.Studio.EventInstance oneShotEvent;
  
    void OnEnable()
    {
        oneShotEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        oneShotEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        play("Intensity", 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void play(string param = "", float val = 0.0f)
    {
        if (fmodEvent != "")
        {
            if (param == "")
            {
                oneShotEvent.start();
            }
            else
            {
                oneShotEvent.setParameterValue(param, val);
                oneShotEvent.start();
            }
        }
    }
}
