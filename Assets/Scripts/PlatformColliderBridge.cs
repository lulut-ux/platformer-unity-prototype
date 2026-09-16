using UnityEngine;

// Attach this to the platform's GameObject (with collider). It detects collisions with Player and tells Player
public class PlatformColliderBridge : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            var player = col.collider.GetComponent<PlayerController>();
            var mp = GetComponent<MovingPlatform>();
            if (player != null && mp != null)
            {
                player.SetStandingPlatform(mp);
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            var player = col.collider.GetComponent<PlayerController>();
            var mp = GetComponent<MovingPlatform>();
            if (player != null && mp != null)
            {
                player.ClearStandingPlatform(mp);
            }
        }
    }
}
