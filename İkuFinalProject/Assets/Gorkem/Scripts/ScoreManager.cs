using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Her yerden erişim anahtarı

    [Header("UI Bağlantıları")]
    public TextMeshProUGUI scoreText; // Puan yazısı
    public TextMeshProUGUI killText;  // Öldürme sayacı yazısı (YENİ)

    private int score = 0;
    private int killCount = 0; // Kaç düşman öldürdük?

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        UpdateUI();
    }

    // --- PUAN EKLEME ---
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // --- ÖLDÜRME SAYISINI ARTIRMA (YENİ) ---
    public void AddKill()
    {
        killCount++; // Sayıyı 1 artır
        UpdateUI();
    }

    void UpdateUI()
    {
        // Puanı Yazdır
        if (scoreText != null)
        {
            scoreText.text = "PUAN: " + score.ToString();
        }

        // Kill Sayısını Yazdır (YENİ)
        if (killText != null)
        {
            // İstersen başına kurukafa emojisi koyabilirsin
            killText.text = "" + killCount.ToString();
        }
    }
}
