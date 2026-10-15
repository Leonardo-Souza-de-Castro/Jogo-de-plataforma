using UnityEngine;

public class NuvemAnimacao : MonoBehaviour
{
    public float velocidadeMin = 0.15f;
    public float velocidadeMax = 0.4f;

    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.speed = Random.Range(velocidadeMin, velocidadeMax);

        // começa em um ponto aleatório do ciclo, para as nuvens não trocarem de sprite juntas
        anim.Play(0, 0, Random.value);
    }
}