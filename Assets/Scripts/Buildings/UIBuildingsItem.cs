using System;
using TMPro;
using UnityEngine;
using webgl;

public class UIBuildingsItem : MonoBehaviour
{
    [SerializeField] private Build _buildings;
    private BuildingsManager _buildingsManager;
    private Wallet _wallet;
    private int UpgradeBuildPrice;
    private AudioSource _audioSource;
    public TextMeshProUGUI BuildPriceText;
    public TextMeshProUGUI BuildGemPriceText;
    public TextMeshProUGUI UpdatePriceBuildText;
    public TextMeshProUGUI HireManagerPrice;

    public TextMeshProUGUI BuildLevelText;

    //public TextMeshProUGUI RevenueDisplayText;
    public TextMeshProUGUI CycleTimeDisplayText;
    public TextMeshProUGUI VolumeeDisplayText;
    public GameObject UILevelElement;
    public GameObject UImanagerElement;
    public GameObject[] BtnsWrappers;
    private Tutorial _tutorial;

    private void Start()
    {
        _wallet = FindObjectOfType<Wallet>();
        _buildingsManager = FindObjectOfType<BuildingsManager>();
        _tutorial = FindObjectOfType<Tutorial>();
        _audioSource = _buildingsManager.GetComponent<AudioSource>();

        UpgradeBuildPrice = GetUpgradePrice();

        CheckDisplayText();
        CheckBtnsCondition();
        Checkmanagers();
        CheckTexts();
    }

    private void CheckTexts()
    {
        CycleTimeDisplayText.text = $"{TextTranslator.GetText("Время", "Time")} - {_buildings.GetTimeCycle():f1} {TextTranslator.GetText("с", "s")}";
        VolumeeDisplayText.text = $"{TextTranslator.GetText("Макс", "Max")} - {_buildings.GetCapacity()}";
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

    public void UpgradeBuild()
    {
        if (_wallet.GetMoney() >= GetUpgradePrice())
        {
            _wallet.UseMoney(GetUpgradePrice());
            AdsUpgradeBuild();
        }
    }

    public void AdsUpgradeBuild()
    {
        _audioSource.Play();
        _buildings.UpgradeBuild();

        PlayerPrefs.SetInt("UpgradeBuildPrice" + _buildings.GetId(), UpgradeBuildPrice);
        CheckTexts();
        CheckDisplayText();
    }


    public void Checkmanagers()
    {
        if (_buildings.manager > 0)
        {
            UImanagerElement.SetActive(true);
            BtnsWrappers[2].SetActive(false); //кнопка покупки менеджера
            BtnsWrappers[3].SetActive(true); //муляж, менеджер уже куплен
        }
    }

    public void BuyBuiling()
    {
        if (_wallet.GetMoney() >= _buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice &&
            _wallet.GetGem >= _buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GemPrice)
        {
            _wallet.UseMoney(_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice);
            _wallet.UseGem(_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GemPrice);
            _buildings.BuildLevels++;
            _buildings.BuildNewBuilding();
            CheckDisplayText();
            CheckBtnsCondition();

            if (!_tutorial) return;
            if (_tutorial.GetTutorialStep == 2)
                _tutorial.NextTutorialStep(3);

            AppMetEvents.SendEvent("Buy_Building_ID-" + (_buildings.GetId() - 1));
        }
    }

    public void BuyManager()
    {
        if (_wallet.GetMoney() >= _buildingsManager.ManagerPrices[(_buildings.GetId() - 1)])
        {
            _wallet.UseMoney(_buildingsManager.ManagerPrices[(_buildings.GetId() - 1)]);
            _buildings.manager++;
            Checkmanagers();
        }
    }

    private int GetUpgradePrice()
    {
        return (int) ((_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice / 2) *
                      (float) Math.Pow(GameBalance.BuildingPriceUpgradeCoef, _buildings.BuildLevels));
    }

    private void CheckDisplayText()
    {
        BuildLevelText.text = _buildings.BuildLevels.ToString();
        BuildPriceText.text = FormatMoneys.FormatMoney(_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GoldPrice);
        BuildGemPriceText.text = FormatMoneys.FormatMoney(_buildingsManager.BuildPrices[(_buildings.GetId() - 1)].GemPrice);
        HireManagerPrice.text = FormatMoneys.FormatMoney(_buildingsManager.ManagerPrices[(_buildings.GetId() - 1)]);
        UpdatePriceBuildText.text = FormatMoneys.FormatMoney(GetUpgradePrice());
    }
}
