using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverManager : MonoBehaviour
{
    [Header("UI Baðlantýlarý")]
    public GameObject gameOverPanel; // Yaptýðýn o panel
    public TextMeshProUGUI finalScoreText; // Paneldeki skor yazýsý

    // Bu fonksiyonu PlayerHealth çaðýracak
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // Paneli aç

        // Final Skoru Yazdýr
        if (ScoreManager.instance != null && finalScoreText != null)
        {
            // ScoreManager'dan puaný çekip yazdýrýyoruz
            finalScoreText.text = "SKOR: " + ScoreManager.instance.GetScore().ToString();
        }

        // Oyunu Durdur
        Time.timeScale = 0f;
    }

    // --- BUTON 1: RESTART (Yeniden Baþla) ---
    public void RestartGame()
    {
        Time.timeScale = 1f; // ZAMANI TEKRAR BAÞLAT (Unutma!)

        // Þu anki sahneyi (Leveli) yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- BUTON 2: MAIN MENU (Ana Menü) ---
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Zamaný baþlat

        // "MainMenu" adýnda bir sahnen olmalý (Adýný doðru yaz)
        SceneManager.LoadScene("MainMenu");
    }

    // --- BUTON 3: QUIT (Çýkýþ) ---
    public void QuitGame()
    {
        Debug.Log("OYUNDAN ÇIKILDI!"); // Editörde çalýþmaz, build alýnca çalýþýr
        Application.Quit();
    }
}
