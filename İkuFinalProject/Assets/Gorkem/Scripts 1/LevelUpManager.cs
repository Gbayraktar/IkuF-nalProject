using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    [Header("UI Baðlantýsý")]
    public GameObject levelUpPanel; // Panel objesi

    [Header("Player Baðlantýlarý")]
    public PlayerHealth playerHealth;     // Can iþlemleri için
    public PlayerMovement playerMovement; // Yürüme hýzý için
    public PlayerAttacksc playerAttack;     // Saldýrý hýzý ve menzil için

    // Level atlayýnca bu çalýþýr
    public void ShowLevelUpOptions()
    {
        levelUpPanel.SetActive(true); // Paneli aç
        Time.timeScale = 0f;          // Oyunu dondur
    }

    // --- BUTON 1: MAX CAN ARTIRMA ---
    public void UpgradeHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(10); // Daha dengeli
        }
        ClosePanel();
    }

    // --- BUTON 2: YÜRÜME HIZI ARTIRMA ---
    public void UpgradeSpeed()
    {
        if (playerMovement != null)
        {
            playerMovement.IncreaseMoveSpeed(0.25f);
        }
        ClosePanel();
    }

    // --- BUTON 3: SALDIRI HIZI (MERMÝ SERÝLÝÐÝ) ---
    public void UpgradeFireRate()
    {
        if (playerAttack != null)
        {
            // %10 Hýzlandýr (Süreyi 0.9 ile çarparsan %10 azalýr)
            playerAttack.PermanentSpeedUpgrade(0.100f);
        }
        ClosePanel();
    }

    // --- BUTON 4: MENZÝL GENÝÞLETME (CIRCLE) ---
    public void UpgradeRange()
    {
        if (playerAttack != null)
        {
            playerAttack.IncreaseRange(0.5f);
        }
        ClosePanel();
    }

    // Paneli kapatma ve oyunu devam ettirme
    void ClosePanel()
    {
        levelUpPanel.SetActive(false); // Paneli gizle
        Time.timeScale = 1f;           // Oyunu devam ettir
    }
}