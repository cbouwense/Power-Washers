using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWindow : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController pc = GetComponent<PlayerController>();
        MessManager mm = collision.GetComponent<MessManager>();

        if (collision.gameObject.tag == "Mess")
        {
            pc.onMess = true;
        }

        if (pc.cleaning)
        {
            pc.Clean();
            mm.Clean();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController pc = GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Mess")
        {
            pc.onMess = false;
        }
    }

}