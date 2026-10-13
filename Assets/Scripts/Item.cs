
using UnityEngine;

public class Item : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        movePlayer player = collision.gameObject.GetComponent<movePlayer>();

        if (player != null)
        {
            player.AdicionarVida();
            Destroy(gameObject);
        }
    }
}
