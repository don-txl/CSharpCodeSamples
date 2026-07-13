/*
 * Purpose Of This Script: This script turns Menu Panels on and off.
 * Use: Attach to the Main Menu Canvas and then set the public game objects to the appropriate panels so it can turn them on and off.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using Colorful;
using UnityEngine.VR;




public class Main_Menu_Manager_Don : MonoBehaviour
{
    public bool newGame;                                //IF New Game is selected, this becomes true

    
    [Header("----------Main Menu----------")]
    [Tooltip("This is the Canvas that shows on the Main Menu Scene")]
    public GameObject mainMenuCanvas;                    //Main Menu Panel
    public Button mainMenuButtonToHighlight;            //This is the object that will get highlighted when the above panel is active


    [Header("----------Options Menu----------")]
    [Tooltip("This should be set to the Options Canvas")]
    public GameObject optionsMenuCanvas;                 //Options Menu Panel
    public Button optionsButtonToHighlight;             //This is the object that will get highlighted when the above panel is active
    public Button optionsDisplayButton;                 //We have this so the other panels know what to highlight when Back Button is pressed
    public Button optionsAudioButton;                   //We have this so the other panels know what to highlight when Back Button is pressed
    public Button optionsControllsButton;               //We have this so the other panels know what to highlight when Back Button is pressed


    [Header("----------Loading Menu----------")]
    [Tooltip("This should be set to the Loading Screen Canvas")]
    public GameObject loadingMenuCanvas;                    //Main Menu Loading Panel
    public string newGameToLoad;                            //This is the screen to load when new game is selected
    public string resumeGameToLoad;


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


    GameObject lastselect;                                  //This makes sure that when the user clicks the mouse in the screen, the focus on the button is not lost

    public void Start()
    {
        //This makes sure that when the user clicks the mouse in the screen, the focus on the button is not lost
        //lastselect = new GameObject();
        initializeMovements();
        mainMenuPanelOn();
    }

    public void Update()
    {
        //backButtonPressed();

        
        //This makes sure that when the user clicks the mouse in the screen, the focus on the button is not lost
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }

        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
        
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


   

    //___________________________________________________________________________________________________________________________________________________
    
    //-------------------------------Begin Turning On and Off Canvases. This Is Where We Set Up How Panels Turn On And Off-------------------------------
    //*******************************We need to add to Panels and Canvases to this section so they get turned on and off properly************************
    //____________________________________________________________________________________________________________________________________________________

    public void mainMenuPanelOn()
    {
        mainMenuCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);
        audioMenuCanvas.SetActive(false);
        loadingMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
        mainMenuButtonToHighlight.Select();
    }

    public void mainOptionsPanelOn()
    {
        mainMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(false);
        loadingMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
        optionsButtonToHighlight.Select();
    }

    public void mainAudioPanelOn()
    {
        mainMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(true);
        loadingMenuCanvas.SetActive(false);
        displayMenuCanvas.SetActive(false);
        controllsMenuCanvas.SetActive(false);
        audioSliderToHighlight.Select();
    }

    public void mainDisplayPanelOn()
    {
        mainMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(false);
        loadingMenuCanvas.SetActive(false);
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
        mainMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
        audioMenuCanvas.SetActive(false);
        loadingMenuCanvas.SetActive(false);
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

    //________________________________________________________________________________________________________________________________________________

    //-------------------------------------------------------End tunring on and off canvases----------------------------------------------------------
    //________________________________________________________________________________________________________________________________________________

    
    //Main Menu New Game function. This should be used to load the Continue Game Screen

    public void newGameButton()
    {

        StartCoroutine(startNewGame());

    }


    IEnumerator startNewGame()
    {
        
        
        //delete saved data file
        GameObject savemngr = GameObject.FindGameObjectWithTag("SaveManager");
        SaveSystem_Manager savemngrScript = null;
        if (savemngr != null)
        {
            savemngrScript = savemngr.GetComponent<SaveSystem_Manager>();
            if (savemngrScript != null)
            {
                savemngrScript.deleteSaveFiles();
                Debug.Log("Deleted the save files from within the menu");
            }
        }
        else
        {
            Debug.Log("Save System Manager does not exist in scene or can not be found. Cannot delete save files");
            yield break;
        }

        //remove loaded save settings
        savemngrScript.clearLoadedData();
         

    }


    
    public void backButtonPressed()
    {
                                  
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
        else if (optionsMenuCanvas.activeSelf)
        {
            mainMenuPanelOn();
        }

        else
        {

        }

    }


    public void toggleVR()
    {

        GameObject vrManager = GameObject.Find("Managers/VR Manager");
        if (vrManager != null)
        {
            VRManager_Enyx mngr = vrManager.GetComponent<VRManager_Enyx>();

            if (!VRSettings.enabled)
            {
                SettingsManager_Singleton settingsManager = GameObject.Find("Managers/Settings_Manager_Singleton").GetComponent<SettingsManager_Singleton>();
                settingsManager.TurnVrOn(true);
                mngr.BeginVRSetup();
            }

            else
            {
                SettingsManager_Singleton settingsManager = GameObject.Find("Managers/Settings_Manager_Singleton").GetComponent<SettingsManager_Singleton>();
                settingsManager.TurnVrOn(false);
                mngr.BeginShutdownVR();
            }
        }
    }



    /*________________________________________________________________________________________________________________________________________________

    //                                                  Begin Setup: Saving and Loading
    //                                                  ** Do Not Edit Unless Broken **   
    //________________________________________________________________________________________________________________________________________________

    This section does everything we need to save and load settings into the game */

    void saveSettings()
    {
        GameObject SaveManager = GameObject.FindGameObjectWithTag("SaveManager");

        if (SaveManager != null)
        {
            SaveSystem_Manager scrpt = SaveManager.GetComponent<SaveSystem_Manager>();
            scrpt.doSave();
        }

        else
        {
            Debug.Log("save manager cannot be found menu settings will not be saved.");
        }
    }

    //performs a load when we make a change
    void loadSettings()
    {
        GameObject SaveManager = GameObject.FindGameObjectWithTag("SaveManager");

        if (SaveManager != null)
        {
            SaveSystem_Manager scrpt = SaveManager.GetComponent<SaveSystem_Manager>();
            scrpt.doLoad();
        }

        else
        {
            Debug.Log("save manager cannot be found menu settings will not be loaded.");
        }
    }

 
    //________________________________________________________________________________________________________________________________________________

    //                                      Begin Setup: Getting Settings from Sliders, Toggles, and Buttons
    //________________________________________________________________________________________________________________________________________________
    
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



    //________________________________________________________________________________________________________________________________________________

    //                                      End Setup: Getting Settings from Sliders, Toggles, and Buttons
    //________________________________________________________________________________________________________________________________________________
}
