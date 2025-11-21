using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject enemyPrefab;  // Hangi düþmaný doðuracak?
    public float spawnInterval = 2f; // Kaç saniyede bir doðsun?
    public float spawnRadius = 5f;  // Ne kadar geniþ bir alana yayýlsýn?

    [Header("Limit")]
    public int maxEnemies = 10;     // Sahnede en fazla kaç düþman olabilir?

    private float timer;

    void Update()
    {
        // Sadece belirli aralýklarla çalýþ
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            // Limit kontrolü: Çok fazla düþman varsa doðurma
            // (Performansýn çökmemesi için önemli)
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length; // Tag'i "Enemy" olanlarý sayar

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                timer = 0; // Süreyi sýfýrla
            }
        }
    }

    void SpawnEnemy()
    {
        // 1. RASTGELE POZÝSYON BUL
        // Spawner'ýn merkezinden 'spawnRadius' kadar ötede rastgele bir nokta seç
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;

        // Bu rastgele noktayý Spawner'ýn o anki yerine ekle
        Vector3 spawnPos = transform.position + new Vector3(randomPos.x, randomPos.y, 0);

        // 2. DÜÞMANI OLUÞTUR
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    // Editörde doðma alanýný görmek için sarý çember çizer
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
