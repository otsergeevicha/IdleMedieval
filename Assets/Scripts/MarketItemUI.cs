using TMPro;
using UnityEngine;

public class MarketItemUI : MonoBehaviour
{
    [SerializeField] private int id;
    private Market _market;
    public TextMeshProUGUI PriceText;

    private void Start()
    {
        _market = FindObjectOfType<Market>();
    }
}
