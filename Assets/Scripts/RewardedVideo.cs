// using System;
// using UnityEngine;
// using UnityEngine.Advertisements;
//
// public class RewardedVideo : MonoBehaviour //IUnityAdsListener
// {
// #if UNITY_IOS
//     private string gameId = "3496815";
// #elif UNITY_ANDROID
//   private string gameId = "3496814";
// #endif
//
//
//   private int idReward;
//
//   public string myPlacementId = "rewardedVideo";
//   private Wallet _wallet;
//   private KingdomsWrapper _kingdomsWrapper;
//   private Battleground _battleground;
//   private RandomBonus _randomBonus;
//
//   void Start()
//   {
//     //Advertisement.AddListener(this);
//     //TODO:ADS
//     //Advertisement.Initialize(gameId, true);
//     _wallet = FindObjectOfType<Wallet>();
//     _kingdomsWrapper = GetComponent<KingdomsWrapper>();
//     _battleground = FindObjectOfType<Battleground>();
//     _randomBonus = FindObjectOfType<RandomBonus>();
//   }
//
//   // Implement a function for showing a rewarded video ad:
//   public void ShowRewardedVideo(int id)
//   {
//     idReward = id;
//     //Advertisement.Show(myPlacementId);
//   }
//
//   // Implement IUnityAdsListener interface methods:
//   public void OnUnityAdsReady(string placementId)
//   {
//     // If the ready Placement is rewarded, activate the button: 
//     if (placementId == myPlacementId)
//     {
//     }
//   }
//
//   public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
//   {
//     // Define conditional logic for each ad completion status:
//     if (showResult == ShowResult.Finished)
//     {
//       switch (idReward)
//       {
//         //X2 Bonus Earn
//         case 1:
//           _wallet.AddMoney(_wallet.GetEarnMoney());
//           break;
//
//         //Instant Attack
//         case 2:
//           _kingdomsWrapper.AttackBtn();
//           _battleground._adsSkipBattle = true;
//           _battleground.CheckBattleStatus();
//           break;
//
//         //X2 Winner
//         case 3:
//           _battleground.WinCloseBtn(1);
//           break;
//
//         //random bonus
//         case 4:
//           _randomBonus.AdsOpenWindow();
//           break;
//
//         //X2 timer
//         case 5:
//           _randomBonus.x2Activate();
//           break;
//
//         //500 gold
//         case 6:
//           _wallet.AddMoney(500);
//           break;
//       }
//
//       idReward = 0;
//       // Reward the user for watching the ad to completion.
//     }
//     else if (showResult == ShowResult.Skipped)
//     {
//       // Do not reward the user for skipping the ad.
//     }
//     else if (showResult == ShowResult.Failed)
//     {
//     }
//   }
//
// #if UNITY_IOS
// string adUnitId = "YOUR_IOS_AD_UNIT_ID";
// #else // UNITY_ANDROID
//   string adUnitId = "YOUR_ANDROID_AD_UNIT_ID";
// #endif
//   int retryAttempt;
//
//   public void InitializeRewardedAds()
//   {
//     // Attach callback
//     // MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
//     // MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
//
//     // Load the first rewarded ad
//     LoadRewardedAd();
//   }
//
//   private void LoadRewardedAd()
//   {
//    // MaxSdk.LoadRewardedAd(adUnitId);
//   }
//
//   private void OnRewardedAdLoadedEvent()//string adUnitId, MaxSdkBase.AdInfo adInfo)
//   {
//     // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
//
//     // Reset retry attempt
//     retryAttempt = 0;
//   }
//
//   private void OnRewardedAdLoadFailedEvent()//string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//   {
//     // Rewarded ad failed to load 
//     // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
//
//     retryAttempt++;
//     double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
//
//     Invoke("LoadRewardedAd", (float) retryDelay);
//   }
//
//   private void OnRewardedAdDisplayedEvent()//string adUnitId, MaxSdkBase.AdInfo adInfo)
//   {
//   }
//
//   private void OnRewardedAdFailedToDisplayEvent()//string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
//   //  MaxSdkBase.AdInfo adInfo)
//   {
//     // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
//     LoadRewardedAd();
//   }
//
//   private void OnRewardedAdClickedEvent()//string adUnitId, MaxSdkBase.AdInfo adInfo)
//   {
//   }
//
//   private void OnRewardedAdHiddenEvent()//string adUnitId, MaxSdkBase.AdInfo adInfo)
//   {
//     // Rewarded ad is hidden. Pre-load the next ad
//     LoadRewardedAd();
//   }
//
//   private void OnRewardedAdReceivedRewardEvent()//string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
//   {
//     // The rewarded ad displayed and the user should receive the reward.
//   }
//
//   private void OnRewardedAdRevenuePaidEvent()//string adUnitId, MaxSdkBase.AdInfo adInfo)
//   {
//     // Ad revenue paid. Use this callback to track user revenue.
//   }
// }