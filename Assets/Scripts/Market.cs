using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public GameObject MoneyAddedText;
    [SerializeField] private Image[] arrowsIcon;
    public TextMeshProUGUI[] PriceTexts;
    [SerializeField] private float[] ResourcesPrice;
    private float[] ResourcesPriceOld;
    [SerializeField] private float[] _basePrices;
    [SerializeField] private Color[] _priceColors;
    private RectTransform[] _arrowsRectTransform;
    GameObject Clone;

    private Wallet _wallet;

    public float GetResourcePrice(int id)
    {
        return ResourcesPrice[id];
    }

    private void Awake()
    {
        ResourcesPriceOld = new float[ResourcesPrice.Length];
        _basePrices = new float[ResourcesPrice.Length];
        _arrowsRectTransform = new RectTransform[arrowsIcon.Length];

        for (int i = 0; i < arrowsIcon.Length; i++)
            _arrowsRectTransform[i] = arrowsIcon[i].GetComponent<RectTransform>();

        // if (PlayerPrefs.HasKey("savedPrices0"))
        if (PlayerPrefs.HasKey("savedPrices0"))
        {
            for (int i = 0; i < ResourcesPrice.Length; i++)
            {
                // ResourcesPriceOld[i] = PlayerPrefs.GetFloat("savedPricesOld" + i);
                ResourcesPriceOld[i] = PlayerPrefs.GetFloat("savedPricesOld" + i);
                // _basePrices[i] = PlayerPrefs.GetFloat("savedPricesBase" + i);
                _basePrices[i] = PlayerPrefs.GetFloat("savedPricesBase" + i);
            }

            if (TimeLastSession.ts.TotalSeconds >= 5)
            {
                for (int i = 0; i < ResourcesPrice.Length; i++)
                {
                    // ResourcesPriceOld[i] = PlayerPrefs.GetFloat("savedPrices" + i);
                    ResourcesPriceOld[i] = PlayerPrefs.GetFloat("savedPrices" + i);
                    ResourcesPrice[i] = _basePrices[i] + (_basePrices[i] * Random.Range(-.4f, .8f));
                }

                CheckTextColorPrices();
                saveMarketPrices();
            }
            else
            {
                for (int i = 0; i < ResourcesPrice.Length; i++)
                {
                    // ResourcesPrice[i] = PlayerPrefs.GetFloat("savedPrices" + i);
                    ResourcesPrice[i] = PlayerPrefs.GetFloat("savedPrices" + i);
                }

                CheckTextColorPrices();
            }
        }
        else
        {
            ResourcesPriceOld = ResourcesPrice;
            _basePrices = ResourcesPrice;

            saveMarketPrices();
        }
    }

    private void Start()
    {
        CheckTextsPrice();

        _wallet = FindObjectOfType<Wallet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPC_Porter>(out NPC_Porter npc_porter))
        {
            _wallet.AddMoney((int) (npc_porter.GetQuantityProduct() * ResourcesPrice[npc_porter.GetPorterId() - 1]));
            MoneyAddedText.GetComponentInChildren<TextMeshPro>().text =
                "+" + (int) (npc_porter.GetQuantityProduct() * ResourcesPrice[npc_porter.GetPorterId() - 1]);
            Clone = Instantiate(MoneyAddedText, transform.position, Quaternion.Euler(new Vector3(55, 15, 0)));
            Destroy(Clone, 1.5f);
            npc_porter.GoBack();
            npc_porter.done = true;
        }
    }

    private void CheckTextColorPrices()
    {
        for (int i = 0; i < ResourcesPrice.Length; i++)
        {
            if (ResourcesPrice[i] > ResourcesPriceOld[i])
            {
                PriceTexts[i].color = _priceColors[1];
                arrowsIcon[i].color = _priceColors[1];
                _arrowsRectTransform[i].localScale = new Vector3(1, 1, 1);
            }
            else if (ResourcesPrice[i] < ResourcesPriceOld[i])
            {
                PriceTexts[i].color = _priceColors[2];
                arrowsIcon[i].color = _priceColors[2];
                _arrowsRectTransform[i].localScale = new Vector3(1, -1, 1);
            }
            else
            {
                PriceTexts[i].color = _priceColors[0];
                _arrowsRectTransform[i].gameObject.SetActive(false);
            }
        }
    }

    private void CheckTextsPrice()
    {
        for (int i = 0; i < PriceTexts.Length; i++)
        {
            if (ResourcesPrice[i] >= 1000)
                PriceTexts[i].text = FormatMoneys.FormatMoney(ResourcesPrice[i]);
            else
                PriceTexts[i].text = ResourcesPrice[i].ToString("f1");
        }
    }

    private void saveMarketPrices()
    {
        for (int i = 0; i < ResourcesPrice.Length; i++)
        {
            PlayerPrefs.SetFloat("savedPrices" + i, ResourcesPrice[i]);
            PlayerPrefs.SetFloat("savedPricesOld" + i, ResourcesPriceOld[i]);
            PlayerPrefs.SetFloat("savedPricesBase" + i, _basePrices[i]);
        }
    }
}
