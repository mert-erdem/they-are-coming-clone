using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIndicator : MonoBehaviour
{
    [SerializeField] private Transform theIndicator;
    [Header("Specs")]
    [SerializeField] private float offsetY = 0;
    [SerializeField] private float offsetX = 0;
    private Camera mainCam;
    private Vector3 newPos;

    private void Start()
    {
        GameManager.ActionMiniGame += DisableThis;
        mainCam = CameraManager.Instance.MainCam;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        newPos = mainCam.WorldToScreenPoint(this.transform.position);
        var currentPos = theIndicator.position;
        currentPos.y = newPos.y + offsetY;
        currentPos.x = newPos.x + offsetX;
        theIndicator.position = currentPos;
    }

    private void DisableThis()
    {
        theIndicator.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.ActionMiniGame -= DisableThis;
    }
}
