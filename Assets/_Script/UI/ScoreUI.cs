using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    GameManager gameManager;

    TextMeshProUGUI killScoreText;


    private void Awake()
    {
        killScoreText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onEnemyKillCountChange += RefreshScoreUI;
        killScoreText.text = $"Score:  {gameManager.EnemyScore}";
    }

    private void RefreshScoreUI()
    {
        killScoreText.text = $"Score:  {gameManager.EnemyScore}";
    }
}

