using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    // 2. DEÐÝÞÝKLÝK: 'Text' yerine 'TextMeshProUGUI' yazýyoruz
    public TextMeshProUGUI scoreText;

    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Yazý atama kýsmý aynýdýr (.text)
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
