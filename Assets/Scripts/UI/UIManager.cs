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
        
        
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            EventsManager.Instance.OnLevelFinishedEvent.AddListener(DrawPanel_LevelEnd);
            EventsManager.Instance.OnLevelStartedEvent.AddListener(OnNewLevelStarted);
        }

        /// <summary>
        /// Evento ricevuto di fine livello. mostra pannello di vittoria / sconfitta
        /// </summary>
        /// <param name="levelStats"></param>
        private void DrawPanel_LevelEnd(LevelStats levelStats)
        {
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