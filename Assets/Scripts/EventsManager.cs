using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scrips
{
    /// <summary>
    /// Singleton generico di raccolta e diramazione eventi in-game
    /// Per chi non conoscesse il meccanismo. Da ovunque può essere chiamato con:
    /// EventManager.Instance
    /// viene aggiunto in automatico nella scena. e persiste da li in poi in ogni scena in modo univoco 
    /// </summary>
    public class EventsManager : MonoBehaviour
    {
        private static EventsManager _instance;
        public static EventsManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<EventsManager>();
                }

                return _instance;
            }
        }
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            OnHamsterDieEvent = new HamsterDiedEvent();
            OnLevelFinishedEvent = new LevelFinishedEvent();
            OnHamsterReachHouseEvent = new HamsterReachHouseEvent();
        }

        [Header("Custom Events")] 
        public GameOverEvent OnGameOverEvent;

        public HamsterDiedEvent OnHamsterDieEvent;
        public HamsterReachHouseEvent OnHamsterReachHouseEvent;
        public LevelFinishedEvent OnLevelFinishedEvent;
        public LevelStartedEvent OnLevelStartedEvent;
        public HamsterSpawnEvent OnHamsterSpawnEvent;
        public UsableChargesEvent OnUsableChargesEvent;
        public PositiveChargeConsumedEvent OnPositiveChargeEvent;
        public NegativeChargeConsumedEvent OnNegativeChargeEvent;
        /// <summary>
        /// Ogni evento globale può essere mandato qui dove viene processato dal motore audio
        /// ed eventualmente diramato ad altri gameobject in ascolto
        /// </summary>
        public void OnHamsterDie(GameObject hamster)
        {
            AkSoundEngine.PostEvent("Play_Hamster_Death_Blood", gameObject);
            
            OnHamsterDieEvent?.Invoke(hamster);
        }

        public void OnHamsterSpawn(GameObject hamster)
        {
            OnHamsterSpawnEvent.Invoke(hamster);
        }

        public void OnHamsterReachHouse()
        {
            OnHamsterReachHouseEvent?.Invoke();
        }

        public void OnUsableChargesChanged(int positive, int negative)
        {
            
            OnUsableChargesEvent.Invoke(positive, negative);
        }

        public void OnPositiveUsed()
        {
            OnPositiveChargeEvent.Invoke();
        }

        public void OnNegativeUsed()
        {
            OnNegativeChargeEvent.Invoke();
        }

        public void OnGameOver()
        {
            OnGameOverEvent?.Invoke();
        }

        public void OnLevelFinished(LevelStats statistiche)
        {
            Debug.Log("Ho finito il livello!!");
            OnLevelFinishedEvent?.Invoke(statistiche);
        }

        /// <summary>
        /// Lanciato dallo start del level manager
        /// aggiusta ui, prepara gamemanager
        /// </summary>
        public void OnLevelStarted()
        {
            OnLevelStartedEvent.Invoke();
        }
    }
    
    #region custom unity events

    [System.Serializable]
    public class GameOverEvent : UnityEvent { }
    
    [System.Serializable]
    public class LevelStartedEvent : UnityEvent { }
    
    [System.Serializable]
    public class HamsterDiedEvent : UnityEvent<GameObject> { }
    
    [System.Serializable]
    public class HamsterSpawnEvent : UnityEvent<GameObject> { }
    
    [System.Serializable]
    public class HamsterReachHouseEvent : UnityEvent { }
    
    [System.Serializable]
    public class LevelFinishedEvent : UnityEvent<LevelStats> { }
    
    [System.Serializable]
    public class UsableChargesEvent : UnityEvent<int,int> { }
    
    [System.Serializable]
    public class PositiveChargeConsumedEvent : UnityEvent { }
    
    [System.Serializable]
    public class NegativeChargeConsumedEvent : UnityEvent { }

    #endregion
}