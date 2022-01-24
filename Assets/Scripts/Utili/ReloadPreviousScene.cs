using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scrips.Utili
{
    public class ReloadPreviousScene : MonoBehaviour
    {
        void Start()
        {
            EditorSceneManager.LoadScene(PlayerPrefs.GetString("savedScene"));
        }
    }
}