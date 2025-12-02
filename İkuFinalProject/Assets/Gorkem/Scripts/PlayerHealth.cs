using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false; // Öldük mü?

    [Header("Bağlantılar")]
    public Slider healthSlider;
    private Animator animator;

    void Start()
    {
        // ÖNEMLİ: Eğer daha önce oyun donmuşsa, yeni oyunda zamanı tekrar başlat
        Time.timeScale = 1;

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // --- 1. ANİMASYON (Eksik olan kısım burasıydı) ---
        // Eğer karakterde Animator varsa 'Hit' tetikleyicisini çalıştır
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        // --- 2. EKRAN TİTREMESİ ---
        CameraFollowsysteem cam = Camera.main.GetComponent<CameraFollowsysteem>();
        if (cam != null)
        {
            cam.TriggerShake(0.15f, 0.3f);
        }

        // --- 3. UI GÜNCELLEME ---
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log("PLAYER: Hasar aldı! Kalan Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealFull()
    {
        if (isDead) return;
        currentHealth = maxHealth;
        UpdateUI();
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;      // Kapasiteyi artır
        currentHealth += amount;  // Artan miktar kadar mevcut cana da ekle

        // UI varsa güncelle (Eğer slider kullanıyorsan)
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        Debug.Log($"MAX CAN ARTTI! Yeni Max Can: {maxHealth}");
    }

    void Die()
    {
        // Eğer zaten öldüysek tekrar çalışmasın (Hata önleyici)
        if (isDead) return;

        isDead = true;
        Debug.Log("OYUNCU ÖLDÜ! Animasyon oynatılıyor...");

        // 1. HAREKETLERİ KİLİTLE (Senin kodların)
        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = false;

        // Saldırı scriptini kapat (Senin script adın PlayerAttacksc ise onu yaz)
        if (GetComponent<PlayerAttacksc>() != null) // Veya PlayerAttacksc
            GetComponent<PlayerAttacksc>().enabled = false;

        // Kaymayı önlemek için hızı sıfırla
        // (Unity sürümüne göre 'velocity' veya 'linearVelocity' olabilir)
        if (GetComponent<Rigidbody2D>() != null)
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // 2. ÖLÜM ANİMASYONUNU OYNAT
        if (animator != null) // Senin kodunda 'animator' ise burayı değiştir
        {
            animator.SetTrigger("Hit"); // Veya senin animasyonun adı "isDead" ise onu yaz
            // Tavsiye: Ölüm için ayrı bir "Death" trigger'ı kullanmak daha iyidir.
        }

        // 3. SAYACI DURDUR
        SurvivalTimer timer = FindObjectOfType<SurvivalTimer>();
        if (timer != null)
        {
            timer.StopTimer();
        }

        // 4. BEKLEME VE PANEL AÇMA (Coroutine Başlat)
        StartCoroutine(DeathSequence());
    }

    // --- ÖLÜM SENARYOSU (Bekleyip Paneli Açar) ---
    System.Collections.IEnumerator DeathSequence()
    {
        // Animasyonun bitmesi için 1.5 saniye bekle (Süreyi kendine göre ayarla)
        yield return new WaitForSeconds(1.5f);

        // 5. GAME OVER PANELİNİ AÇ
        GameoverManager gm = FindObjectOfType<GameoverManager>();
        if (gm != null)
        {
            gm.ShowGameOver(); // Bu fonksiyon zaten oyunu donduruyor (TimeScale = 0)
        }

        // 6. PLAYER'I ARTIK YOK EDEBİLİRİZ
        // (Panel açıldıktan sonra arkada görünmesini istemiyorsan)
        Destroy(gameObject);
    }

    IEnumerator StopGameAfterDelay()
    {
        // 2 Saniye bekle (Bu sürede animasyon oynar, düşmanlar hareket etmeye devam eder)
        yield return new WaitForSeconds(2f);

        // --- ZAMANI DURDUR ---
        Time.timeScale = 0;

        Debug.Log("2 SANİYE GEÇTİ: OYUN TAMAMEN DURDU.");
        // Buraya Game Over panelini açma kodu gelecek
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
