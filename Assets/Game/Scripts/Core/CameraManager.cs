using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera cmInGame, cmMiniGame;

    private void Start()
    {
        GameManager.ActionMiniGame += SetMiniGameCam;
    }

    private void SetMiniGameCam()
    {
        cmInGame.enabled = false;
        cmMiniGame.enabled = true;
    }

    private void OnDestroy()
    {
        GameManager.ActionMiniGame -= SetMiniGameCam;
    }
}
