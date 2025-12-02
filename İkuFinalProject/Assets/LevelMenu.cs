using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [Header("Panel Baðlantýlarý")]
    public GameObject menuPanel;        // Ana Menü (Start, Credits, Quit)
    public GameObject creditsPanel;     // Yapýmcýlar
    public GameObject levelSelectPanel; // Level Seçme Ekraný (YENÝ)

    void Start()
    {
        // Baþlangýçta sadece Ana Menü açýk olsun
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }

    // --- ANA MENÜ BUTONLARI ---

    // START Butonuna basýnca Level Seçme ekranýný aç
    public void OpenLevelSelect()
    {
        menuPanel.SetActive(false);       // Menüyü kapat
        levelSelectPanel.SetActive(true); // Level ekranýný aç
    }

    public void OpenCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("ÇIKIÞ YAPILDI!");
        Application.Quit();
    }

    // --- GERÝ BUTONLARI (Hem Credits hem Level ekraný için) ---

    public void BackToMenu()
    {
        creditsPanel.SetActive(false);
        levelSelectPanel.SetActive(false); // Level ekranýný da kapat
        menuPanel.SetActive(true);         // Ana menüyü aç
    }

    // --- LEVEL SEÇME BUTONLARI ---

    public void LoadLevel1()
    {
        // Sahnenin adýnýn tam olarak "Level1" olduðundan emin ol
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        // Sahnenin adýnýn tam olarak "Level2" olduðundan emin ol
        SceneManager.LoadScene("Level2");
    }
}
