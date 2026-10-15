
using UnityEngine;

public class buraco : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo entrou no detector: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador caiu no buraco!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Debug.LogError("GameManager.Instance está nulo!");
            }
        }
    }
}
