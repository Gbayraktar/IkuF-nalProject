using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [Header("Ekrandaki Yazýlar")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI rangeText;

    [Header("Player Baðlantýlarý")]
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public PlayerAttacksc playerAttack;

    void Update()
    {
        // MAX CAN
        if (playerHealth != null)
        {
            hpText.text = "MaxHealth: " + playerHealth.maxHealth.ToString();
        }

        // HIZ
        if (playerMovement != null)
        {
            speedText.text = "Speed: " + playerMovement.moveSpeed.ToString("F1");
        }

        // --- DÜZELTTÝÐÝMÝZ KISIM BURASI ---
        if (playerAttack != null)
        {
            // Eskisi: Bekleme süresini yazýyordu (0.5 -> 0.4 -> 0.1)
            // Yenisi: 1 saniyeyi bekleme süresine bölersek "Hýzý" buluruz.

            // Örnek: 1 / 0.5 = 2 (Saniyede 2 mermi)
            // Örnek: 1 / 0.1 = 10 (Saniyede 10 mermi!)

            float atisHizi = 1f / playerAttack.fireRate;

            fireRateText.text = "AttackSpeed: " + atisHizi.ToString("F1");

            rangeText.text = "Rate: " + playerAttack.attackRange.ToString("F1");
        }
    }
}