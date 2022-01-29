using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        public Button btnLevels;
        public Button btnExit;

        public GameObject panelLevels;

        void Start()
        {
            btnExit.onClick.AddListener(ExitGame);
            btnLevels.onClick.AddListener(ShowLevelsChoice);
        }

        void ExitGame()
        {
            Application.Quit();
        }

        void ShowLevelsChoice()
        {
            panelLevels.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}