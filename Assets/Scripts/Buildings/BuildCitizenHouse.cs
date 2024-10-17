using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class CitizwnEvent : UnityEvent<int> { }

public class BuildCitizenHouse : MonoBehaviour
{
    private int _buildLevel;
    private int _citizens;
    private Wallet _wallet;

    public CitizwnEvent CitizenChange;
    [Header("UI Build Elements")]
    public TextMeshProUGUI BuildLevelDisplayText;
    public TextMeshProUGUI CitizenDisplayText;
    public TextMeshProUGUI PriceDisplayText;

    void Awake()
    {
        _wallet = FindObjectOfType<Wallet>();

        // if (PlayerPrefs.HasKey("_citizens"))
        if (PlayerPrefs.HasKey("_citizens"))
        {
            // _citizens = PlayerPrefs.GetInt("_citizens");
            _citizens = PlayerPrefs.GetInt("_citizens");
            // _buildLevel = PlayerPrefs.GetInt("_citizens_buildLevel");
            _buildLevel = PlayerPrefs.GetInt("_citizens_buildLevel");
        }

        else
        {
            _citizens = 5;
            _buildLevel = 1;
        }

        CheckText();
    }

    public int GetCitizen()
    {
        return _citizens;
    }

    public void UseCitizen(int citizencount)
    {
        _citizens -= citizencount;
        CitizenChange.Invoke(_citizens);
        CheckText();
        SaveCitizen();
    }

    public void AddCitizen(int citizencount)
    {
        _citizens += citizencount;
        CitizenChange.Invoke(_citizens);
        _buildLevel++;
        CheckText();
        SaveCitizen();
    }

    private void SaveCitizen()
    {
        PlayerPrefs.SetInt("_citizens", _citizens);
        PlayerPrefs.SetInt("_citizens_buildLevel", _buildLevel);
    }

    private void CheckText()
    {
        BuildLevelDisplayText.text = _buildLevel.ToString();
        CitizenDisplayText.text = "Unemployed citizens " + _citizens.ToString();
        PriceDisplayText.text = FormatMoneys.FormatMoney((220 + _buildLevel * 50));
    }

    public void UpgradeBtn()
    {
        if(_wallet.GetMoney() >= (220 + _buildLevel * 50))
        {
            _wallet.UseMoney(220 + _buildLevel * 50);
            AddCitizen(5);
        }
    }
}
