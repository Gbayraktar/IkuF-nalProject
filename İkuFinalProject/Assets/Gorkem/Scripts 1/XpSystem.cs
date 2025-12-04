using UnityEngine;

public class XpSystem : MonoBehaviour
{
    public int xpAmount = 20;
    public float magnetRange = 3f;
    public float flySpeed = 5f;

    private Transform player;
    private bool isMovingToPlayer = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < magnetRange || isMovingToPlayer)
        {
            isMovingToPlayer = true;
            transform.position = Vector2.MoveTowards(transform.position, player.position, flySpeed * Time.deltaTime);
            flySpeed += 10f * Time.deltaTime;
        }
    }

    // --- ÇARPIÞMA KONTROLÜ ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Önce çarpýþma algýlanýyor mu ona bakalým
        Debug.Log("XP TOPU BÝR ÞEYE DEÐDÝ! Deðdiði þeyin adý: " + other.name);

        // 2. Deðdiði þeyin etiketi "Player" mý?
        if (other.CompareTag("Player"))
        {
            Debug.Log("EVET! Deðen þey PLAYER.");

            // 3. Player'da LevelSystem scripti var mý?
            PlayerLevelSystem levels = other.GetComponent<PlayerLevelSystem>();
            if (levels != null)
            {
                Debug.Log("XP VERÝLÝYOR...");
                levels.GainExperience(xpAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("HATA: Player'a çarptým ama onda 'LevelSystem' scripti yok!");
            }
        }
        else
        {
            Debug.LogWarning("HAYIR! Deðen þey Player deðil. Deðen þeyin Tag'i: " + other.tag);
        }
    }
}
