using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFindPlayer : MonoBehaviour
{
    Player player;
    CinemachineVirtualCamera virtualCamera;


    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        virtualCamera.Follow = player.transform;
    }
}
