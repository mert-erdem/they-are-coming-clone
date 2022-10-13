using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game.Core;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera cmInGame, cmMiniGame;
    [SerializeField] private Camera mainCam;
    public Camera MainCam => mainCam;

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
