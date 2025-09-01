using UnityEngine;

public class GameOverWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.EndGame();
        }
    }
}

// @Author F.B.