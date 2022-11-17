using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] int timer;
    [SerializeField] int maxEnemies;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] GameObject bossType;
    [SerializeField] Transform bossPos;
    [SerializeField] bool hasBoss;

    int enemies;
    bool isSpawning;
    bool startSpawning;
    bool spawnBoss;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.waveCount++;
        gameManager.instance.UpdateWaveCount();
        gameManager.instance.enemyCount += maxEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && gameManager.instance.enemyCount == 0) {
            gameManager.instance.HasPlayerBeatAllWaves();
            if (!gameManager.instance.hasPlayerBeatAllWaves) {
                gameManager.instance.waveCount++;
                gameManager.instance.UpdateWaveCount();
                if (hasBoss && gameManager.instance.waveCount == gameManager.instance.waveMax) {
                    maxEnemies++;
                    spawnBoss = true;
                } else {
                    maxEnemies = (maxEnemies * 2) - 1;
                }
                gameManager.instance.enemyCount = maxEnemies;
                enemies = 0;
            }
        }
        if (startSpawning && !isSpawning && enemies < maxEnemies)
        {
            StartCoroutine(spawn());
            enemies++;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        if (spawnBoss) {
            Instantiate(bossType, bossPos.transform.position, bossType.transform.rotation);
            spawnBoss = false;
        } else {
            int randomEnemy = Random.Range(0, enemyTypes.Length - 1);
            int randomSpawn = Random.Range(0, spawnPos.Length - 1);
            Instantiate(enemyTypes[randomEnemy], spawnPos[randomSpawn].transform.position, enemyTypes[randomEnemy].transform.rotation);
        }

        yield return new WaitForSeconds(timer);
        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.enemyText.gameObject.SetActive(true);
        gameManager.instance.enemyCountText.gameObject.SetActive(true);
        gameManager.instance.waveText.gameObject.SetActive(true);
        gameManager.instance.waveCountText.gameObject.SetActive(true);
        gameManager.instance.waveNumberText.gameObject.SetActive(true);
        gameManager.instance.spawnerEnabled = true;

        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

}