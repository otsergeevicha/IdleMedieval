using TMPro;
using UnityEngine;

public class UITempleItem : MonoBehaviour
{
    [SerializeField] private TempleBuild _buildings;
    private Wallet _wallet;
    private BuildingsManager _buildingsManager;
    public TextMeshProUGUI BuildPriceText;
    public GameObject UILevelElement;
    public GameObject[] BtnsWrappers;

    // Start is called before the first frame update
    void Start()
    {
        _wallet = FindObjectOfType<Wallet>();
        _buildingsManager = FindObjectOfType<BuildingsManager>();
        CheckBtnsCondition();
        BuildPriceText.text = FormatMoneys.FormatMoney(_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice);
    }

    public void CheckBtnsCondition()
    {
        if (_buildings.BuildLevels > 0)
        {
            BtnsWrappers[0].SetActive(false);
            UILevelElement.SetActive(true);
            BtnsWrappers[1].SetActive(true);
        }
    }

    public void BuyBuiling()
    {
        if (_wallet.GetMoney() >= _buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice)
        {
            _wallet.UseMoney(_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice);
            _buildings.BuildLevels++;
            _buildings.BuildNewBuilding();
            CheckBtnsCondition();
        }
    }
}
