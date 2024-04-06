using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private int itemPrice;

    [SerializeField] private GameController gameController;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        if (gameController != null)
        {
            gameController.BuyItem(itemPrice);
        }
    }
}
