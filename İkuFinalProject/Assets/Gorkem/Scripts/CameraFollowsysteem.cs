using UnityEngine;

public class CameraFollowsysteem : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target;       // Takip edilecek obje (Player)
    public float smoothSpeed = 0.125f; // Takip etme yumuþaklýðý (0 ile 1 arasý)

    public Vector3 offset;         // Kamera karakterin tam neresinde duracak?

    void LateUpdate()
    {
        if (target == null) return; // Player ölürse hata vermesin

        // 1. Gitmek istediðimiz pozisyon (Player + Offset)
        Vector3 desiredPosition = target.position + offset;

        // 2. Yumuþak geçiþ (Lerp) hesaplama
        // Mevcut pozisyondan, istenen pozisyona 'smoothSpeed' hýzýyla kay
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. Kamerayý hareket ettir
        transform.position = smoothedPosition;
    }
}
