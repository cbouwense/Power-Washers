using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GUIController : MonoBehaviour
{
    public GameObject cleanBar1, cleanBar2;
    private Canvas HUD;
   
    private RectTransform cleanBar1Rect, cleanBar2Rect;
    private PlayerController pc1, pc2;
    private float maxCleanLength;

    // Use this for initialization
    void Start()
    {
        HUD = GetComponent<Canvas>();
        maxCleanLength = HUD.GetComponent<RectTransform>().rect.width;
        Debug.Log("maxCleanLength: " + maxCleanLength);

        pc1 = GameObject.Find("Player1").GetComponent<PlayerController>();
        pc2 = GameObject.Find("Player2").GetComponent<PlayerController>();
    }

    void Awake()
    {
        cleanBar1Rect = cleanBar1.gameObject.GetComponent<RectTransform>();
        cleanBar2Rect = cleanBar2.gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {

        // 1250

        float multiplier = HUD.GetComponent<RectTransform>().rect.width / 1250;
        
        cleanBar1Rect.sizeDelta = new Vector2(pc1.cleaned * multiplier, cleanBar1Rect.sizeDelta.y);
        cleanBar2Rect.sizeDelta = new Vector2(pc2.cleaned * multiplier, cleanBar2Rect.sizeDelta.y);
    }

}
