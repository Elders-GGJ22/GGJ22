using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scrips.UI
{

    // TODO
    public class EndOfLevelPanel : MonoBehaviour
    {
        [Header("End Stats")]
        [SerializeField] private TMP_Text _statsAliveTextValue = null;
        [SerializeField] private TMP_Text _statsDeadTextValue = null;
        
        // TODO
        public void Draw(LevelStats stats)
        {
            _statsAliveTextValue.text = stats.HamstersSave.ToString("0");
            _statsDeadTextValue.text = stats.HamstersDead.ToString("0");
        }
    }
}