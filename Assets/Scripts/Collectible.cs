using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.AddScore(value);
            Destroy(gameObject);
        }
    }
}
