using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using Colorful;
using UnityEngine.VR;

public class PauseMenu_Manager : MonoBehaviour
{
    public GameObject pausePanel;
    [Header("Offset Settings")]
    [Tooltip("How much vertical offset should the canvas be placed in relation to the main camera?")]
    public float heightOffset = 0.0f;
    [Tooltip("When placing the canvas, how much to multiply the forward vector's position by? (1 = Use the forward vector as is)")]
    public float forwardFactor = 1.0f;

    [Header("----------Main Panel----------")]
    [Tooltip("This should be set to the Options Canvas")]
    public GameObject mainPauseCanvas;                 //Options Menu Panel
    public Button pauseButtonToHighlight;             //This is the object that will get highlighted when the above panel is active
    public Button pauseOptionsButton;                 //We have this so the other panels know what to highlight when Back Button is pressed
    //public Button mainAudioButton;                   //We have this so the other panels know what to highlight when Back Button is pressed
    //public Button mainControllsButton;               //We have this so the other panels know what to highlight when Back Button is pressed

    [Header("----------Options Menu----------")]
    [Tooltip("This should be set to the Options Canvas")]
    public GameObject optionsMenuCanvas;                 //Options Menu Panel
    public Button optionsButtonToHighlight;             //This is the object that will get highlighted when the above panel is active
    public Button optionsDisplayButton;                 //We have this so the other panels know what to highlight when Back Button is pressed
    public Button optionsAudioButton;                   //We have this so the other panels know what to highlight when Back Button is pressed
    public Button optionsControllsButton;               //We have this so the other panels know what to highlight when Back Button is pressed

    [Header("----------Audio Menu----------")]
    [Tooltip("This should be set to the Audio Canvas")]
    public GameObject audioMenuCanvas;                      //Main Menu Audio Panel
    public Slider audioSliderToHighlight;                   //This is the object that will get highlighted when the above panel is active
    [Tooltip("these are the audio sliders from the audio canvas")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider dialogueVolumeSlider;


    [Header("----------Display Menu----------")]
    [Tooltip("This should be set to the Display Canvas")]
    public GameObject displayMenuCanvas;                    //Main Menu Audio Panel
    public Slider VRDisplaySliderToHighlight;                 //This is the object that will get highlighted when the above panel is active
    public Slider nonVRDisplaySliderToHighlight;                 //This is the object that will get highlighted when the above panel is active
    //public Slider nonVRbrightnessSlider;
    //public Slider VRbrightnessSlider;
    public GameObject VRDisplayPanel;
    public GameObject nonVRDisplayPanel;


    [Header("----------Controls Menu----------")]
    [Tooltip("This should be set to the Display Canvas")]
    public GameObject controllsMenuCanvas;                   //Main Menu Audio Panel

    [Header("     VR Settings")]
    public Toggle VRControllsSliderToHighlight;               //This is the object that will get highlighted when the above panel is active
    public GameObject VRMovementsPanel;
    public Slider VRWalkSpeedSlider;
    public Slider VRRunSpeedSlider;
    public Slider VRTurnSpeedSlider;
    public float VRWalkSpeedSlowPreset = 2.4f;
    public float VRRunSpeedSlowPreset = 3f;
    public float VRTurnSpeedSlowPreset = 1f;
    public float VRWalkSpeedModeratePreset = 3f;
    public float VRRunSpeedModeratePreset = 4f;
    public float VRTurnSpeedModeratePreset = 1.1f;
    public float VRWalkSpeedFastPreset = 3.6f;
    public float VRRunSpeedFastPreset = 5f;
    public float VRTurnSpeedFastPreset = 1.2f;
    public GameObject VRWalkSpeedCurrentValue;
    public GameObject VRRunspeedCurrentValue;
    public GameObject VRTurnSpeedCurrentValue;


    [Header("     Non VR Settings")]
    public Toggle nonVRControllsSliderToHighlight;               //This is the object that will get highlighted when the above panel is active
    public GameObject nonVRMovementsPanel;
    public Slider nonVRWalkSpeedSlider;
    public Slider nonVRRunSpeedSlider;
    public Slider nonVRTurnSpeedSlider;
    public float nonVRWalkSpeedSlowPreset = 2.5f;
    public float nonVRRunSpeedSlowPreset = 4f;
    public float nonVRTurnSpeedSlowPreset = 1.1f;
    public float nonVRWalkSpeedModeratePreset = 4f;
    public float nonVRRunSpeedModeratePreset = 6f;
    public float nonVRTurnSpeedModeratePreset = 1.5f;
    public float nonVRWalkSpeedFastPreset = 5f;
    public float nonVRRunSpeedFastPreset = 8f;
    public float nonVRTurnSpeedFastPreset = 2f;
    public GameObject nonVRWalkSpeedCurrentValue;
    public GameObject nonVRRunspeedCurrentValue;
    public GameObject nonVRTurnSpeedCurrentValue;

    GameObject lastselect;

    void Start ()
    {
        //lastselect = new GameObject();
        initializeMovements();
    }


    void Update ()
    {
        //This makes sure that when the user clicks the mouse in the screen, the focus on the button is not lost
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }

        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }

        if (InputManager_Enyx.pauseMenuButton == true)
        {
            pauseGame();
            //places menu in front of camera
            //transform.position = (Camera.main.transform.forward * forwardFactor) + Camera.main.transform.position; // - adds in the player's position to move it to where the player is located.
            //transform.eulerAngles = Camera.main.transform.eulerAngles; // Rotate the canvas to match the player's rotation.
            mainPausePanelOn();

        }
    }

    //pauses the game
    public void pauseGame()
    {
        //	centerCamera ();
        Time.timeScale = 0;

        FMOD_Audio_Menu audioManager = GetComponent<FMOD_Audio_Menu>();
        if (audioManager != null)
        {
            audioManager.pauseGameAudio();
        }

        GameObject settingsMNGR = GameObject.Find("Managers/Settings_Manager_Singleton");

        if (settingsMNGR != null)
        {
            SettingsManager_Singleton sttingsScript = settingsMNGR.GetComponent<SettingsManager_Singleton>();
            sttingsScript.freezeMovements(true);
        }

    }

    //unpauses the game
    public void unpauseGame()
    {
        Time.timeScale = 1;

        FMOD_Audio_Menu audioManager = this.GetComponent<FMOD_Audio_Menu>();
        if (audioManager != null)
        {
            audioManager.resumeGameAudio();
        }

        
 
        GameObject settingsMNGR = GameObject.Find("Managers/Settings_Manager_Singleton");

        if (settingsMNGR != null)
        {
            SettingsManager_Singleton sttingsScript = settingsMNGR.GetComponent<SettingsManager_Singleton>();
            sttingsScript.freezeMovements(false);
        }

        turnOffAllPanels();

    }






    void initializeMovements()
    {
        GameObject sttingsMngr = GameObject.Find("Managers/Settings_Manager_Singleton");
        if (sttingsMngr != null)
        {
            SettingsManager_Singleton mngr = sttingsMngr.GetComponent<SettingsManager_Singleton>();

            VRWalkSpeedSlowPreset = mngr.playerWalkSpeed_VR;
            VRWalkSpeedSlowPreset = mngr.playerMaxWalkSpeed_VR;
            VRWalkSpeedSlowPreset = mngr.playerStairWalkSpeed_VR; // i dont know what stairs should be. need to fix this

            VRRunSpeedSlowPreset = mngr.playerRunSpeed_VR;
            VRTurnSpeedSlowPreset = mngr.XSensitivity_VR;
            VRTurnSpeedSlowPreset = mngr.YSensitivity_VR;

            //non-vr
            nonVRWalkSpeedSlowPreset = mngr.playerWalkSpeed_NonVR;
            nonVRWalkSpeedSlowPreset = mngr.playerMaxWalkSpeed_NonVR;
            nonVRWalkSpeedSlowPreset = mngr.playerStairWalkSpeed_NonVR; // i dont know what stairs should be. need to fix this

            nonVRRunSpeedSlowPreset = mngr.playerRunSpeed_NonVR;
            nonVRTurnSpeedSlowPreset = mngr.XSensitivity_NonVR;
            nonVRTurnSpeedSlowPreset = mngr.YSensitivity_NonVR;


            if (VRSettings.enabled)
            {
                VRWalkSpeedSlider.value = mngr.playerWalkSpeed_VR;
                VRRunSpeedSlider.value = mngr.playerRunSpeed_VR;
                VRTurnSpeedSlider.value = mngr.XSensitivity_VR;
                VRDisplaySliderToHighlight.value = mngr.brightness / 5;
            }

            else
            {
                nonVRWalkSpeedSlider.value = mngr.playerWalkSpeed_NonVR;
                nonVRRunSpeedSlider.value = mngr.playerRunSpeed_NonVR;
                nonVRTurnSpeedSlider.value = mngr.XSensitivity_NonVR;
                nonVRDisplaySliderToHighlight.value = mngr.brightness / 5;
            }

            mngr.updateStaticVars();

        }
    }

    public void turnOffAllPanels()
    {
        mainPauseCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
        audioMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
    }

    public void mainPausePanelOn()
    {
        mainPauseCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);
        audioMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
        pauseButtonToHighlight.Select();
    }

    public void mainOptionsPanelOn()
    {
        mainPauseCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
        optionsButtonToHighlight.Select();
    }

    public void mainAudioPanelOn()
    {
        mainPauseCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(true);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
        audioSliderToHighlight.Select();
    }

    public void mainDisplayPanelOn()
    {
        mainPauseCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(true);
        controllsMenuCanvas.SetActive(false);

        if (VRSettings.enabled)
        {
            VRDisplayPanel.SetActive(true);
            nonVRDisplayPanel.SetActive(false);
            VRDisplaySliderToHighlight.Select();
        }

        else
        {
            VRDisplayPanel.SetActive(false);
            nonVRDisplayPanel.SetActive(true);
            nonVRDisplaySliderToHighlight.Select();
        }
    }

    public void mainControlsPanelOn()
    {
        mainPauseCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(true);
        handleWalkSpeedSlider();
        handleRunSpeedSlider();
        handleTurnSpeedSlider();

        if (VRSettings.enabled)
        {
            VRMovementsPanel.SetActive(true);
            nonVRMovementsPanel.SetActive(false);
            VRControllsSliderToHighlight.Select();
        }

        else
        {
            VRMovementsPanel.SetActive(false);
            nonVRMovementsPanel.SetActive(true);
            nonVRControllsSliderToHighlight.Select();
        }
    }


    //audio menu functions
    public void handleMusicVolumeSlider()
    {
        FMOD_Audio_Menu audioManager = this.GetComponent<FMOD_Audio_Menu>();
        audioManager.setMusicLevel(musicVolumeSlider.value);
    }

    public void handleSFXVolumeSlider()
    {
        FMOD_Audio_Menu audioManager = this.GetComponent<FMOD_Audio_Menu>();
        audioManager.setSFXLevel(sfxVolumeSlider.value);
    }

    public void handleDialogueVolumeSlider()
    {
        FMOD_Audio_Menu audioManager = this.GetComponent<FMOD_Audio_Menu>();
        audioManager.setDialogueLevel(dialogueVolumeSlider.value);
    }


    public void handleWalkSpeedSlider()
    {
        //Debug.Log("Walk Speed = " + walkSpeedSlider.value);
        GameObject settingsManager = GameObject.Find("Managers/Settings_Manager_Singleton");
        if (settingsManager != null)
        {
            SettingsManager_Singleton mngr = settingsManager.GetComponent<SettingsManager_Singleton>();
            if (VRSettings.enabled)
            {
                mngr.playerWalkSpeed_VR = VRWalkSpeedSlider.value;
                mngr.playerMaxWalkSpeed_VR = VRWalkSpeedSlider.value;
                mngr.playerStairWalkSpeed_VR = VRWalkSpeedSlider.value / 2; // i dont know what stairs should be. need to fix this
                mngr.updateStaticVars();
                
                GameObject currentDisplayValue = VRWalkSpeedCurrentValue;
                if (currentDisplayValue != null)
                {
                    Text textBox = currentDisplayValue.GetComponent<Text>();
                    textBox.text = VRWalkSpeedSlider.value.ToString("f1");
                }
            }

            else
            {
                mngr.playerWalkSpeed_NonVR = nonVRWalkSpeedSlider.value;
                mngr.playerMaxWalkSpeed_NonVR = nonVRWalkSpeedSlider.value;
                mngr.playerStairWalkSpeed_NonVR = nonVRWalkSpeedSlider.value / 2; // i dont know what stairs should be. need to fix this
                mngr.updateStaticVars();

                GameObject currentDisplayValue = nonVRWalkSpeedCurrentValue;
                if (currentDisplayValue != null)
                {
                    Text textBox = currentDisplayValue.GetComponent<Text>();
                    textBox.text = nonVRWalkSpeedSlider.value.ToString("f1");
                }
            }

        }
    }




    public void handleRunSpeedSlider()
    {
        //Debug.Log("Run Speed = " + runSpeedSlider.value);
        GameObject settingsManager = GameObject.Find("Managers/Settings_Manager_Singleton");
        if (settingsManager != null)
        {
            SettingsManager_Singleton mngr = settingsManager.GetComponent<SettingsManager_Singleton>();

            if (VRSettings.enabled)
            {
                mngr.playerRunSpeed_VR = VRRunSpeedSlider.value;
                mngr.updateStaticVars();

                GameObject currentDisplayValue = VRRunspeedCurrentValue;
                if (currentDisplayValue != null)
                {
                    Text textBox = currentDisplayValue.GetComponent<Text>();
                    textBox.text = VRRunSpeedSlider.value.ToString("f1");
                }
            }

            else
            {
                mngr.playerRunSpeed_NonVR = nonVRRunSpeedSlider.value;
                mngr.updateStaticVars();

                GameObject currentDisplayValue = nonVRRunspeedCurrentValue;
                if (currentDisplayValue != null)
                {
                    Text textBox = currentDisplayValue.GetComponent<Text>();
                    textBox.text = nonVRRunSpeedSlider.value.ToString("f1");
                }

            }
        }

    }

    public void handleTurnSpeedSlider()
    {
        // Debug.Log("Turn Speed = " + turnSpeedSlider.value);
        GameObject settingsManager = GameObject.Find("Managers/Settings_Manager_Singleton");
        if (settingsManager != null)
        {
            SettingsManager_Singleton mngr = settingsManager.GetComponent<SettingsManager_Singleton>();
            if (VRSettings.enabled)
            {
                mngr.XSensitivity_VR = VRTurnSpeedSlider.value;
                mngr.YSensitivity_VR = VRTurnSpeedSlider.value;
                mngr.updateStaticVars();

                GameObject currentDisplayValue = VRTurnSpeedCurrentValue;
                if (currentDisplayValue != null)
                {
                    Text textBox = currentDisplayValue.GetComponent<Text>();
                    textBox.text = VRTurnSpeedSlider.value.ToString("f1");
                }
            }

            else
            {
                mngr.XSensitivity_NonVR = nonVRTurnSpeedSlider.value;
                mngr.YSensitivity_NonVR = nonVRTurnSpeedSlider.value;
                mngr.updateStaticVars();

                //GameObject currentDisplayValue = 
                GameObject currentDisplayValue = nonVRTurnSpeedCurrentValue;
                if (currentDisplayValue != null)
                {
                    Text textBox = currentDisplayValue.GetComponent<Text>();
                    textBox.text = nonVRTurnSpeedSlider.value.ToString("f1");
                }
            }

        }
    }


    public void handleBrightness()
    {
        if (VRSettings.enabled)
        {
            setBrightness(VRDisplaySliderToHighlight.value);

        }

        else if (!VRSettings.enabled)
        {
            setBrightness(nonVRDisplaySliderToHighlight.value);
        }

        else
        {
            Debug.Log("Brightness slider not connected. Drag appropriote slider into main menu manager");
        }
    }




    //acutalls sets colorful's brightness
    void setBrightness(float val)
    {
        BrightnessContrastGamma colorful = Camera.main.GetComponent<BrightnessContrastGamma>();
        colorful.Brightness = val * 5;

        GameObject settingsManager = GameObject.Find("Managers/Settings_Manager_Singleton");
        if (settingsManager != null)
        {
            SettingsManager_Singleton mngr = settingsManager.GetComponent<SettingsManager_Singleton>();
            mngr.brightness = colorful.Brightness;
        }
    }

    public void backButtonPressed()
    {
        //GameObject sttingsMngr = GameObject.Find("Managers/Settings_Manager_Singleton");

        //if (sttingsMngr != null)
        //{
        //SettingsManager_Singleton scrpt = sttingsMngr.GetComponent<SettingsManager_Singleton>();

        //if (InputManager_Enyx.bButtonDown || Input.GetMouseButtonDown(1))
        //{

        //Here we need to setup every canvas so the back button knows how to behave properly. We need to add new canvas here
        if (audioMenuCanvas.activeSelf)
        {
            optionsAudioButton.Select();
        }

        else if (displayMenuCanvas.activeSelf)
        {
            optionsDisplayButton.Select();
        }

        else if (controllsMenuCanvas.activeSelf)
        {
            optionsControllsButton.Select();
        }

        //***This needs to always be the last in this list to prevent going back to the main menu whiloe in other panels such as Audio, Display, etc.
        //else if (optionsMenuCanvas.activeSelf)
       // {
        //    mainMenuPanelOn();
       // }

        else
        {
       
        }
        //}
        //}
    }

}
