using UnityEngine;

public class BulletSpeedPower : MonoBehaviour
{
    [Header("Ayarlar")]
    [Tooltip("Her alýnýþta bekleme süresinden kaç saniye düþsün? (Örn: 0.05)")]
    public float reductionAmount = 0.05f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttacksc attackScript = other.GetComponent<PlayerAttacksc>();

            if (attackScript != null)
            {
                // Kalýcý fonksiyonu çaðýr
                attackScript.PermanentSpeedUpgrade(reductionAmount);

                // Eþyayý yok et
                Destroy(gameObject);
            }
        }
    }
}
