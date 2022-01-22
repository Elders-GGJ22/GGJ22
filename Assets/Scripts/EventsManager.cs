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
        }

        [Header("AK Wwise Events")] 
        public AK.Wwise.Event SfxEvent_HamsterDead;
        public AK.Wwise.Event SfxEvent_GameOver;
        public AK.Wwise.Event SfxEvent_Collided;
        // etc

        [Header("Custom Events")] 
        public GameOverEvent OnGameOverEvent;

        public HamsterDiedEvent OnHamsterDieEvent;
        public HamsterReachHouseEvent OnHamsterReachHouseEvent;
        public LevelFinished OnLevelFinishedEvent;
        /// <summary>
        /// Ogni evento globale può essere mandato qui dove viene processato dal motore audio
        /// ed eventualmente diramato ad altri gameobject in ascolto
        /// </summary>
        public void OnHamsterDie()
        {
            SfxEvent_HamsterDead.Post(this.gameObject);
            OnHamsterDieEvent?.Invoke();
        }

        public void OnHamsterReachHouse()
        {
            OnHamsterReachHouseEvent?.Invoke();
        }

        public void OnGameOver()
        {
            SfxEvent_GameOver.Post(this.gameObject);
            OnGameOverEvent?.Invoke();
        }

        public void OnLevelFinished(LevelStats statistiche)
        {
            OnLevelFinishedEvent?.Invoke(statistiche);
        }
        
        // etc..
        
        
    }
    
    #region custom unity events

    [System.Serializable]
    public class GameOverEvent : UnityEvent { }
    
    [System.Serializable]
    public class HamsterDiedEvent : UnityEvent { }
    
    [System.Serializable]
    public class HamsterReachHouseEvent : UnityEvent { }
    
    [System.Serializable]
    public class LevelFinished : UnityEvent<LevelStats> { }
    #endregion
}