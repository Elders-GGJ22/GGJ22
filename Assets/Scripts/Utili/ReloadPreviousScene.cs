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

#if UNITY_EDITOR
            //EditorSceneManager.LoadScene(PlayerPrefs.GetString("MainScene"));
#endif
        }
    }
}