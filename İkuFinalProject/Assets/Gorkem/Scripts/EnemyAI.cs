using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform player;
    public float speed = 3f;
    public float followRange = 5f;

    [Header("Ödül")]
    public int scoreValue = 10;

    [Header("Sald�r� & Fizik")]
    public int damage = 20;
    public float knockbackForce = 10f; // �tme g�c�
    public float stunTime = 0.3f;      // Ne kadar s�re sersemlesinler?

    private Rigidbody2D rb;
    private int currentHealth = 100; // Basit can sistemi
    private bool isKnockedBack = false; // Sersemleme kontrol�



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Player'� bulma kodu
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // E�ER SERSEMLEM��SE (KNOCKBACK YEM��SE) HAREKET ETME!
        if (isKnockedBack) return;

        // Mesafe �l� ve takip et
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < followRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // ... diğer kodlar ...

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // --- İŞTE BU SATIRI EKLİYORUZ ---
        // Eğer ScoreManager varsa, puan ekle
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }
        // --------------------------------

        // Efektler vs...
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. PLAYER'A HASAR VER
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(damage);

            // 2. PLAYER'I GER� FIRLAT (PlayerMovement i�indeki fonksiyonu �a��r)
            PlayerMovement playerMove = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMove != null)
            {
                // Player'a "Benden uzakla�" diyoruz
                playerMove.CallKnockback(stunTime, knockbackForce, transform);
            }

            // 3. D��MANI (KEND�N�) GER� FIRLAT
            StartCoroutine(EnemyKnockbackRoutine(collision.transform));
        }
    }

    // D��man�n kendi geriye tepme rutini
    IEnumerator EnemyKnockbackRoutine(Transform playerTransform)
    {
        isKnockedBack = true; // Takip etmeyi durdur

        // Y�n� hesapla: (Benim yerim - Player'�n yeri) = Geriye do�ru
        Vector2 direction = (transform.position - playerTransform.position).normalized;

        // Kendine kuvvet uygula
        rb.linearVelocity = Vector2.zero; // �nceki h�z�n� s�f�rla
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Sersemleme s�resi kadar bekle
        yield return new WaitForSeconds(stunTime);

        // Normale d�n
        rb.linearVelocity = Vector2.zero; // Kaymay� durdur
        isKnockedBack = false;
    }
}
