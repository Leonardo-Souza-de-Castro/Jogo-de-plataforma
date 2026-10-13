
using UnityEngine;

public class moveItens : MonoBehaviour
{
    public GameObject Item;

    void Start()
    {
        if (Item != null)
            Item.SetActive(false);
    }

    public void AtivarItem()
    {
        if (Item != null)
        {
            Item.transform.SetParent(null);
            Item.SetActive(true);
        }
    }
}
