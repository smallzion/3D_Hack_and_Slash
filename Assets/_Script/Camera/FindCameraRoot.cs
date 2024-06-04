using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCameraRoot : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        virtualCamera.Follow = GameManager.Instance.Player.transform;
    }
}
