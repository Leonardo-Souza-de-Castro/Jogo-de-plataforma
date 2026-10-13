
using System.Collections;
using UnityEngine;

public class moveInimigo : MonoBehaviour
{
    public float speed = 1.0f;
    public float maxX = 7.57f;
    public float minX = 3.5f;

    [Header("Detecção do jogador")]
    [SerializeField] private Transform player;
    [SerializeField] private float raioAtaque = 2.0f;
    [SerializeField] private float tempoAtaque = 1.0f;

    [Header("Derretido")]
    [SerializeField] private RuntimeAnimatorController controllerMorte;
    [SerializeField] private Vector2 tamanhoDerretido = new Vector2(0.02f, 0.02f);
    [SerializeField] private Vector2 tamanhoAtacando = new Vector2(0.6f, 0.31f);

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private bool estaVivo = true;
    private bool atacando = false;
    [SerializeField] private float intervaloDano = 1f;

    private float proximoDano = 0f;

    private int direcao = -1; // 1 = direita, -1 = esquerda

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        rb2d.freezeRotation = true;
        sr.flipX = direcao == 1;

        // Procura automaticamente o objeto com a tag Player
        if (player == null)
        {
            GameObject objetoPlayer = GameObject.FindGameObjectWithTag("Player");

            if (objetoPlayer != null)
                player = objetoPlayer.transform;
        }
    }

    void Update()
    {
        if (!estaVivo || player == null)
            return;

        float distancia = Vector2.Distance(
            transform.position,
            player.position
        );

        // O jogador entrou no raio de ataque
        if (distancia <= raioAtaque && !atacando)
        {
            StartCoroutine(ExecutarAtaque());
        }

        // Movimento e mudança de direção apenas quando não ataca
        if (atacando)
            return;

        float x = transform.position.x;

        if (x >= maxX)
            MudarDirecao(-1);
        else if (x <= minX)
            MudarDirecao(1);
    }

    void FixedUpdate()
    {
        if (!estaVivo || atacando)
        {
            rb2d.linearVelocity = Vector2.zero;
            return;
        }

        var vel = rb2d.linearVelocity;
        vel.x = direcao * speed;
        rb2d.linearVelocity = vel;
    }

    IEnumerator ExecutarAtaque()
    {
        atacando = true;

        rb2d.linearVelocity = Vector2.zero;

        if (player != null)
        {
            if (player.position.x > transform.position.x)
                sr.flipX = true;
            else
                sr.flipX = false;
        }

        animator.SetTrigger("Atacar");

        yield return new WaitForSeconds(tempoAtaque);

        if (!estaVivo)
            yield break;

        atacando = false;
    }

    void MudarDirecao(int novaDirecao)
    {
        if (direcao == novaDirecao)
            return;

        direcao = novaDirecao;
        sr.flipX = direcao == 1;
    }

    public void Substituir()
    {
        if (!estaVivo)
            return;

        estaVivo = false;
        StopAllCoroutines();

        rb2d.linearVelocity = Vector2.zero;

        gameObject.layer = LayerMask.NameToLayer("InimigoMorto");

        animator.runtimeAnimatorController = controllerMorte;

        float baseY = col.offset.y - col.size.y / 2f;

        col.size = tamanhoDerretido;

        col.offset = new Vector2(
            col.offset.x,
            baseY + tamanhoDerretido.y / 2f
        );
    }

    public void Atacar()
    {
        if (!estaVivo || atacando)
            return;

        StartCoroutine(ExecutarAtaque());
        float baseY = col.offset.y - col.size.y / 2f;

        col.size = tamanhoAtacando;

        col.offset = new Vector2(
            col.offset.x,
            baseY + tamanhoAtacando.y / 2f
        );
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time >= proximoDano)
        {
            var player = collision.gameObject.GetComponent<movePlayer>();

            if (player != null)
            {
                player.PerderVida();
                proximoDano = Time.time + intervaloDano;
            }
        }
    }

}
