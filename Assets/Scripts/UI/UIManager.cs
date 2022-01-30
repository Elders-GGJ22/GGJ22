using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Assets.Scrips.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject panelEndLevel;
        [SerializeField] private GameObject panelHelp;
        [SerializeField] private GameObject panelSide;
        [SerializeField] private GameObject panelInGame;
        [SerializeField] private GameObject panelMainMenu;
        
        
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            
        }

        void Start()
        {
            EventsManager.Instance.OnLevelFinishedEvent.AddListener(DrawPanel_LevelEnd);
            EventsManager.Instance.OnLevelStartedEvent.AddListener(OnNewLevelStarted);
            EventsManager.Instance.OnMainMenuEvent.AddListener(OnMainMenu);
        }

        void OnMainMenu()
        {
            panelMainMenu.SetActive(true);
        }

        /// <summary>
        /// Evento ricevuto di fine livello. mostra pannello di vittoria / sconfitta
        /// </summary>
        /// <param name="levelStats"></param>
        private void DrawPanel_LevelEnd(LevelStats levelStats)
        {
            Debug.Log("level stats di fine livello: " + levelStats.HamstersSave + " " + levelStats.HamstersDead);
            panelEndLevel.gameObject.SetActive(true);
            panelEndLevel.GetComponent<P_EndOfLevel>().Draw(levelStats);
        }

        private void OnNewLevelStarted()
        {
            panelInGame.SetActive(true);
            
            panelSide.SetActive(true);
            panelSide.GetComponentInChildren<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;
            panelSide.transform.DOMoveX(-200, 0.7f).SetDelay(1).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    panelSide.SetActive(false);
                    panelInGame.GetComponent<P_InGame>().StartLevelUI();
                });
        }

        /// <summary>
        /// Abilita il pannello di aiuto. richiamabile probabilmente da più punti
        /// </summary>
        public void OpenHelpPanel()
        {
            panelHelp.gameObject.SetActive(true);
        }
    }
}