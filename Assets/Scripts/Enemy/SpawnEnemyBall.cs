using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyBall : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject spawnEffect;
    [SerializeField] bool isFlyingEnemy;

    EnemySpawner enemySpawner;
    float spawnPosY;

    private void Start()
    {
        GameObject enemySpawnerGO = GameObject.FindGameObjectWithTag("EnemySpawner");

        if (enemySpawnerGO != null)
        {
            enemySpawner = enemySpawnerGO.GetComponent<EnemySpawner>();
            enemySpawner.activeEnemies.Add(gameObject);
        }
        
        if (isFlyingEnemy) spawnPosY = 10;
        else spawnPosY = 1.5f;

        Destroy(gameObject, 10);
    }

    void Update()
    {
        if (transform.position.y <= spawnPosY)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            if (!isEnemySpawnerNull())
            {
                enemySpawner.activeEnemies.Remove(gameObject);
                enemySpawner.activeEnemies.Add(newEnemy);
            }

            Instantiate(spawnEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void setEnemySpaw(GameObject enemyPrefab)
    {
        this.enemyPrefab = enemyPrefab;
        isFlyingEnemy = enemyPrefab.name == "Flying Robot";
    }

    public void setSpawnPosition(Vector3 spawnBallPos)
    {
        transform.position = spawnBallPos;
    }

    bool isEnemySpawnerNull()
    {
        return enemySpawner == null;
    }
}
