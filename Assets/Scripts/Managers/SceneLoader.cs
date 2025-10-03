using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

[Serializable]
public struct SceneStruct
{
    public string sceneDisplayName;
    public SceneReference sceneToLoad;
    public LoadSceneMode loadSceneMode;
}

public class SceneLoader : Singleton<SceneLoader>
{
    public event Action onSceneOpened;
    
    [Header("Scenes")] 
    [SerializeField] private List<SceneStruct> scenes;
    [SerializeField] private List<SceneStruct> openScenes = new();

    private void OnEnable()
    {
        // Make sure that the current scene is accounted for in open scenes
        SceneStruct currentScene = scenes.Find(x => x.sceneToLoad.Name == SceneManager.GetActiveScene().name);
        if (!openScenes.Contains(currentScene))
        {
            openScenes.Add(currentScene);
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        LoadScene(GetSceneStructByString(sceneToLoad.Trim()));
    }

    /// <summary>
    /// Loads a scene! Nice! 
    /// </summary>
    /// <param name="sceneToLoad">The scene that you want to load</param>
    public bool LoadScene(SceneStruct sceneToLoad)
    {
        if (sceneToLoad.sceneToLoad != null)
        {
            Debug.Log($"Open Scene: {sceneToLoad.sceneDisplayName}");
            
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneToLoad.sceneToLoad.BuildIndex, sceneToLoad.loadSceneMode);
            openScenes.Add(sceneToLoad);
            if (loadSceneOperation != null)
            {
                loadSceneOperation.completed += SceneOpened;
                return true;
            }
        }
 
        Debug.Log($"Attempted to load '{sceneToLoad.sceneDisplayName}' but no scene asset was found.");
        return false;
    }

    private void SceneOpened(AsyncOperation loadOperation)
    {
        Debug.Log($"Scene opened successfully");
        onSceneOpened?.Invoke();
    }

    public void CloseScene(string sceneToClose)
    {
        CloseScene(GetSceneStructByString(sceneToClose.Trim()));
    }

    /// <summary>
    /// Closes a scene! This will produce an AsyncOperation that will report when completed.
    /// </summary>
    /// <param name="sceneToClose">The scene you want to close</param>
    public void CloseScene(SceneStruct sceneToClose)
    {
        Debug.Log($"Close Scene: {sceneToClose.sceneDisplayName}");
        
        if (openScenes.Contains(sceneToClose))
        {
            openScenes.Remove(sceneToClose);
        }
        
        // Unloads the scene and bind to the async operation.
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose.sceneToLoad.BuildIndex);
        if (unloadOperation != null)
        {
            unloadOperation.completed += SceneClosed;
        }
    }

    /// <summary>
    /// After a scene has successfully been unloaded, this function will fire.
    /// </summary>
    /// <param name="unloadOperation"></param>
    private void SceneClosed(AsyncOperation unloadOperation)
    {
        Debug.Log($"Scene closed successfully");
        unloadOperation.completed -= SceneClosed;
    }

    private SceneStruct GetSceneStructByString(string sceneToGet)
    {
        return scenes.Find(x => x.sceneDisplayName == sceneToGet);
    }

    public bool CanLoadScene(string sceneString)
    {
        return GetSceneStructByString(sceneString).sceneToLoad != null;
    }
}
