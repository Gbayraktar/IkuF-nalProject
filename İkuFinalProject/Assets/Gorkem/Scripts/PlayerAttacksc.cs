using UnityEngine;

public class PlayerAttacksc : MonoBehaviour
{
    [Header("Saldýrý Ayarlarý")]
    public GameObject bulletPrefab; // Mermi prefabý
    public float attackRange = 3f;  // Saldýrý menzili (Halka boyutu)
    public float fireRate = 0.5f;   // Ateþ etme hýzý (Saniye baþý bekleme)
    public LayerMask enemyLayers;   // Sadece düþmanlarý görmek için

    private float nextFireTime = 0f;

    void Update()
    {
        // Eðer ateþ etme zamaný geldiyse (Cooldown bittiyse)
        if (Time.time >= nextFireTime)
        {
            // En yakýndaki düþmaný bul
            Transform targetEnemy = FindClosestEnemy();

            // Eðer menzilde bir düþman varsa ateþ et
            if (targetEnemy != null)
            {
                Shoot(targetEnemy);

                // Bir sonraki ateþ zamanýný ayarla
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    // En yakýn düþmaný bulan fonksiyon
    Transform FindClosestEnemy()
    {
        // Menzil içindeki tüm düþmanlarý tara
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        Transform closest = null;
        float minDistance = Mathf.Infinity; // Baþlangýçta en kýsa mesafe sonsuz kabul edilir

        foreach (Collider2D enemy in hitEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            // Eðer bu düþman, þu ana kadar bulduðumuzdan daha yakýnsa, yeni hedef bu olur
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest; // En yakýn düþmaný (veya kimse yoksa null) döndür
    }

    void Shoot(Transform target)
    {
        // Düþmana doðru olan açýyý hesapla
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Mermiyi o açýya göre döndürerek oluþtur
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Instantiate(bulletPrefab, transform.position, rotation);
    }

    // Editörde menzili görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
