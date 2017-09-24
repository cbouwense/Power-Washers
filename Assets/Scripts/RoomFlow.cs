using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class RoomFlow : MonoBehaviour {

    public GameObject mess, p1, p2, UI, camera;
    public GameObject replayButton, menuButton;
    private GameObject winrar;

    private enum GameState { spawnMesses, ready, play, done }
    [SerializeField]private GameState state = GameState.spawnMesses;
    private Vector2[] spawnPos;
    private GameObject[] messes;
    private float readyTimer = 3;
    private int messCount = 45;

    private PlayerController pc1, pc2;

    // How much you need to clean to win
    private float cleanedWin = 1250;

	// Use this for initialization
	void Start () {

        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        
        // Yeah I know, shut the fuck up
        if (sceneName == "level0")
        {
            spawnPos = new Vector2[messCount];
            spawnPos[0] = new Vector2(4, 0.76f);
            spawnPos[1] = new Vector2(5.92f, 0.76f);
            spawnPos[2] = new Vector2(7.84f, 0.76f);
            spawnPos[3] = new Vector2(1.00f, -1.00f);
            spawnPos[4] = new Vector2(10.85f, -1.00f);
            spawnPos[5] = new Vector2(5.92f, -2.24f);
            spawnPos[6] = new Vector2(1.00f, -3.91f);
            spawnPos[7] = new Vector2(2.95f, -3.91f);
            spawnPos[8] = new Vector2(8.85f, -3.91f);
            spawnPos[9] = new Vector2(10.85f, -3.91f);
            spawnPos[10] = new Vector2(2.98f, 2.41f);
            spawnPos[11] = new Vector2(8.96f, 2.41f);
            spawnPos[12] = new Vector2(1.06f, 4.55f);
            spawnPos[13] = new Vector2(2.99f, 4.55f);
            spawnPos[14] = new Vector2(8.91f, 4.55f);
            spawnPos[15] = new Vector2(10.8f, 4.55f);
            spawnPos[16] = new Vector2(5.96f, 6.66f);
            spawnPos[17] = new Vector2(0.99f, 10.42f);
            spawnPos[18] = new Vector2(10.78f, 10.42f);
            spawnPos[19] = new Vector2(4.03f, 11.93f);
            spawnPos[20] = new Vector2(6f, 11.93f);
            spawnPos[21] = new Vector2(7.86f, 11.93f);
            spawnPos[22] = new Vector2(2.99f, 14.89f);
            spawnPos[23] = new Vector2(8.93f, 14.89f);
            spawnPos[24] = new Vector2(5.97f, 16.77f);
            spawnPos[25] = new Vector2(-0.96f, 19.22f);
            spawnPos[26] = new Vector2(12.84f, 19.22f);
            spawnPos[27] = new Vector2(5.91f, 21.48f);
            spawnPos[28] = new Vector2(3.95f, 26.66f);
            spawnPos[29] = new Vector2(5.88f, 26.66f);
            spawnPos[30] = new Vector2(7.86f, 26.66f);
            spawnPos[31] = new Vector2(5.94f, 31.81f);
            spawnPos[32] = new Vector2(1.03f, 33.25f);
            spawnPos[33] = new Vector2(2.93f, 33.25f);
            spawnPos[34] = new Vector2(8.87f, 33.25f);
            spawnPos[35] = new Vector2(10.83f, 33.25f);
            spawnPos[36] = new Vector2(0.09f, 38.38f);
            spawnPos[37] = new Vector2(1.94f, 38.38f);
            spawnPos[38] = new Vector2(3.88f, 38.38f);
            spawnPos[39] = new Vector2(7.92f, 38.38f);
            spawnPos[40] = new Vector2(9.82f, 38.38f);
            spawnPos[41] = new Vector2(11.78f, 38.38f);
            spawnPos[42] = new Vector2(3.97f, 44.18f);
            spawnPos[43] = new Vector2(5.87f, 44.18f);
            spawnPos[44] = new Vector2(7.81f, 44.18f);
        }

        pc1 = p1.GetComponent<PlayerController>();
        pc2 = p2.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update () {
		
        switch (state)
        {

            case GameState.spawnMesses:

                messes = new GameObject[spawnPos.Length];
                for (int i = 0; i < spawnPos.Length; i++)
                {
                    messes[i] = Instantiate(mess, spawnPos[i], new Quaternion());
                }

                // Intentonal fall through
                state = GameState.ready;
                goto case GameState.ready;

            case GameState.ready:

                // TODO Implement ready, go! timer
                state = GameState.play;
                // When timer is implemented, don't fall through
                goto case GameState.play;

            case GameState.play:

                // Player 1 Wins
                if (pc1.cleaned > cleanedWin)
                {
                    winrar = p1;
                    state = GameState.done;
                    goto case GameState.done;
                }

                // Player 2 Wins
                if (pc2.cleaned > cleanedWin)
                {
                    winrar = p2;
                    state = GameState.done;
                    goto case GameState.done;
                }

                break;

            case GameState.done:

                Debug.Log(winrar.name + " wins!");
                camera.GetComponent<CameraController>().WinrarGet(winrar);                
                UI.SetActive(true);

                break;

        }

	}
}
