using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hız Ayarları")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator; // Animasyon kontrolcüsü
    private bool isFacingRight = true; // Karakter sağa mı bakıyor?

    // Knockback (Geri tepme) için değişkenler
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Karakterin üzerindeki Animator bileşenini al
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isKnockedBack) return;

        // 1. Girdileri Al
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // 2. Animasyonu Ayarla
        // Eğer hareket vektörü (x veya y) 0 değilse, karakter yürüyordur
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // 3. Karakteri Döndür (Flip)
        // Eğer sağa gidiyorsa ve sağa bakmıyorsa -> Çevir
        if (movement.x > 0 && !isFacingRight)
        {
            Flip();
        }
        // Eğer sola gidiyorsa ve sağa bakıyorsa -> Çevir
        else if (movement.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (isKnockedBack) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Karakterin yönünü ters çeviren fonksiyon
    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Karakterin Scale değerini al, X'i -1 ile çarp ve geri yükle
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // --- KNOCKBACK SİSTEMİ (Önceki kodundan korundu) ---
    public void CallKnockback(float duration, float force, Transform enemyTransform)
    {
        StartCoroutine(KnockbackRoutine(duration, force, enemyTransform));
    }

    IEnumerator KnockbackRoutine(float duration, float force, Transform enemyTransform)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;

        Vector2 direction = (transform.position - enemyTransform.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}

