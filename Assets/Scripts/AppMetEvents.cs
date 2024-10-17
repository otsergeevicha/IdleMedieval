using UnityEngine;
using System.Collections.Generic;
//using GameAnalyticsSDK;

public class AppMetEvents : MonoBehaviour
{
    private Dictionary<string, object> eventParameters = new Dictionary<string, object>();

    public void LevelStartEvent(int _level, int _gameCount)
    {
        eventParameters["level"] = _level.ToString();
        eventParameters["level_count"] = _gameCount.ToString();
        eventParameters["level_random"] = "0";
        eventParameters.Clear();
    }

    public void LevelFinishEvent(int _level, string _result, int _gameCount,float _gametimer)
    {
        eventParameters["level"] = _level.ToString();
        eventParameters["result"] = _result;
        eventParameters["level_count"] = _gameCount.ToString();
        eventParameters["level_random"] = "0";
        eventParameters["level_time"] = _gametimer.ToString("f0");
        eventParameters.Clear();
    }

    public void VideoAdsAvailable(string _placement)
    {
        eventParameters["ad_type"] = "rewarded";
        eventParameters["placement"] = _placement;
        eventParameters["result"] = "success";
        eventParameters.Clear();
    }

    public void VideoAdsStartded(string _placement)
    {
        eventParameters["ad_type"] = "rewarded";
        eventParameters["placement"] = _placement;
        eventParameters["result"] = "start";
        eventParameters.Clear();
    }


    public void VideoAdsWatch(string _result, string _placement)
    {
        eventParameters["ad_type"] = "rewarded";
        eventParameters["placement"] = _placement;
        eventParameters["result"] = _result;
       // GameAnalytics.NewDesignEvent("rewarded"+_placement, 1);
        eventParameters.Clear();
    }

    public void IAP(string id, string currency, string price)
    {
        eventParameters["inapp_id"] = id;
        eventParameters["currency"] = currency;
        eventParameters["price"] = price;
        eventParameters["inapp_type"] = "buy_gems";
        eventParameters.Clear();
    }

    public void IAP_Gold(string id, string currency, string price)
    {
        eventParameters["inapp_id"] = id;
        eventParameters["currency"] = currency;
        eventParameters["price"] = price;
        eventParameters["inapp_type"] = "buy_Gold";
        eventParameters.Clear();
    }

    public static void SendEvent(string eventTitle)
    {
      //  GameAnalytics.NewDesignEvent(eventTitle, 1);
    }

    public static void SendProgressionEvent()//GAProgressionStatus status, int battleID)
    {
      //  GameAnalytics.NewProgressionEvent(status, "Battle_ID" + battleID);
    }
}
