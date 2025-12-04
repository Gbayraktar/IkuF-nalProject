using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Fizik çalýþýyor mu?
        Debug.Log("ÝKSÝRE BÝR ÞEY DEÐDÝ! Deðen þey: " + other.name);

        // 2. Tag doðru mu?
        if (other.CompareTag("Player"))
        {
            Debug.Log("EVET! Deðen þey Player. Can aranýyor...");

            // 3. Script var mý?
            PlayerHealth healthScript = other.GetComponent<PlayerHealth>();

            if (healthScript != null)
            {
                Debug.Log("Script bulundu! Can fulleniyor...");
                healthScript.HealFull();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("HATA: Player objesinde 'PlayerHealth' scripti yok!");
            }
        }
        else
        {
            Debug.LogWarning("HAYIR! Deðen þey Player deðil. Deðen þeyin Tag'i: " + other.tag);
        }
    }
}
