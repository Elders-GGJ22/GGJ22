using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scrips.Utili
{
    public class ReloadPreviousScene : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene("MainScene");
            return;
            
#if UNITY_EDITOR
            EditorSceneManager.LoadScene(PlayerPrefs.GetString("savedScene"));
#else
            SceneManager.LoadScene("MainScene");
#endif
        }
    }
}