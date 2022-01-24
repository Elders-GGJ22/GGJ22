using UnityEngine;

namespace Assets.Scrips.UI
{
    public class UIManager : MonoBehaviour
    {
        // TODO qui riferimenti ai vari pannelli
        
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            EventsManager.Instance.OnLevelFinishedEvent.AddListener(DrawPanel_LevelEnd);
        }

        private void DrawPanel_LevelEnd(LevelStats levelStats)
        {
            // TODO abilita il tuo pannellino di fine livello qua
        }
    }
}