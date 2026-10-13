using UnityEngine;

public class ataque : MonoBehaviour
{
    public float velocidade = 8f;
    public float tempoDeVida = 2f;

    private Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Disparar(int direcao)
    {
        rb2d.linearVelocity = new Vector2(direcao * velocidade, 0f);
        GetComponent<SpriteRenderer>().flipX = direcao == -1; // ajuste se o sprite olhar para o outro lado
        Destroy(gameObject, tempoDeVida); // some sozinha se não acertar nada
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("inimigo"))
        {
            if (outro.TryGetComponent<moveInimigo>(out var inimigo))
                inimigo.Substituir();
            else
                Destroy(outro.gameObject); 

            Destroy(gameObject);
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.AdicionarPontos(10); 
                gameManager.DerrotarInimigo();
            }
                
        }
        else if (outro.CompareTag("bau"))
        {
            moveItens scriptItem = outro.GetComponent<moveItens>();

            if(scriptItem != null)
            {
                scriptItem.AtivarItem();
            }

            Destroy(outro.gameObject);
            Destroy(gameObject);
        }
        else if (outro.gameObject.layer == LayerMask.NameToLayer("Chao"))
        {
            Destroy(gameObject);
        }
    }
}