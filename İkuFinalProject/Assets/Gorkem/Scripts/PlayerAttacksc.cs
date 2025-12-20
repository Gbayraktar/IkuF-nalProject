using UnityEngine;
using System.Collections; // Coroutine için gerekli

public class PlayerAttacksc : MonoBehaviour
{
    [Header("Saldýrý Ayarlarý")]
    public GameObject bulletPrefab; // Mermi Prefabý
    public float attackRange = 3f;  // Saldýrý Menzili
    public LayerMask enemyLayers;   // Düþman Katmaný (Enemy seçili olmalý)

    [Header("Hýz Dengesi")]
    public float fireRate = 0.6f;      // Ýki mermi arasý bekleme (Küçük = Hýzlý)
    public float minFireRate = 0.1f;   // En fazla ne kadar hýzlanabilir?

    [Header("Görsel Ayar")]
    public Transform visualAura;       // Karakterin etrafýndaki Halka (Circle) objesi

    // Private deðiþkenler (Sistem için gerekli)
    private float nextFireTime = 0f;
    private float initialRange;       // Oyun baþýndaki menzil (Orantý için)
    private Vector3 initialAuraScale; // Oyun baþýndaki halka boyutu (Orantý için)

    [Header("Ses Efektleri")]
    public AudioClip shootSound;    // Ses dosyasýný buraya sürükleyeceðiz
    private AudioSource audioSource; // Player'ýn üzerindeki AudioSource

    void Start()
    {
        // Baþlangýç deðerlerini hafýzaya alýyoruz
        initialRange = attackRange;

        if (visualAura != null)
        {
            initialAuraScale = visualAura.localScale;
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Ateþ etme zamaný geldiyse
        if (Time.time >= nextFireTime)
        {
            Transform targetEnemy = FindClosestEnemy();

            // Menzilde düþman varsa ateþ et
            if (targetEnemy != null)
            {
                Shoot(targetEnemy);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    // --- ÖZELLÝK 1: SALDIRI HIZI ARTIRMA ---
    // LevelUpManager çaðýrýr (Örn: 0.05 azaltýr)
    // %10 Hýzlandýrma için amount yerine 0.9f göndereceðiz
    public void PermanentSpeedUpgrade(float percentage)
    {

        // YENÝ YÖNTEM: Mevcut hýzý %10 azalt (Çarpma iþlemi)
        // Örn: 0.5 * 0.9 = 0.45
        // Örn: 0.1 * 0.9 = 0.09 (Fark azalýyor, denge saðlanýyor)
        fireRate = fireRate * percentage;

        // Limit kontrolü
        if (fireRate < minFireRate)
        {
            fireRate = minFireRate;
            Debug.Log("Maksimum Hýza Ulaþýldý!");
        }
    }

    // --- ÖZELLÝK 2: MENZÝL VE HALKA BÜYÜTME ---
    // LevelUpManager çaðýrýr (Örn: 1 birim artýrýr)
    public void IncreaseRange(float amount)
    {
        // 1. Gerçek saldýrý menzilini artýr
        attackRange += amount;

        // 2. Görsel halkayý orantýlý büyüt (Resim bozulmadan)
        if (visualAura != null)
        {
            // Oran hesabý: Yeni Menzil / Ýlk Menzil
            float ratio = attackRange / initialRange;

            // Ýlk boyutu bu oranla çarp
            visualAura.localScale = initialAuraScale * ratio;
        }

        Debug.Log($"MENZÝL ARTTI! Yeni Menzil: {attackRange}");
    }

    // --- YARDIMCI FONKSÝYONLAR ---

    // En yakýn düþmaný bulur
    Transform FindClosestEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in hitEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }
        return closest;
    }

    // Mermiyi oluþturur ve düþmana çevirir
    void Shoot(Transform target)
    {
        if (bulletPrefab == null) return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        if (audioSource != null && shootSound != null)
        {
            // PlayOneShot: Seslerin üst üste binmesine izin verir (Taramalý tüfek gibi)
            audioSource.PlayOneShot(shootSound);
        }

        Instantiate(bulletPrefab, transform.position, rotation);
    }

    // Editörde sarý menzil çizgisi çizer
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
