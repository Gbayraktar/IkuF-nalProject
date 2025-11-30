using UnityEngine;
using System.Collections.Generic; // Listeler için gerekli
using TMPro;
using System.Collections;

public class EnemySpawnner : MonoBehaviour
{
    [System.Serializable]
    public class WaveEnemy
    {
        public string name;           // Düþman ismi (Ekranda görünecek)
        public GameObject prefab;
        public float startMinute;

        [HideInInspector]
        public bool messageShown = false; // Yazý çýktý mý kontrolü (Gizli)
    }

    [Header("Düþman Listesi")]
    public List<WaveEnemy> enemies = new List<WaveEnemy>();

    [Header("UI Baðlantýsý")]
    public TextMeshProUGUI waveWarningText; // Ekranda çýkacak yazý

    [Header("Ayarlar")]
    public float spawnInterval = 2f;
    public float spawnRadius = 5f;
    public int maxEnemies = 20;

    private float timer;

    void Update()
    {
        // 1. ZAMAN KONTROLÜ VE UYARI SÝSTEMÝ
        CheckForNewWaves();

        // 2. SPAWN SÝSTEMÝ
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                timer = 0;
            }
        }
    }

    // --- YENÝ EKLENEN FONKSÝYON: UYARI SÝSTEMÝ ---
    void CheckForNewWaves()
    {
        float currentMinute = Time.time / 60f;

        foreach (WaveEnemy enemy in enemies)
        {
            // Eðer dakika geldiyse VE daha önce uyarý vermediysek VE oyunun baþý deðilse (0. dakika)
            if (currentMinute >= enemy.startMinute && !enemy.messageShown && enemy.startMinute > 0)
            {
                enemy.messageShown = true; // Bir daha gösterme

                // Uyarýyý baþlat
                StartCoroutine(ShowWarningRoutine("Dikkat!Yenisurugeliyor " + enemy.name.ToUpper()));
            }
        }
    }

    IEnumerator ShowWarningRoutine(string message)
    {
        if (waveWarningText != null)
        {
            waveWarningText.text = message;
            waveWarningText.gameObject.SetActive(true); // Yazýyý aç

            // 3 saniye ekranda kalsýn
            yield return new WaitForSeconds(3f);

            waveWarningText.gameObject.SetActive(false); // Yazýyý kapat
        }
    }

    void SpawnEnemy()
    {
        float currentMinute = Time.time / 60f;
        List<GameObject> availablePrefabs = new List<GameObject>();

        foreach (WaveEnemy enemy in enemies)
        {
            if (currentMinute >= enemy.startMinute)
            {
                availablePrefabs.Add(enemy.prefab);
            }
        }

        if (availablePrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePrefabs.Count);
            GameObject selectedPrefab = availablePrefabs[randomIndex];

            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomPos.x, randomPos.y, 0);

            GameObject newEnemy = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

            // Zorluk Artýrma (Can)
            EnemyAI enemyScript = newEnemy.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                int bonusHealth = Mathf.RoundToInt(currentMinute * 20);
                enemyScript.maxHealth += bonusHealth;
                enemyScript.currentHealth = enemyScript.maxHealth;
            }
        }
    }
}
