using UnityEngine;
using System;

public class RewardAds : MonoBehaviour
{
// #if UNITY_IOS
//      string rewardedAdUnitId = "86215ec23e0adbc1"; // IOS
// #else
//   string rewardedAdUnitId = "86215ec23e0adbc1"; // Android
// #endif


    int retryAttempt;

    // private AdsButton _adsbutton;
    // private Spells _adsSpells;
    private int _idRewardButton;

    private AppMetEvents _appMetEvents;

    // private GameUI _gameUI;
    private Wallet _wallet;
    private KingdomsWrapper _kingdomsWrapper;
    private Battleground _battleground;
    private RandomBonus _randomBonus;
    private Tawnhall tawnhall;
    private BuildCitizenHouse _buildCitizenHouse;
    [SerializeField] private UIBuildingsItem[] _upgradeBuildingItem;

    private void Start()
    {
        _wallet = FindObjectOfType<Wallet>();
        _kingdomsWrapper = FindObjectOfType<KingdomsWrapper>();
        _battleground = FindObjectOfType<Battleground>();
        _randomBonus = FindObjectOfType<RandomBonus>();
        _appMetEvents = FindObjectOfType<AppMetEvents>();
        tawnhall = FindObjectOfType<Tawnhall>();
        _buildCitizenHouse = FindObjectOfType<BuildCitizenHouse>();

        //MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        // {
        //   // AppLovin SDK is initialized, configure and start loading ads.
        //   Debug.Log("MAX SDK Initialized");
        //   InitializeRewardedAds();
        // };

        // MaxSdk.SetSdkKey("OP7huuk2dHdSduPkl8rGi8KhMnCZ8ewBCsRxzs_DD2U6lxranzNZ8SLzXwSHrLXoPnwNNFIsCtsvQ-iSV3DG8J");
        // MaxSdk.InitializeSdk();
    }

    private void OnDisable()
    {
        // MaxSdkCallbacks.OnRewardedAdLoadedEvent -= OnRewardedAdLoadedEvent;
        // MaxSdkCallbacks.OnRewardedAdLoadFailedEvent -= OnRewardedAdFailedEvent;
        // MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent -= OnRewardedAdFailedToDisplayEvent;
        // MaxSdkCallbacks.OnRewardedAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
        // MaxSdkCallbacks.OnRewardedAdClickedEvent -= OnRewardedAdClickedEvent;
        // MaxSdkCallbacks.OnRewardedAdHiddenEvent -= OnRewardedAdDismissedEvent;
        // MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        // MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        // MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        // MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        // MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        // MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        // MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        // MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        // MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to load 
        // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float) retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
    }

    private void OnRewardedAdClickedEvent(string adUnitId)
    {
    }

    private void OnRewardedAdDismissedEvent(string adUnitId)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    public void AdsGetReward(int _id)
    {
        _idRewardButton = _id;
        //MirraSDK.Ads.InvokeRewarded(OnRewardedAdReceivedRewardEvent);
    }

    private void OnRewardedAdReceivedRewardEvent()//string adUnitId, MaxSdk.Reward reward)
    {
        switch (_idRewardButton)
        {
            //X2 Bonus Earn
            case 1:
                _wallet.AddMoney(_wallet.GetEarnMoney());
                //_appMetEvents.VideoAdsWatch("watched", "get_bonus_X2_earn");
                break;

            //Instant Attack
            case 2:
                _kingdomsWrapper.AttackBtn();
                _battleground._adsSkipBattle = true;
                _battleground.CheckBattleStatus();
                //_appMetEvents.VideoAdsWatch("watched", "get_Instant_Attack");
                break;

            //X2 Winner
            case 3:
                _battleground.WinCloseBtn(1);
                //_appMetEvents.VideoAdsWatch("watched", "get_X2Gold_whenWinner");
                break;

            //random bonus
            case 4:
                _randomBonus.AdsOpenWindow();
                //_appMetEvents.VideoAdsWatch("watched", "get_randomBonus");
                break;

            //X2 timer
            case 5:
                _randomBonus.x2Activate();
                //_appMetEvents.VideoAdsWatch("watched", "get_X2_timer");
                break;

            //500 gold
            case 6:
                _wallet.AddMoney(500);
                //_appMetEvents.VideoAdsWatch("watched", "get_500Gold");
                break;

            case 7:
                tawnhall.AdsHireWarrior(0);
                //_appMetEvents.VideoAdsWatch("watched", "get_free_swordman");
                break;

            case 8:
                tawnhall.AdsHireWarrior(1);
                //_appMetEvents.VideoAdsWatch("watched", "get_free_archery");
                break;

            case 9:
                tawnhall.AdsHireWarrior(2);
               // _appMetEvents.VideoAdsWatch("watched", "get_free_heavySwordman");
                break;

            case 10:
                tawnhall.AdsHireWarrior(3);
                //_appMetEvents.VideoAdsWatch("watched", "get_free_crossbowman");
                break;

            case 11:
                tawnhall.AdsHireWarrior(4);
                //_appMetEvents.VideoAdsWatch("watched", "get_free_commander");
                break;

            //Upgrade Buildings

            case 12:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[0].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Farm");
                break;


            case 13:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[1].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Lambermill");
                break;


            case 14:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[2].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Blacksmith");
                break;

            case 15:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[3].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Workshop");
                break;

            case 16:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[4].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Alchemist");
                break;

            case 17:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[5].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Library");
                break;

            case 18:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[6].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Cowhouse");
                break;

            case 19:
                for (int i = 0; i < 3; i++)
                    _upgradeBuildingItem[7].AdsUpgradeBuild();
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Brewery");
                break;

            case 20:
                _buildCitizenHouse.AddCitizen(5);
                //_appMetEvents.VideoAdsWatch("watched", "get_upgrade_Cityzen");
                break;
        }

        // Time.timeScale = 1;
        //_audioManager.MuteAudio(false);
        _idRewardButton = 0;
    }
}
