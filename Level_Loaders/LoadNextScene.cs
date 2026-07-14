//This script is used to additively load levels using IUnity's Async. This allows for zero wait time inbetween loading scenes in Unity

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class LoadNextScene : MonoBehaviour
{
    public string currentScene;
    public string nextSceneToLoad;
    public float timeToWait;

    void Start()
    {
        StartCoroutine(nextScene());
    }


    IEnumerator nextScene()
    {
        if (nextSceneToLoad != null)
        {
            //This sets up which scene to additively load
            AsyncOperation async1;
            async1 = SceneManager.LoadSceneAsync(nextSceneToLoad, LoadSceneMode.Additive);
            Application.backgroundLoadingPriority = ThreadPriority.High;

            while (async1.isDone == false)
            {
                yield return null;
            }

            yield return new WaitForSeconds(timeToWait);

            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentScene));
            instantActivateAllScene(nextSceneToLoad);

        }

        else
        {
            Debug.Log("No scene set in Inspector");
        }
    }



    //This will turn all async levels to active
    void instantActivateAllScene(string scn)
    {
        GameObject[] objects = SceneManager.GetSceneByName(scn).GetRootGameObjects();

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name == "Managers")
            {
                objects[i].SetActive(true);
            }
        }

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }

    }
}
