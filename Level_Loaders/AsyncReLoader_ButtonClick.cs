//This script is used to load and unload scenes additively inside of Unity
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AsyncReLoader_ButtonClick : MonoBehaviour {
    public float timeToWait;        // The amount of time (in seconds) to wait before beginning the reload process.
    public string[] loadScenes;     // List of scenes that should be loaded/reloaded.
    public string[] unloadScenes;

    [HideInInspector]
    public List<string> visibleSceneNames = new List<string>(); // A list of scene names that actually exist in the project.
    private bool visibleSceneNamesPopulated = false;            // Prevents the list from being rebuilt multiple times unnecessarily.


    public void beginLoad ()
    {
        StartCoroutine(Activate());
    }


    // This waits for a specified amount of time before reloading all valid scenes.
    IEnumerator Activate()
    {
        yield return new WaitForSeconds(timeToWait);

        // If the list hasn't been built yet (or is empty), populate it with valid scene names.
        if (!visibleSceneNamesPopulated || visibleSceneNames.Count == 0)
        {
            PopulateVisibleSceneNames();
        }

        // Reload every scene in the list.
        foreach (string sceneName in visibleSceneNames)
        {
            SceneController.control.ReloadScene(sceneName);
            
        }

        // Once all reload requests have been sent, clean up by unloading the previous scene instances.
        Deactivate();


    }

    // Unloads the scenes after the new versions have been loaded.
    void Deactivate()
    {
        // Unload all currently loaded scenes in the list.
        SceneController.control.UnloadAllScenes(visibleSceneNames.ToArray());
        SceneController.control.UnloadScenes(visibleSceneNames.ToArray());
    }

    // Builds a list of valid scene names. Only scenes that exist in the project are added. The current scene containing this GameObject is also added.
    void PopulateVisibleSceneNames()
    {
        visibleSceneNames = loadScenes.Where(sceneName => ProjectSceneList.SceneExists(sceneName)).ToList();
        visibleSceneNames.Add(gameObject.scene.name);
        visibleSceneNamesPopulated = true;
    }

    void OnValidate()
    {
        PopulateVisibleSceneNames();
    }
}
