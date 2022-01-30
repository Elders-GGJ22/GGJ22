using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        public Button btnLevels;
        public Button btnExit;
        public Button btnCredits;

        public GameObject panelLevels;
        public GameObject panelCredits;

        void Start()
        {
            btnExit.onClick.AddListener(ExitGame);
            btnLevels.onClick.AddListener(ShowLevelsChoice);
            btnCredits.onClick.AddListener(ShowCredits);
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

        void ShowCredits()
        {
            panelCredits.gameObject.SetActive(true);
        }
    }
}