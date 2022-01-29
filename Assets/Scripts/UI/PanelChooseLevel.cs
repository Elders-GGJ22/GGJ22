using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scrips.UI
{
    public class PanelChooseLevel : MonoBehaviour
    {
        public Button btnLevel1;
        public Button btnLevel2;
        public Button btnLevel3;
        public Button btnTutorial;

        public void Start()
        {
            btnLevel1.onClick.AddListener(OnLoadLevel1);
            btnLevel2.onClick.AddListener(OnLoadLevel2);
            btnLevel3.onClick.AddListener(OnLoadLevel3);
            btnTutorial.onClick.AddListener(OnTutorial);
        }

        public void OnLoadLevel1()
        {
            SceneManager.LoadScene("Def_Level1");
            this.gameObject.SetActive(false);
        }

        public void OnLoadLevel2()
        {
            SceneManager.LoadScene("Def_Level2");
            this.gameObject.SetActive(false);
        }

        public void OnLoadLevel3()
        {
            SceneManager.LoadScene("Def_Level3");
            this.gameObject.SetActive(false);
        }

        public void OnTutorial()
        {
            SceneManager.LoadScene("Tutorial");
            this.gameObject.SetActive(false);
        }
    }
}