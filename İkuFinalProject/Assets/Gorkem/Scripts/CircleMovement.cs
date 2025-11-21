using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float donmeHizi = 50f;

    void Update()
    {
        // Z ekseninde sürekli döndürür
        transform.Rotate(0, 0, donmeHizi * Time.deltaTime);
    }
}
