using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 30;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
        Destroy(gameObject, 3f);
    }

    // "Is Trigger" kutucu�u i�aretliyse bu �al���r
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. �nce neye �arpt���m�z� g�relim
        Debug.Log("Mermi bir �eye �arpt�: " + hitInfo.name);

        // 2. �arpt���m�z objenin �zerinde "EnemyAI" scripti var m� bakal�m?
        EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            Debug.Log("EVET! �arpt���m �ey bir D��man. Can� azalt�l�yor...");
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Mermiyi yok et
        }
        else
        {
            Debug.Log("HAYIR! �arpt���m �eyde 'EnemyAI' scripti YOK. Neye �arpt�m? -> " + hitInfo.name);
        }
    }
}
