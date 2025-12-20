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
    public float xpToNextLevel = 10;
    public float difficultyIncrease = 5;

    public LevelUpManager levelManager;

    // --- YENÝ EKLENEN KISIM: SES DEÐÝÞKENLERÝ ---
    [Header("Ses Efektleri")]
    public AudioClip levelUpSound;   // Editörden ses dosyasýný buraya sürükle
    private AudioSource audioSource; // Sesi çalacak hoparlör
    // ---------------------------------------------

    void Start()
    {
        UpdateUI();

        // --- YENÝ EKLENEN KISIM: AUDIOSOURCE BULMA ---
        // Scriptin olduðu objede AudioSource var mý diye bakar, yoksa ekler
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // ---------------------------------------------
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

        // --- YENÝ EKLENEN KISIM: SESÝ ÇAL ---
        if (audioSource != null && levelUpSound != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }
        // ------------------------------------

        // Paneli açmasý için Manager'a haber ver
        if (levelManager != null)
        {
            levelManager.ShowLevelUpOptions();
        }

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