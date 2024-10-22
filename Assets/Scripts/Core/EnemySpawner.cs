using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI prepareText;
    [SerializeField] GameObject[] enemyPrefabs;  
    [SerializeField] Transform[] spawnPoints;   
    [SerializeField] float spawnInterval = 3f;
    [SerializeField] float waitToSpawnTime = 15f;
    private List<GameObject> activeEnemies = new List<GameObject>(); // Danh sách các kẻ thù đang tồn tại trên bản đồ
    private float timer;  // Biến đếm thời gian giữa các lần spawn

    void Start()
    {
        timer = waitToSpawnTime; 
        prepareText.gameObject.SetActive(true);
        prepareText.text = $"You have {timer} seconds to prepare!";
        StartCoroutine(PrepareCountdown());
    }

    void Update()
    {
        // Kiểm tra và xóa kẻ thù đã chết (null) ra khỏi danh sách
        activeEnemies.RemoveAll(enemy => enemy == null);

        // Chỉ spawn kẻ thù nếu số lượng hiện tại nhỏ hơn 10
        if (activeEnemies.Count < 10)
        {
            // Giảm thời gian chờ và kiểm tra nếu timer lớn hơn 0
            if (timer > 0)
            {
                timer -= Time.deltaTime;  // Giảm thời gian chờ

                // Cập nhật prepareText với thời gian còn lại
                prepareText.text = $"You have {Mathf.Ceil(timer)} seconds to prepare!";
            }
            else
            {
                // Khi timer về 0, bắt đầu sinh kẻ thù
                SpawnEnemy();
                timer = spawnInterval;  // Reset lại thời gian chờ
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        if (randomEnemy.name == "Flying Robot") randomSpawnPoint.y = 10;
        GameObject spawnedEnemy = Instantiate(randomEnemy, randomSpawnPoint, Quaternion.identity);

        activeEnemies.Add(spawnedEnemy);
    }

    private IEnumerator PrepareCountdown()
    {
        yield return new WaitForSeconds(waitToSpawnTime); // Chờ 30 giây
        prepareText.gameObject.SetActive(false); // Tắt prepareText
    }
}
