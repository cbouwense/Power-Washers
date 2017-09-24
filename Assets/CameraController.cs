using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public enum CameraState { start, panUp, stop, targetWinrar }
    public CameraState state = CameraState.start;

    private float panTimer = 0;
    private float panThreshold = 10;

    private GameObject winrar;
    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update () {
		
        // Camera state machine
        switch (state)
        {

            case CameraState.start:

                if (panTimer >= panThreshold)
                {
                    state = CameraState.panUp;
                }
                panTimer += Time.deltaTime;

                break;

            case CameraState.panUp:

                transform.position = new Vector3(transform.position.x,
                                                 transform.position.y + 0.01f,
                                                 transform.position.z);

                if (transform.position.y >= 38)
                {
                    state = CameraState.stop;
                }

                break;

            case CameraState.stop:
                

                break;

            case CameraState.targetWinrar:

                camera.orthographicSize = 1.5f;
                transform.position = new Vector3(winrar.transform.position.x,
                                                 winrar.transform.position.y,
                                                 -5);

                break;

        }

	}

    public void WinrarGet(GameObject winrar)
    {
        this.winrar = winrar;
        state = CameraState.targetWinrar;
    }

}
