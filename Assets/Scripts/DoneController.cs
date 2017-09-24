using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DoneController : MonoBehaviour {
    
    public GameObject replayButton, menuButton;
    public GameObject player1Winrar, player2Winrar;

    public void WinrarGet(GameObject winrar)
    {
        if (winrar.name == "Player1")
        {
            player1Winrar.SetActive(true);
        }
        else
        {
            player2Winrar.SetActive(true);
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

}
