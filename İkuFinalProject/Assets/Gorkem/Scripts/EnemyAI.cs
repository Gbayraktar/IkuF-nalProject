using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Bileşenler")]
    private Animator anim;      // Animasyon kontrolcüsü
    private Rigidbody2D rb;     // Fizik bileşeni

    [Header("Hareket Ayarları")]
    public Transform player;       // Hedef (Player)
    public float speed = 3f;       // Yürüme hızı
    public float followRange = 10f; // Takip mesafesi

    [Header("Can Ayarları")]
    public int maxHealth = 100;    // Maksimum can (Spawner bunu değiştirebilir)
    public int currentHealth;      // Anlık can

    [Header("Saldırı & Fizik")]
    public int damage = 10;            // Player'a vereceği hasar
    public float knockbackForce = 5f;  // Geri tepme gücü
    public float stunTime = 0.3f;      // Darbe alınca sersemleme süresi

    [Header("Ödüller")]
    public GameObject xpPrefab;    // XP Topu Prefabı
    public int scoreValue = 10;    // Ölünce kaç puan versin?

    // Düşmanın o an sersemleyip sersemlemediğini kontrol eder
    private bool isKnockedBack = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Oyuna başlarken canı fulle
        currentHealth = maxHealth;

        // Player'ı otomatik bul (Eğer elle atanmadıysa)
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // EĞER SERSEMLEMİŞSE (KNOCKBACK YEMİŞSE) HAREKET ETME!
        if (isKnockedBack) return;

        // Player ile mesafe ölç
        float distance = Vector2.Distance(transform.position, player.position);

        // Menzildeyse YÜRÜ
        if (distance < followRange)
        {
            // Player'a doğru hareket et
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // --- ANİMASYON: YÜRÜME ---
            if (anim != null) anim.SetFloat("Speed", 1f);

            // --- YÖN ÇEVİRME (FLIP) ---
            // Player sağda mı solda mı? Ona göre yüzünü dön.
            if (player.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1); // Sağa bak
            else
                transform.localScale = new Vector3(-1, 1, 1); // Sola bak
        }
        else
        {
            // Duruyorsa Idle animasyonuna geç
            if (anim != null) anim.SetFloat("Speed", 0f);
        }
    }

    // --- HASAR ALMA FONKSİYONU ---
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // --- ANİMASYON: DARBE (HIT) ---
        if (anim != null) anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- ÇARPIŞMA MANTIĞI ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. PLAYER'A HASAR VER
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // 2. PLAYER'I GERİ İT (PlayerMovement içindeki fonksiyonu çağır)
            PlayerMovement playerMove = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMove != null)
            {
                playerMove.CallKnockback(stunTime, knockbackForce, transform);
            }

            // 3. DÜŞMANI (KENDİNİ) GERİ İT
            StartCoroutine(EnemyKnockbackRoutine(collision.transform));
        }
    }

    // Düşmanın kendi geriye tepme rutini
    IEnumerator EnemyKnockbackRoutine(Transform playerTransform)
    {
        isKnockedBack = true; // Hareketi kilitle

        // Yönü hesapla: (Benim yerim - Player'ın yeri) = Geriye doğru
        Vector2 direction = (transform.position - playerTransform.position).normalized;

        // Önceki hızı sıfırla ve kuvvet uygula
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Sersemleme süresi kadar bekle
        yield return new WaitForSeconds(stunTime);

        // Normale dön
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false; // Kilidi aç
    }

    // --- ÖLÜM FONKSİYONU ---
    void Die()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue); // Puan ver

            ScoreManager.instance.AddKill(); // <-- YENİ EKLENEN SATIR (Sayaç Artsın)
        }

        // 2. XP DÜŞÜR
        if (xpPrefab != null)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }

        // 3. GANİMET (LOOT) DÜŞÜR
        LootBag lootBag = GetComponent<LootBag>();
        if (lootBag != null)
        {
            lootBag.DropLoot();
        }

        // 4. YOK OL
        Destroy(gameObject);
    }

    // Editörde menzili çizgi olarak görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
    }
}
