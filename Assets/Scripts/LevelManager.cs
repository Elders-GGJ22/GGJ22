using Assets.Scrips.Hamsters;
using Assets.Scrips.UI;
using UnityEngine;

namespace Assets.Scrips
{
    /// <summary>
    /// Classe che gestice le regole di vittoria/sconfitta per ogni livello 
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Win condition")] 
        public int MinHamsterAlive;
        public bool AllAlive;
        
        private int _hamsterOnLevel;
        private LevelStats _levelStats;

        [Header("Canvas Control")]
        [SerializeField] private GameObject _canvasWin;
        [SerializeField] private EndOfLevelPanel _endOfLevel;
        public void Start()
        {
            // conto quanti hamster ci sono nel livello 
            // quando questo contatore arriverà a 0 avremo finito il livello
            _hamsterOnLevel = FindObjectsOfType<Hamster>().Length;
            
            // ascolto gli eventi di morte e successo degli hamster
            EventsManager.Instance.OnHamsterDieEvent.AddListener(OnHamsterDie);
            EventsManager.Instance.OnHamsterReachHouseEvent.AddListener(OnHamsterReachHouse);

            // preparo l'oggetto statistiche da inviare a fine livello
            _levelStats = new LevelStats(_hamsterOnLevel);
        }

        private void OnHamsterDie()
        {
            _hamsterOnLevel--;
            _levelStats.HamstersDead++;
            CheckIfLevelIsCompleted();
        }

        private void OnHamsterReachHouse()
        {
            _hamsterOnLevel--;
            _levelStats.HamstersSave++;
            CheckIfLevelIsCompleted();
        }

        private void CheckIfLevelIsCompleted()
        {
            if (_hamsterOnLevel <= 0)
            {
                // il giocatore vince solo con le condizioni indicate sopra (tutti salvi se specificato, o almeno il numero minimo
                if (MinHamsterAlive >= _levelStats.HamstersSave || (AllAlive && _levelStats.HamstersDead == 0))
                {
                    Debug.Log("ciao");
                    _levelStats.PlayerWin = true;
                    _canvasWin.SetActive(true);
                    _endOfLevel.Draw(_levelStats);
                }
                EventsManager.Instance.OnLevelFinished(_levelStats);
            }
        }
    }

    /// <summary>
    /// oggetto che contiene le informazioni necessarie a fine livello per ui e statistiche
    /// </summary>
    public class LevelStats
    {
        public int TotalHamsters { get; set; }
        public int HamstersDead { get; set; }
        public int HamstersSave { get; set; }
        
        public bool PlayerWin { get; set; }

        public LevelStats()
        {
            TotalHamsters = 0;
            HamstersDead = 0;
            HamstersSave = 0;
            PlayerWin = false;
        }

        public LevelStats(int totalHamsters)
        {
            TotalHamsters = totalHamsters;
            HamstersDead = 0;
            HamstersSave = 0;
            PlayerWin = false;
        }
    }
}