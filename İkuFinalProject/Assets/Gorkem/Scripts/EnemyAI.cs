using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Bileşenler")]
    private Animator anim;      // Animasyon kontrolcüsü
    private Rigidbody2D rb;     // Fizik bileşeni

    [Header("Hareket Ayarları")]
    public Transform player;        // Hedef (Player)
    public float speed = 3f;        // Yürüme hızı
    public float followRange = 10f; // Takip mesafesi

    [Header("Can Ayarları")]
    public int maxHealth = 100;     // Maksimum can
    public int currentHealth;       // Anlık can

    [Header("Saldırı & Fizik")]
    public int damage = 10;            // Player'a vereceği hasar
    public float knockbackForce = 5f;  // Geri tepme gücü
    public float stunTime = 0.3f;      // Darbe alınca sersemleme süresi

    [Header("Ödüller")]
    public GameObject xpPrefab;     // XP Topu Prefabı
    public int scoreValue = 10;     // Ölünce kaç puan versin?

    [Header("Ses Efektleri")]
    public AudioClip deathSound;    // Ölme sesi (Mp3/Wav)

    [Header("Boss Ayarı")]
    public bool isBoss = false;     // Sadece Boss prefabında işaretle!

    // Düşmanın o an sersemleyip sersemlemediğini kontrol eder
    private bool isKnockedBack = false;

    // Boss boyutunun bozulmaması için başlangıç boyutunu hafızada tutuyoruz
    private Vector3 defaultScale;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Canı fulle
        currentHealth = maxHealth;

        // Başlangıç boyutunu kaydet (Boss ise 3,3,1 kalır, zombiyse 1,1,1)
        defaultScale = transform.localScale;

        // Player'ı otomatik bul (Eğer sürüklemeyi unuttuysan)
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

        // Eğer darbe yemişse (Knockback), hareket etmesin
        if (isKnockedBack) return;

        // Player ile mesafe ölç
        float distance = Vector2.Distance(transform.position, player.position);

        // Menzildeyse YÜRÜ
        if (distance < followRange)
        {
            // Player'a doğru hareket et
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Animasyon: Yürüme (Speed 1)
            if (anim != null) anim.SetFloat("Speed", 1f);

            // --- YÖN ÇEVİRME (FLIP) ---
            // Player sağda mı solda mı? Ona göre yüzünü dön.
            // defaultScale kullanarak Boss'un küçülmesini engelliyoruz.
            if (player.position.x > transform.position.x)
            {
                // SAĞA BAK (Pozitif Boyut)
                transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
            }
            else
            {
                // SOLA BAK (Negatif Boyut)
                transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
            }
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

        // Animasyon: Darbe (Hit)
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

            // 2. PLAYER'I GERİ İT
            PlayerMovement playerMove = collision.gameObject.GetComponent<PlayerMovement>();
            // Eğer senin scriptin adı farklıysa (örn: PlayerMovementSc) burayı değiştir
            if (playerMove != null)
            {
                // Player scriptinde CallKnockback fonksiyonu varsa çağır
                // Yoksa hata vermemesi için burayı yorum satırı yapabilirsin
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
        rb.linearVelocity = Vector2.zero; // Unity 6 kullanıyorsan rb.linearVelocity yap
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
        // 1. SESİ ÇAL (Düşman yok olsa bile ses devam eder)
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        }

        // 2. BOSS KONTROLÜ (Eğer bu bir Boss ise oyunu bitir)
        if (isBoss)
        {
            Debug.Log("BOSS ÖLDÜ! ZAFER!");

            // Sayacı Durdur
            SurvivalTimer timer = FindObjectOfType<SurvivalTimer>();
            if (timer != null) timer.StopTimer();

            // Game Over (Win) Ekranını Aç
            GameoverManager gm = FindObjectOfType<GameoverManager>();
            if (gm != null) gm.ShowGameOver();
        }

        // 3. PUAN VER VE LEŞ SAY
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
            ScoreManager.instance.AddKill();
        }

        // 4. XP DÜŞÜR
        if (xpPrefab != null)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }

        // 5. GANİMET (LOOT) DÜŞÜR
        LootBag lootBag = GetComponent<LootBag>();
        if (lootBag != null)
        {
            lootBag.DropLoot();
        }

        // 6. YOK OL
        Destroy(gameObject);
    }

    // Editörde menzili çizgi olarak görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
    }
}