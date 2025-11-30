using UnityEngine;

public class CameraFollowsysteem : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [Header("Harita Sýnýrlarý")]
    public bool limitMap = true;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    // --- TÝTREME DEÐÝÞKENLERÝ ---
    private float shakeDuration = 0f;    // Ne kadar sürecek?
    private float shakeMagnitude = 0.1f; // Ne kadar þiddetli olacak?
    private float dampingSpeed = 1.0f;   // Titreme ne kadar hýzlý azalsýn?
    Vector3 initialPosition;

    void FixedUpdate()
    {
        if (target == null) return;

        // 1. Normal Takip Pozisyonunu Hesapla
        Vector3 desiredPosition = target.position + offset;

        // Harita Sýnýrlarý (Varsa)
        if (limitMap)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 2. TÝTREME EFEKTÝNÝ EKLE
        if (shakeDuration > 0)
        {
            // Rastgele bir yöne kaydýr (X ve Y ekseninde)
            Vector3 shakeOffset = Random.insideUnitCircle * shakeMagnitude;

            // Mevcut pozisyona titremeyi ekle
            transform.position = smoothedPosition + shakeOffset;

            // Süreyi azalt
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Titreme yoksa normal pozisyona git
            transform.position = smoothedPosition;
        }
    }

    // --- BU FONKSÝYONU ÇAÐIRACAÐIZ ---
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
