using DG.Tweening;
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

        public void Start()
        {
            EventsManager.Instance.OnHamsterDieEvent.AddListener(DeathCounterIncrease);
            EventsManager.Instance.OnUsableChargesEvent.AddListener(OnUsableChargesChanged);
        }
        
        public void StartLevelUI()
        {
            _time = Time.realtimeSinceStartup;
            deaths = 0;
        }

        private void DeathCounterIncrease(GameObject hamster)
        {
            lblDeaths.text = (++deaths).ToString();
        }

        private void OnUsableChargesChanged(int positive, int negative)
        {
            Debug.Log("pos: " + positive + " neg " + negative);
            lblPositives.text = positive.ToString();
            lblNegatives.text = negative.ToString();

            if (negative <= 0)
            {
                var imgneg = lblNegatives.transform.parent.GetComponent<Image>();
                imgneg.fillAmount = 0;
                imgneg.DOFillAmount(1, 3);
            }
            
            if (positive <= 0)
            {
                var img = lblPositives.transform.parent.GetComponent<Image>();
                img.fillAmount = 0;
                img.DOFillAmount(1, 3);
            }
        }

        void Update()
        {
            lblTime.text = (Time.realtimeSinceStartup - _time).ToString("0:00.0");
            chargesContainer.transform.position = Input.mousePosition;
        }
    }
}