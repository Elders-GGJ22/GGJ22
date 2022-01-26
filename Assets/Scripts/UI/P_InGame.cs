using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.UI 
{
    public class P_InGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lblTime;

        private float _time = 0;
        public void StartLevelUI()
        {
            _time = Time.realtimeSinceStartup;
        }

        void Update()
        {
            lblTime.text = (Time.realtimeSinceStartup - _time).ToString("0:00.0");
        }
    }
}