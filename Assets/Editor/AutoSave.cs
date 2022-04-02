using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public class AutoSave
    {
        static AutoSave()
        {
            EditorApplication.playModeStateChanged += Save;
        }
        
        private static void Save(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}