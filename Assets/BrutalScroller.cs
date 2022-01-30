using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrutalScroller : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject creditPanel;

    public float speed = 50;

    public void CloseWindow()
    {
        creditPanel.gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
            float contentHeight = scrollRect.content.sizeDelta.y;
            float contentShift = speed * -1 * Time.deltaTime;
            scrollRect.verticalNormalizedPosition += contentShift / contentHeight;
    }
}
