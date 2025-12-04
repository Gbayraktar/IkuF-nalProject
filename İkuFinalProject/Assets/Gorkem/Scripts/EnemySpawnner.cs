using UnityEngine;
using System.Collections.Generic; // Listeler için gerekli
using TMPro;
using System.Collections;

public class EnemySpawnner : MonoBehaviour
{
    [System.Serializable]
    public class WaveEnemy
    {
        public string name;
        public GameObject prefab;
        public float startMinute;
        [HideInInspector] public bool messageShown = false;
    }

    [Header("Sýradan Düþmanlar (Sürekli Doðar)")]
    public List<WaveEnemy> enemies = new List<WaveEnemy>();

    [Header("--- BOSS AYARLARI ---")]
    public GameObject bossPrefab;    // Boss Prefabý buraya
    public float bossSpawnMinute = 5f; // Dakika 5
    public string bossWarningMessage = "";
    private bool bossSpawned = false; // Daha önce doðdu mu?

    [Header("UI & Genel Ayarlar")]
    public TextMeshProUGUI waveWarningText;
    public float spawnInterval = 2f;
    public float spawnRadius = 5f;
    public int maxEnemies = 20;

    private float timer;

    void Update()
    {
        // 1. BOSS KONTROLÜ (Öncelikli)
        CheckForBoss();

        // 2. NORMAL DALGA UYARILARI
        CheckForNewWaves();

        // 3. NORMAL DÜÞMAN DOÐURMA
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            // Boss sahnedeyken kalabalýk olmasýn istersen buraya (!bossSpawned) ekleyebilirsin
            if (currentEnemyCount < maxEnemies)
            {
                SpawnRandomEnemy();
                timer = 0;
            }
        }
    }

    // --- BOSS DOÐURMA SÝSTEMÝ ---
    void CheckForBoss()
    {
        // Eðer Boss daha önce doðmadýysa VE vakti geldiyse
        if (!bossSpawned && (Time.time / 60f) >= bossSpawnMinute)
        {
            bossSpawned = true; // Bir daha doðmasýn diye iþaretle
            StartCoroutine(SpawnBossRoutine());
        }
    }

    IEnumerator SpawnBossRoutine()
    {
        // 1. Önce Uyarýyý Göster
        if (waveWarningText != null)
        {
            StartCoroutine(ShowWarningRoutine(bossWarningMessage));
        }

        // 2. Biraz bekle (3 saniye uyarý bitene kadar)
        yield return new WaitForSeconds(3f);

        // 3. Boss'u Doður
        if (bossPrefab != null)
        {
            // Boss'u oyuncudan biraz uzaða koy (Çakýþmasýn)
            Vector2 randomPos = Random.insideUnitCircle.normalized * (spawnRadius + 2f);
            Vector3 spawnPos = transform.position + new Vector3(randomPos.x, randomPos.y, 0);

            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            Debug.Log("BOSS SAHNEYE ÝNDÝ!");
        }
    }

    // --- NORMAL DÜÞMAN DOÐURMA ---
    void SpawnRandomEnemy()
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

            // Zorluk Artýrma
            EnemyAI enemyScript = newEnemy.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                int bonusHealth = Mathf.RoundToInt(currentMinute * 20);
                enemyScript.maxHealth += bonusHealth;
                enemyScript.currentHealth = enemyScript.maxHealth;
            }
        }
    }

    // --- UYARI FONKSÝYONLARI ---
    void CheckForNewWaves()
    {
        float currentMinute = Time.time / 60f;
        foreach (WaveEnemy enemy in enemies)
        {
            if (currentMinute >= enemy.startMinute && !enemy.messageShown && enemy.startMinute > 0)
            {
                enemy.messageShown = true;
                StartCoroutine(ShowWarningRoutine("YENÝ SÜRÜ: " + enemy.name.ToUpper()));
            }
        }
    }

    IEnumerator ShowWarningRoutine(string message)
    {
        if (waveWarningText != null)
        {
            waveWarningText.text = message;
            waveWarningText.gameObject.SetActive(true);

            float duration = 3f;
            float endTime = Time.time + duration;

            while (Time.time < endTime)
            {
                waveWarningText.color = Color.red;
                yield return new WaitForSeconds(0.2f);
                waveWarningText.color = Color.black;
                yield return new WaitForSeconds(0.2f);
            }
            waveWarningText.gameObject.SetActive(false);
        }
    }
}
