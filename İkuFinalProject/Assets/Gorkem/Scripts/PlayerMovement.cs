using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hýz Ayarlarý")]
    public float moveSpeed = 5f; // Karakterin yürüme hýzý

    private Rigidbody2D rb;
    private Vector2 movement; // X ve Y eksenindeki hareket yönünü tutar

    void Start()
    {
        // Karakterin üzerindeki Rigidbody2D bileþenini alýyoruz
        rb = GetComponent<Rigidbody2D>();
    }

    // Girdileri (Input) buradan alýrýz
    void Update()
    {
        // Yatay (A-D veya Sol-Sað Ok) ve Dikey (W-S veya Yukarý-Aþaðý Ok) girdileri al
        // GetAxisRaw kullanýyoruz ki karakter tuþu býrakýnca hemen dursun (buzda kayýyormuþ gibi olmasýn)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Çapraz giderken daha hýzlý gitmemesi için vektörü normalize et (Ýsteðe baðlý)
        movement = movement.normalized;
    }

    // Fizik iþlemlerini (hareket ettirmeyi) burada yaparýz
    void FixedUpdate()
    {
        // Mevcut pozisyon + (Yön * Hýz * Zaman)
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

