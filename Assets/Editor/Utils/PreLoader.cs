using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scrips.Utils
{
    [InitializeOnLoad]
    public class PreLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadPreLoader()
        {
            if(EditorBuildSettings.scenes.Length  == 0)
            {
                Debug.LogWarning("Occhio non ci sono scene da pre-caricare");
                return;
            }
 
            foreach(GameObject go in Object.FindObjectsOfType<GameObject>())
                go.SetActive(false);
            
            
            PlayerPrefs.SetString("savedScene", EditorSceneManager.GetActiveScene().name);
            //EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.LoadScene(0);
        }
    }
}