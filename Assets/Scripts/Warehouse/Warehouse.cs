using TMPro;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] warehouseDisplayText;
    private int[] _warehouse = new int[8];
    private Market _market;
    private Wallet _wallet;

    private void Awake()
    {
        for (int i = 0; i < _warehouse.Length; i++)
        {
            // _warehouse[i] = PlayerPrefs.GetInt("_warehouse" + i);
            _warehouse[i] = PlayerPrefs.GetInt("_warehouse" + i);
        }

        UpdateText();
    }

    private void Start()
    {
        _market = FindObjectOfType<Market>();
        _wallet = FindObjectOfType<Wallet>();
    }

    private void UpdateText()
    {
        for (int i = 0; i < _warehouse.Length; i++)
        {
            warehouseDisplayText[i].SetText(_warehouse[i].ToString());
        }
    }

    public void BuyResource(int id)
    {
        if (_wallet.GetMoney() >= _market.GetResourcePrice(id))
        {
            _wallet.UseMoney(_market.GetResourcePrice(id));
            _warehouse[id]++;
            PlayerPrefs.SetInt("_warehouse"+id, _warehouse[id]);
            UpdateText();
        }
    }

    public void SellResource(int id)
    {
        if (_warehouse[id] > 0)
        {
            _wallet.AddMoney(_market.GetResourcePrice(id));
            PlayerPrefs.SetInt("_warehouse"+id, _warehouse[id]);
            UpdateText();
        }
    }
   
}
