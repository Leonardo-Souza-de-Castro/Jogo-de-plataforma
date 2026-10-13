
using UnityEngine;
using UnityEngine.UI;

public class movePlayer : MonoBehaviour
{
    // Kaycaps movement
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode up = KeyCode.Space;
    public KeyCode golpe = KeyCode.Z;

    // Ground check
    private bool estaNoChao;
    public LayerMask camadaDoChao;
    public Transform verificadorDoChao;

    // Attack
    public GameObject balaPrefab;
    public Transform pontoDeTiro;

    // Stats
    public float speed = 1.0f;
    public float maxX = 8.38f;
    public float minX = -8.36f;
    public float forcaDoPulo = 7f;
    public float impulsoPuloHorizontal = 3f;
    private int vidas = 3;

    // Corações da interface
    public GameObject[] coracoes;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer sr;

    private int direcao = 0; // 1 = direita, -1 = esquerda

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb2d.freezeRotation = true;

        sr.flipX = false;
    }

    void Update()
    {
        Movimentacao();

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            Pular();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Atacar();
        }
    }

    void Movimentacao()
    {
        var vel = rb2d.linearVelocity;

        if (Input.GetKey(moveRight))
        {
            vel.x = speed;
            direcao = 1;
            sr.flipX = false;
            animator.SetFloat("Direcao", 1);
        }
        else if (Input.GetKey(moveLeft))
        {
            vel.x = -speed;
            direcao = -1;
            sr.flipX = true;
            animator.SetFloat("Direcao", 1);
        }
        else if (estaNoChao)
        {
            vel.x = 0;
            animator.SetFloat("Direcao", 0);
        }

        rb2d.linearVelocity = vel;

        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    void Atacar()
    {
        animator.SetTrigger("Atacando");
    }

    public void Atirar()
    {
        int dir = sr.flipX ? -1 : 1;

        GameObject bala = Instantiate(
            balaPrefab,
            pontoDeTiro.position,
            Quaternion.identity
        );

        if (bala.TryGetComponent<ataque>(out var scriptAtaque))
            scriptAtaque.Disparar(dir);
    }

    void Pular()
    {
        animator.SetFloat("Pulando", 1);

        rb2d.linearVelocity = new Vector2(
            direcao * impulsoPuloHorizontal,
            forcaDoPulo
        );
    }

    void FixedUpdate()
    {
        estaNoChao = Physics2D.OverlapCircle(
            verificadorDoChao.position,
            0.2f,
            camadaDoChao
        );

        animator.SetFloat("Pulando", estaNoChao ? 0 : 1);
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if ((camadaDoChao.value & (1 << colisao.gameObject.layer)) > 0)
        {
            if (colisao.contacts[0].normal.y > 0.5f)
            {
                animator.SetFloat("Pulando", 0);
            }
        }
    }

    
    public void PerderVida()
    {
        if (vidas <= 0)
            return;

        vidas--;

        // Esconde um coração a cada vida perdida
        if (coracoes != null && vidas < coracoes.Length)
        {
            if (coracoes[vidas] != null)
                coracoes[vidas].SetActive(false);
        }

        Debug.Log("Vidas restantes: " + vidas);

        if (vidas <= 0)
        {
            animator.SetTrigger("Vida");
            rb2d.linearVelocity = Vector2.zero;

            // Impede movimentação física, mantendo o Animator ativo
            rb2d.bodyType = RigidbodyType2D.Kinematic;

            // Desativa apenas a movimentação e os controles do script
            this.enabled = false;

            StartCoroutine(AguardarMorte());
        }
    }

    private System.Collections.IEnumerator AguardarMorte()
    {
        // Aguarda a animação de morte terminar
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length
        );

        var gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    public void AdicionarVida()
    {
        if (vidas >= 3)
            return;

        vidas++;

        // Mostra o coração correspondente à vida recuperada
        if (coracoes != null && vidas - 1 < coracoes.Length)
        {
            if (coracoes[vidas - 1] != null)
                coracoes[vidas - 1].SetActive(true);
        }

        Debug.Log("Vidas restantes: " + vidas);
    }

}
