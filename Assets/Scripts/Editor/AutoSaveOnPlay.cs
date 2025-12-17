using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoSaveOnRun
{
    static AutoSaveOnRun()
    {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state)
    {
        // Спрацьовує тільки в момент натискання кнопки Play
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto-Saving before entering Play Mode...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}