using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessManager : MonoBehaviour {

    private SpriteRenderer sr;
    public int cleanliness = 0;
    
    public Sprite[] mess1 = new Sprite[8];
    public Sprite[] mess2 = new Sprite[8];
    public Sprite[] mess3 = new Sprite[8];

    float messPic;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        messPic = Random.Range(0.01f, 2.99f);
        if ((int)messPic == 0)
        {
            sr.sprite = mess1[0];
        }
        else if ((int)messPic == 1)
        {
            sr.sprite = mess2[0];
        }
        else if ((int)messPic == 2)
        {
            sr.sprite = mess3[0];
        }
    }

    private void Update()
    {
        if ((int)messPic == 0)
        {
            sr.sprite = mess1[cleanliness / 10];
        }
        else if ((int)messPic == 1)
        {
            sr.sprite = mess2[cleanliness / 10];
        }
        else if ((int)messPic == 2)
        {
            sr.sprite = mess3[cleanliness / 10];
        }
    }

    public void Clean()
    {
        cleanliness++;
        if (cleanliness >= 80)
        {
            Destroy(this.gameObject);
        }
    }

}