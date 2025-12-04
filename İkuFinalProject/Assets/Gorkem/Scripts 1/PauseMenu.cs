using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Baðlantýlarý")]
    public GameObject pausePanel; // Oluþturduðun panel

    [Header("Durum Kontrolü")]
    // Oyun zaten bitmiþse ESC çalýþmasýn diye bu kontrolü yapacaðýz
    public GameObject gameOverPanel;

    private bool isPaused = false;

    void Update()
    {
        // ESC tuþuna basýldýysa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Eðer oyun zaten bittiyse (Game Over ekraný açýksa) hiçbir þey yapma
            if (gameOverPanel != null && gameOverPanel.activeSelf) return;

            if (isPaused)
            {
                ResumeGame(); // Açýksa kapat
            }
            else
            {
                PauseGame(); // Kapalýysa aç
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Paneli aç
        Time.timeScale = 0f;        // Zamaný durdur
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Paneli kapat
        Time.timeScale = 1f;         // Zamaný devam ettir
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Menüye dönerken zamaný düzeltmeyi unutma!
        SceneManager.LoadScene("MainMenu"); // Sahne adýný doðru yaz
    }

    public void QuitGame()
    {
        Debug.Log("ÇIKIÞ YAPILDI");
        Application.Quit();
    }
}
