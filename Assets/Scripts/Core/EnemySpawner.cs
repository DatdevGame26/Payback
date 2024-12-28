using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //  Danh sách các kẻ thù đang tồn tại trong cảnh
    public List<GameObject> activeEnemies = new List<GameObject>();

    [SerializeField] bool stopSpawning;     //  Dừng sinh ra kẻ thù
    [SerializeField] TextMeshProUGUI prepareText;   //  Text màu đỏ
    [SerializeField] FillBar progressBar;   //  Thanh tiến trình màn chơi
    [SerializeField] int enemyCountEachWave;    //  Số lượng kẻ thù để kết thúc một wave

    [Header("Normal Enemy")]
    [SerializeField] GameObject enemySpawnBall;     //  Cầu sinh ra kẻ thù
    [SerializeField] GameObject[] enemyPrefabs;     //  Danh sách prefab kẻ thù để nhân bản
    [SerializeField] Transform[] spawnPoints;       //  Các điểm thả cầu sinh kẻ thù
    [SerializeField] float spawnInterval = 3f;      //  Thời gian để sinh kẻ thù tiếp theo
    [SerializeField] float waitToSpawnTime = 15f;   //  Thời gian lúc đầu đợi để bắt đầu wave đầu tiên

    [Header("Boss")]
    [SerializeField] GameObject bossSpawnBall;      //  Cầu sinh ra Boss
    [SerializeField] int spawnCountToTriggerBossFight;  //  Số lượng kẻ thù cần sinh để tiến đến Boss cuối cùng
    [SerializeField] Transform bossSpawnPos;        //  Vị trí sinh ra Boss
    [SerializeField] float waitToSpawnBossTime;     //  Đợi để Boss sinh ra

    List<GameObject> enemyPrefabsWillBeSpawn = new List<GameObject>();
    float spawnTimer;
    int currentAddEnemyIndex;

    int nextWaveCount;
    int enemySpawnCount;
    int mark75PercentSpawn;
    
    private void Awake()
    {
        enemyPrefabsWillBeSpawn.Add(enemyPrefabs[currentAddEnemyIndex]);
        nextWaveCount = enemyCountEachWave;
        mark75PercentSpawn = Mathf.RoundToInt(0.75f * spawnCountToTriggerBossFight);
    }
    void Start()
    {
        spawnTimer = waitToSpawnTime;
        StartCoroutine(PrepareCountdown());
    }

    void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (stopSpawning) return;

        HandleSpawnEnemy();
        progressBar.updateFillBar(enemySpawnCount, spawnCountToTriggerBossFight);
    }

    private void HandleSpawnEnemy()
    {
        if (activeEnemies.Count < 10)
        {
            if (spawnTimer > 0) spawnTimer -= Time.deltaTime;
            else
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
        }
    }

    public void SpawnEnemy()
    {
        GameObject randomEnemy = enemyPrefabsWillBeSpawn[Random.Range(0, enemyPrefabsWillBeSpawn.Count)];
        Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        SpawnEnemyBall spawn = Instantiate(enemySpawnBall, transform.position, Quaternion.identity).GetComponent<SpawnEnemyBall>();
        if (spawn != null)
        {
            spawn.setEnemySpaw(randomEnemy);
            spawn.setSpawnPosition(randomSpawnPoint);
        }

        enemySpawnCount++;
        if (enemySpawnCount >= spawnCountToTriggerBossFight)
        {
            stopSpawning = true;
            StartCoroutine(PrepareToFightBossCountdown(waitToSpawnBossTime));
            return;
        }
        if(enemySpawnCount == mark75PercentSpawn)
        {
            enemyPrefabsWillBeSpawn.RemoveRange(0, 3);
        }

        if (enemySpawnCount >= nextWaveCount)
        {
            nextWaveCount += enemyCountEachWave;
            StartCoroutine(WaitForNextWave());
        }
    }


    private IEnumerator PrepareToFightBossCountdown(float seconds)
    {
        prepareText.gameObject.SetActive(true);
        prepareText.text = "Kẻ lãnh đạo của binh đoàn rô-bốt đã cảm nhận được hỗn loạn mà bạn gây ra. Hãy chuẩn bị — Hắn đang đến!";
        progressBar.gameObject.SetActive(false);

        yield return new WaitForSeconds(seconds);

        Instantiate(bossSpawnBall, bossSpawnPos.position, Quaternion.identity);
        prepareText.gameObject.SetActive(false);
    }

    private IEnumerator PrepareCountdown()
    {
        prepareText.gameObject.SetActive(true);
        prepareText.text = "Một binh đoàn robot hùng hậu đang tiến đến bạn! Hãy sẵn sàng đối mặt!";
        yield return new WaitForSeconds(waitToSpawnTime);
        prepareText.gameObject.SetActive(false);
    }

    IEnumerator WaitForNextWave()
    {
        prepareText.gameObject.SetActive(true);
        prepareText.text = "Tấn công đợt tiếp theo đang đến...";
        stopSpawning = true;
        if(currentAddEnemyIndex < enemyPrefabs.Length - 1)
        {
            currentAddEnemyIndex++;
            enemyPrefabsWillBeSpawn.Add(enemyPrefabs[currentAddEnemyIndex]);
        }
        yield return new WaitForSeconds(20);
        stopSpawning = false;
        prepareText.gameObject.SetActive(false);
    }

    
}
