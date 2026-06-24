using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!CompareTag("Proximity")) return;
        PlayerShoot player = other.GetComponentInParent<PlayerShoot>();

        if (player != null)
        {
            player.SetProximityState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!CompareTag("Proximity")) return;
        PlayerShoot player = other.GetComponent<PlayerShoot>();

        if (player != null)
        {
            player.SetProximityState(false);
        }
    }
}