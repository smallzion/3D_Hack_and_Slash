using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wave : MonoBehaviour
{
    TextMeshProUGUI waveText;

    private void Awake()
    {
        waveText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        waveText.text = $"Wave : {GameManager.Instance.Wave}";
        GameManager.Instance.onWaveChange += (wave) =>
        {
            waveText.text = $"Wave : {wave}";
        };
    }

}
