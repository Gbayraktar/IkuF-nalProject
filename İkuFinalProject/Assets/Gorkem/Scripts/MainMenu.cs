using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel Baðlantýlarý")]
    public GameObject menuPanel;    // Ana Menü Paneli (Start, Credits, Quit burda)
    public GameObject creditsPanel; // Yapýmcýlar Paneli (Yazý, Geri butonu burda)

    void Start()
    {
        // Garanti olsun diye oyun baþýnda menüyü aç, credits'i kapa
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    // --- BUTON 1: OYUNA BAÞLA ---
    public void StartGame()
    {
        // Sahne adýný kendi oyun sahnenin adýyla deðiþtirmeyi unutma!
        SceneManager.LoadScene("SampleScene");
    }

    // --- BUTON 2: CREDITS'E GÝT ---
    public void OpenCredits()
    {
        // Menüyü kapat, Credits'i aç
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // --- BUTON 3: MENÜYE GERÝ DÖN ---
    public void CloseCredits() // Credits panelindeki "Geri" tuþu buna baðlanacak
    {
        // Credits'i kapat, Menüyü aç
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    // --- BUTON 4: ÇIKIÞ ---
    public void QuitGame()
    {
        Debug.Log("ÇIKIÞ YAPILDI!");
        Application.Quit();
    }
}
