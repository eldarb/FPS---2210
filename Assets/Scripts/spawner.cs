using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] int timer;
    [SerializeField] int maxEnemies;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform[] spawnPos;

    int enemies;
    bool isSpawning;
    bool startSpawning;
    int randomEnemyToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.enemyCount += maxEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.enemyCount == 0 && !gameManager.instance.hasPlayerBeatAllWaves)
        {
            gameManager.instance.waveCount++;
            gameManager.instance.HasPlayerBeatAllWaves();
            if (!gameManager.instance.hasPlayerBeatAllWaves)
            {
                maxEnemies = (maxEnemies * 2) - 1;
                gameManager.instance.enemyCount = maxEnemies;
                enemies = 0;
            }
        }
        if (startSpawning && !isSpawning && enemies < maxEnemies)
        {
            randomEnemyToSpawn = Random.Range(0, enemyTypes.Length - 1);
            StartCoroutine(spawn());
            enemies++;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        Instantiate(enemyTypes[randomEnemyToSpawn], spawnPos[Random.Range(0, spawnPos.Length - 1)].transform.position, enemyTypes[randomEnemyToSpawn].transform.rotation);
        //enemies++;

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


        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

}