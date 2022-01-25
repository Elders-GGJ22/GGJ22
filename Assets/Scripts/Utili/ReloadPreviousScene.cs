using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scrips.Utili
{
    public class ReloadPreviousScene : MonoBehaviour
    {
        void Start()
        {
#if UNITY_EDITOR
            EditorSceneManager.LoadScene(PlayerPrefs.GetString("savedScene"));
#endif
        }
    }
}