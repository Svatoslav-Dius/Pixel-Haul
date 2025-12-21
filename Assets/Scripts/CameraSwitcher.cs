using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;

    public CinemachineVirtualCamera staticCamera;

    private bool showStatic = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            showStatic = !showStatic;

            if (showStatic)
            {
                staticCamera.Priority = 20;
                playerCamera.Priority = 10;
            }
            else
            {
                staticCamera.Priority = 10;
                playerCamera.Priority = 20;
            }
        }
    }
}