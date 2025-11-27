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
        if (isDead) return; // Ölüye vurulmaz

        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Yaşıyorsak vurulma animasyonu
            if (animator != null) animator.SetTrigger("isHit");
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
        isDead = true;
        Debug.Log("OYUNCU ÖLDÜ! Animasyon oynatılıyor...");

        // 1. HAREKETLERİ KİLİTLE (Tuşlara basamasın)
        // Yürüme scriptini kapat
        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = false;

        // Saldırı scriptini kapat
        if (GetComponent<PlayerAttacksc>() != null)
            GetComponent<PlayerAttacksc>().enabled = false;

        // Kaymayı önlemek için hızı sıfırla
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // 2. ÖLÜM ANİMASYONUNU OYNAT
        if (animator != null)
        {
            animator.SetTrigger("isDead");
        }

        // 3. ZAMANI DURDURMAK İÇİN SAYACI BAŞLAT
        StartCoroutine(StopGameAfterDelay());
        SurvivalTimer timer = FindObjectOfType<SurvivalTimer>();
        if (timer != null)
        {
            timer.StopTimer();
        }

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
