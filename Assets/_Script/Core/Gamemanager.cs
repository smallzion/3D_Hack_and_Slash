using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// �÷��̾�
    /// </summary>
    Player player;
    public Player Player => player;


    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
    }
    

}
