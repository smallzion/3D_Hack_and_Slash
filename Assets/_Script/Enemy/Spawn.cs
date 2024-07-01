using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    List<Transform> spawnPoint;
    public int maxSpawn = 32;
    int spawnCount = 0;
    bool isSpawn = true;

    float elepsedTime = 0.0f;
    public float coolDown = 5.0f;

    private void Awake()
    {
        spawnPoint = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoint.Add(transform.GetChild(i));
        }
    }
    private void Start()
    {
        GameManager.Instance.onEnemyDie += () => spawnCount--;
        GameManager.Instance.onWaveChange += (wave) =>
        {
            coolDown -= coolDown * 0.1f;
        };
    }
    private void Update()
    {
        if (isSpawn && maxSpawn > spawnCount)
        {
            if (elepsedTime > coolDown)
            {
                EnemySpawn();
                elepsedTime = 0.0f;
            }
            elepsedTime += Time.deltaTime;
        }
    }

    void EnemySpawn()
    {
        int temp = Random.Range(0, spawnPoint.Count);
        Factory.Instance.GetDefaultEnemy(spawnPoint[temp].position);
        spawnCount++;
    }

    public void ToggleSpawning()
    {
        isSpawn = !isSpawn;
    }
}
