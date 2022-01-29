using UnityEngine;

namespace Assets.Scrips.UI
{
    public class CallMainMenu : MonoBehaviour
    {
        public void Start()
        {
            EventsManager.Instance.ShowMainMenu();
        }

    }
}