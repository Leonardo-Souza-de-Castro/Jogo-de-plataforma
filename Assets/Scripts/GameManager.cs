using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pontuação")]
    public int pontuacao = 0;
    public TMP_Text scoreText; // Referência ao texto de score em jogo (HUD)

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
        // Garante que só existe um GameManager na cena
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelGameWin != null) painelGameWin.SetActive(false);

        if (btnReiniciar != null)
        {
            btnReiniciar.onClick.AddListener(ReiniciarJogo);
        }
        if (btnReiniciar2 != null)
        {
            btnReiniciar2.onClick.AddListener(ReiniciarJogo);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + pontuacao;
        }

        inimigosRestantes = GameObject.FindGameObjectsWithTag("inimigo").Length;

        if (inimigosRestantes == 0)
            ProximaFase();
    }

    public void DerrotarInimigo()
    {
        inimigosRestantes--;

        if (inimigosRestantes <= 0)
            ProximaFase();
    }

    void ProximaFase()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Fase1"){
            SceneManager.LoadScene("Fase2");
        }
    }

    public void AdicionarPontos(int pontos)
    {
        if (gameAcabou) return;

        pontuacao += pontos;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + pontuacao;
        }
    }

    public void GameOver()
    {
        if (gameAcabou) return;
        gameAcabou = true;

        if (painelGameOver != null) painelGameOver.SetActive(true);
        if (textoScoreFinalGameOver != null) textoScoreFinalGameOver.text = "Pontuacao final: " + pontuacao;

        Time.timeScale = 0f;
    }

    public void Win()
    {
        if (gameAcabou) return;
        gameAcabou = true;

        if (painelGameWin != null) painelGameWin.SetActive(true);
        if (textoScoreFinalWin != null) textoScoreFinalWin.text = "Pontuacao final: " + pontuacao;

        Time.timeScale = 0f;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        gameAcabou = false;
        pontuacao = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}