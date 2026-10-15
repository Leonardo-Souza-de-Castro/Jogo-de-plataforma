using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Iniciar : MonoBehaviour
{
    public Button btnReiniciar;

    void Start()
    {
        btnReiniciar.onClick.AddListener(IniciarJogo);
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Fase1");
    }
}