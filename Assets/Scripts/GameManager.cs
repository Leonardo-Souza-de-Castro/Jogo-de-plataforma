
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pontuação")]
    public static int pontuacao = 0;
    public static int vidas = 3;

    public TMP_Text scoreText;

    [Header("UI Game Over")]
    public GameObject painelGameOver;
    public TMP_Text textoScoreFinalGameOver;
    public Button btnReiniciar;

    [Header("UI Game Win")]
    public GameObject painelGameWin;
    public TMP_Text textoScoreFinalWin;
    public Button btnReiniciar2;

    private bool gameAcabou = false;
    private int inimigosRestantes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (painelGameOver != null)
            painelGameOver.SetActive(false);

        if (painelGameWin != null)
            painelGameWin.SetActive(false);

        if (btnReiniciar != null)
            btnReiniciar.onClick.AddListener(ReiniciarJogo);

        if (btnReiniciar2 != null)
            btnReiniciar2.onClick.AddListener(ReiniciarJogo);

        AtualizarPontuacao();

        inimigosRestantes =
            GameObject.FindGameObjectsWithTag("inimigo").Length;

        if (inimigosRestantes == 0)
            ProximaFase();
    }

    public void DerrotarInimigo()
    {
        if (gameAcabou || inimigosRestantes <= 0)
            return;

        inimigosRestantes--;

        if (inimigosRestantes == 0)
            ProximaFase();
    }

    void ProximaFase()
    {
        string faseAtual = SceneManager.GetActiveScene().name;

        if (faseAtual == "Fase1")
        {
            SceneManager.LoadScene("Fase2");
        }
        else if (faseAtual == "Fase2")
        {
            Win();
        }
    }

    public void AdicionarPontos(int pontos)
    {
        if (gameAcabou)
            return;

        pontuacao += pontos;
        AtualizarPontuacao();
    }

    void AtualizarPontuacao()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + pontuacao;
    }

    public void GameOver()
    {
        if (gameAcabou)
            return;

        gameAcabou = true;

        if (painelGameOver != null)
            painelGameOver.SetActive(true);

        if (textoScoreFinalGameOver != null)
            textoScoreFinalGameOver.text =
                "Pontuacao final: " + pontuacao;

        Time.timeScale = 0f;
    }

    public void Win()
    {
        if (gameAcabou)
            return;

        gameAcabou = true;

        if (painelGameWin != null)
            painelGameWin.SetActive(true);

        if (textoScoreFinalWin != null)
            textoScoreFinalWin.text =
                "Pontuacao final: " + pontuacao;

        Time.timeScale = 0f;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;

        pontuacao = 0;
        vidas = 3;
        gameAcabou = false;

        SceneManager.LoadScene("Inicio");
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
