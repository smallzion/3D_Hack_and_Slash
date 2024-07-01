using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Player player;
    Slider hpBar;

    private void Awake()
    {
        hpBar = GetComponent<Slider>();
    }

    void Start()
    {
        player = GameManager.Instance.Player;
        player.playerHpChange += (value) =>
        {
            hpBar.value = value;
        };
    }

}
