using System;
using UnityEngine;
using webgl;

public class Wallet : MonoBehaviour
{
    #region Singleton

    public event Action<float> EarnMoneyChange;
    public event Action<int> GemChange;
    public event Action<float> MoneyChange;
    private float _earnMoney;
    private float _money;
    private int _gem;
    public int GetGem => _gem;

    void Awake()
    {
        _earnMoney = 0;

        // if (PlayerPrefs.HasKey("_money"))
        if (PlayerPrefs.HasKey("_money"))
            // _money = PlayerPrefs.GetFloat("_money");
            _money = PlayerPrefs.GetFloat("_money");
        else
            AddMoney(100);

        // if (PlayerPrefs.HasKey("_gem"))
        if (PlayerPrefs.HasKey("_gem"))
            // _gem = PlayerPrefs.GetInt("_gem");
            _gem = PlayerPrefs.GetInt("_gem");
        else
            AddGem(1);

        InApps.OnGold5000 += () => AddMoney(5000);
        InApps.OnGold20000 += () => AddMoney(20000);
        InApps.OnGold100000 += () => AddMoney(100000);
        InApps.OnGold1500000 += () => AddMoney(1500000);
    }

    private void Start()
    {
        AddMoney(0);
        AddGem(112);
    }

    #endregion

    public float GetMoney()
    {
        return _money;
    }

    public float GetEarnMoney()
    {
        return _earnMoney;
    }

    public void UseMoney(float price)
    {
        _money -= price;
        SaveMoney();
        MoneyChange?.Invoke(_money);
    }

    public void AddEarnMoney(float earnMoney)
    {
        _earnMoney += earnMoney;
        EarnMoneyChange?.Invoke(_earnMoney);
    }

    public void AddMoney(float amountMoney)
    {
        _money += amountMoney;
        SaveMoney();
        MoneyChange?.Invoke(_money);
    }

    public void AddGem(int value)
    {
        _gem += value;
        SaveGem();
        GemChange?.Invoke(_gem);
    }

    public void UseGem(int value)
    {
        _gem -= value;
        SaveGem();
        GemChange?.Invoke(_gem);
    }

    private void SaveGem()
    {
        // PlayerPrefs.SetInt("_gem", _gem);
        PlayerPrefs.SetInt("_gem", _gem);
    }

    private void SaveMoney()
    {
        // PlayerPrefs.SetFloat("_money", _money);
        PlayerPrefs.SetFloat("_money", _money);
    }
}
