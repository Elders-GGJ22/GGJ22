using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.UI 
{
    public class P_InGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lblTime;
        [SerializeField] private TextMeshProUGUI lblDeaths;
        [SerializeField] private TextMeshProUGUI lblPositives;
        [SerializeField] private TextMeshProUGUI lblNegatives;
        [SerializeField] private GameObject chargesContainer;

        private int deaths = 0;
        private float _time = 0;
        public void StartLevelUI()
        {
            _time = Time.realtimeSinceStartup;
            deaths = 0;
            EventsManager.Instance.OnHamsterDieEvent.AddListener(DeathCounterIncrease);
            EventsManager.Instance.OnUsableChargesEvent.AddListener(OnUsableChargesChanged);
        }

        private void DeathCounterIncrease(GameObject hamster)
        {
            lblDeaths.text = (++deaths).ToString();
        }

        private void OnUsableChargesChanged(int positive, int negative)
        {
            lblPositives.text = positive.ToString();
            lblNegatives.text = negative.ToString();
        }

        void Update()
        {
            lblTime.text = (Time.realtimeSinceStartup - _time).ToString("0:00.0");
            chargesContainer.transform.position = Input.mousePosition;
        }
    }
}