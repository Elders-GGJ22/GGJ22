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
            Debug.Log("pos: " + positive + " neg " + negative);
            lblPositives.text = positive.ToString();
            lblNegatives.text = negative.ToString();

            if (positive == 0)
            {
                lblPositives.transform.parent.GetComponent<Image>().DOFillAmount(1, 2);
            }
            
            if (negative == 0)
            {
                lblNegatives.transform.parent.GetComponent<Image>().DOFillAmount(1, 2);
            }
        }

        void Update()
        {
            lblTime.text = (Time.realtimeSinceStartup - _time).ToString("0:00.0");
            chargesContainer.transform.position = Input.mousePosition;
        }
    }
}