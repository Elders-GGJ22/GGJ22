using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    
    public Color[] _randomColors; /*= new []{
        new Color(255, 51, 51), 
        new Color(255, 249, 51), 
        new Color(58, 255, 51), 
        new Color(51, 88, 255), 
        new Color(255, 51, 230), 
    };*/
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        
        var randScale = Random.Range(80, 130);
        var randColor = Random.Range(0, _randomColors.Length-1);

        var yPos = (randScale-100) * 0.005f;

        _meshRenderer.material.color = _randomColors[randColor];
        transform.GetChild(0).localPosition = new Vector3(0,yPos,0);
        transform.GetChild(0).localScale = new Vector3(randScale,randScale, randScale);
    }
}
