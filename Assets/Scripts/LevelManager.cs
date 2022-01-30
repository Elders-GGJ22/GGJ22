using System;
using System.Collections;
using Assets.Scrips.Hamsters;
using Assets.Scrips.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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
        public bool InfiniteHamsters;
        
        private int _hamsterOnLevel;
        private LevelStats _levelStats;
        
        [Header("Conditions")]
        public bool SpawnOverTime = false;
        public float SpawnTimer = 2;
        public float scale = 1;
        
        [SerializeField] private Transform SpawnTarget;

        public void Start()
        {
            // conto quanti hamster ci sono nel livello 
            // quando questo contatore arriverà a 0 avremo finito il livello
            _hamsterOnLevel = FindObjectsOfType<Hamster>().Length;
            
            // ascolto gli eventi di morte e successo degli hamster
            EventsManager.Instance.OnHamsterDieEvent.AddListener(OnHamsterDie);
            EventsManager.Instance.OnHamsterReachHouseEvent.AddListener(OnHamsterReachHouse);

            // preparo l'oggetto statistiche da inviare a fine livello
            _levelStats = new LevelStats(_hamsterOnLevel, Time.realtimeSinceStartup);
            
            // comunica l'inizio del livello al mondo
            EventsManager.Instance.OnLevelStarted();

            if (SpawnOverTime)
            {
                StartCoroutine(IeSpawnOverTime());
            }
        }

        IEnumerator IeSpawnOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpawnTimer);
                var go = Instantiate(Resources.Load("Prefabs/Hamster"), SpawnTarget.transform, false) as GameObject;
                go.transform.localScale = new Vector3(scale,scale,scale);
                EventsManager.Instance.OnHamsterSpawn(go);
            }
        }

        private void OnHamsterDie(GameObject hamster)
        {
            if (InfiniteHamsters && !SpawnOverTime)
            {
                var go = Instantiate(Resources.Load("Prefabs/Hamster"), SpawnTarget.transform, false) as GameObject;
                go.transform.localScale = new Vector3(scale,scale,scale);
                EventsManager.Instance.OnHamsterSpawn(go);
                _hamsterOnLevel++;
            }
            
            _hamsterOnLevel--;
            _levelStats.HamstersDead++;
        }

        private void OnHamsterReachHouse()
        {
            _hamsterOnLevel--;
            _levelStats.HamstersSave++;
            
            if (InfiniteHamsters)
            {
                _levelStats.PlayerWin = true;
                LevelEnded();
                return;
            }
            
            CheckIfLevelIsCompleted();
        }

        private void CheckIfLevelIsCompleted()
        {
            if (_hamsterOnLevel <= 0)
            {
                Debug.Log("fine livello: " + MinHamsterAlive + " hamstersave " + _levelStats.HamstersSave );
                
                // il giocatore vince solo con le condizioni indicate sopra (tutti salvi se specificato, o almeno il numero minimo
                if (MinHamsterAlive >= _levelStats.HamstersSave || (AllAlive && _levelStats.HamstersDead == 0))
                {
                    _levelStats.PlayerWin = true;
                }
                LevelEnded();
            }
        }

        private void LevelEnded()
        {
            // aggiorno il tempo
            _levelStats.SetTotalTime(Time.realtimeSinceStartup);
            EventsManager.Instance.OnLevelFinished(_levelStats);
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
        public float TotalTime { get; set; }
        public bool PlayerWin { get; set; }

        // alla creazione classe salvo inizio tempo livello, alla fine, aggiorno facendo differenza col tempo conclusivo
        private float _timeStart;
        public LevelStats()
        {
            TotalHamsters = 0;
            HamstersDead = 0;
            HamstersSave = 0;
            TotalTime = 0;
            PlayerWin = false;
        }

        public LevelStats(int totalHamsters, float timeStart)
        {
            TotalHamsters = totalHamsters;
            HamstersDead = 0;
            HamstersSave = 0;
            TotalTime = 0;
            _timeStart = timeStart;
            PlayerWin = false;
        }

        // imposto il totale in millisecondi di tempo trascorso dall'inizio del livello
        public void SetTotalTime(float endTime)
        {
            TotalTime = endTime - _timeStart;
        }
    }
}