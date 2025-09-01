using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Tooltip("Value of one collectible.")]
    [SerializeField] private int scoreValue = 1;

    // Called when another collider enters this trigger collider.
    private void OnTriggerEnter(Collider other)
    {
        // Check ig object entered has player tag to precvent triggers from non player objects.
        if (other.CompareTag("Player")){

            ScoreManager.Instance.AddScore(scoreValue);

            // Respawn
            CollectibleSpawner.Instance.SpawnSingleCollectible();

            // Destroy collectivle of attached script.
            Destroy(gameObject);
        }
    }
}

// @Author F.B.