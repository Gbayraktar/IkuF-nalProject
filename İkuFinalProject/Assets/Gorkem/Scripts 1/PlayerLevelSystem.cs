using TMPro;
using UnityEngine;
using UnityEngine.UI; // Slider için gerekli

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("UI Baðlantýlarý")]
    public Slider xpSlider;
    public TextMeshProUGUI levelText;

    [Header("Seviye Ayarlarý")]
    public int currentLevel = 1;
    public float currentXP = 0;

    [Header("Zorluk Ayarý")]
    public float xpToNextLevel = 10;   // Ýlk seviye için gereken (Örn: 10)
    public float difficultyIncrease = 5; // Her levelde kaç artacak? (Örn: +5)

    public LevelUpManager levelManager;

    void Start()
    {
        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        currentXP += amount;

        // Level atlama kontrolü
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        currentLevel++;
        float overflowXP = currentXP - xpToNextLevel;
        currentXP = overflowXP;

        // Zorluk artýþý
        xpToNextLevel += difficultyIncrease;

        Debug.Log($"LEVEL ATLADIN! Yeni Level: {currentLevel}");

        // --- DEÐÝÞEN KISIM BURASI ---
        // Paneli açmasý için Manager'a haber ver
        if (levelManager != null)
        {
            levelManager.ShowLevelUpOptions();
        }
        // ----------------------------

        UpdateUI();
    }

    void UpdateUI()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }

        if (levelText != null)
        {
            levelText.text = "Level " + currentLevel.ToString();
        }
    }
}


