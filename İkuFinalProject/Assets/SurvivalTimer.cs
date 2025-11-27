using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    [Header("UI Baðlantýsý")]
    public TextMeshProUGUI timerText; // Metin kutusunu buraya sürükle

    private float currentTime = 0f;   // Süre 0'dan baþlar
    private bool isRunning = true;    // Sayaç çalýþýyor mu?

    void Update()
    {
        if (isRunning)
        {
            // Her saniye süreyi artýr
            currentTime += Time.deltaTime;

            // Ekrana yazdýr
            DisplayTime(currentTime);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Matematik: Toplam saniyeyi Dakika ve Saniye cinsine çevir
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Ekrana "02:15" formatýnda yaz
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Bu fonksiyonu Player ölünce çaðýracaðýz ki süre dursun
    public void StopTimer()
    {
        isRunning = false;
        // Ýstersen burada "Rekor Süre: 10:00" gibi bir þey de yazdýrabilirsin
        // timerText.color = Color.red; 
    }
}