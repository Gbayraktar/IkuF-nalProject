using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;    // Düþmanýn maksimum caný
    private int currentHealth;     // O anki caný (kod içinde deðiþecek)

    [Header("Hareket Ayarlarý")]
    public Transform player;
    public float speed = 3f;
    public float followRange = 5f;

    void Start()
    {
        // Oyuna baþlarken caný fulle
        currentHealth = maxHealth;

        // Player'ý otomatik bulma (Eðer sürükleyip býrakmadýysan)
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. MESAFE VE TAKÝP KODLARI
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < followRange)
        {
            // Takip ederken sürekli log atmasýn diye bu satýrý yoruma aldým, istersen açabilirsin:
            // Debug.Log("DÜÞMAN: Takip ediyor..."); 
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    // --- CAN VE HASAR ALMA SÝSTEMÝ ---

    // Bu fonksiyonu dýþarýdan (silahýndan) çaðýracaðýz
    public void TakeDamage(int damageAmount)
    {
        // Caný azalt
        currentHealth -= damageAmount;

        Debug.Log("DÜÞMAN: Hasar aldý! Kalan Can: " + currentHealth);

        // Can 0 veya altýna düþtü mü kontrol et
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("DÜÞMAN: Öldü ve yok ediliyor!");

        // Efekt eklemek istersen buraya yazabilirsin (Örn: Patlama efekti)

        Destroy(gameObject); // Düþmaný sahneden sil
    }

    // --- PLAYER'A ÇARPMA KISMI ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("DÜÞMAN: Player'a çarptý! Player ölüyor.");
            Destroy(collision.gameObject);
        }
    }

    // --- TEST ÝÇÝN (SÝLEBÝLÝRSÝN) ---
    // Oyun çalýþýrken düþmanýn üzerine mouse ile týklarsan 20 caný gider.
    private void OnMouseDown()
    {
        TakeDamage(20);
    }

    // Menzili çizme (Gizmos)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
    }
}
