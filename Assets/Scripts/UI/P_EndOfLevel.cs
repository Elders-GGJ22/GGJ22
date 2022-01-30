using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Assets.Scrips.UI
{
    public class P_EndOfLevel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lblTitle;
        [SerializeField] private TextMeshProUGUI lblSurvivors;
        [SerializeField] private TextMeshProUGUI LblCasualities;
        [SerializeField] private TextMeshProUGUI LblTime;
        
        [SerializeField] private Button btnContinue;
        [SerializeField] private Button btnMenu;

        private void Start()
        {
            btnContinue.onClick.AddListener(OnNextLevel);
            btnMenu.onClick.AddListener(OnMainMenu);
        }
        
        public void Draw(LevelStats stats)
        {
            lblTitle.text = stats.PlayerWin ? "You Won!" : "You lose";
            lblSurvivors.text = stats.HamstersSave.ToString("0");
            LblCasualities.text = stats.HamstersDead.ToString("0");
            LblTime.text = stats.TotalTime.ToString("0:00");
        }

        private void OnMainMenu()
        {
            SceneManager.LoadScene("MainScene");
            this.gameObject.SetActive(false);
        }

        private void OnNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+ 1);
            this.gameObject.SetActive(false);
        }
    }
}
