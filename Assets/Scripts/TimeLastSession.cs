using System;
using UnityEngine;

public class TimeLastSession : MonoBehaviour
{
   public static TimeSpan ts;
    private Wallet _wallet;

    private void Awake()
    {
        CheckTime();
        _wallet = FindObjectOfType<Wallet>();
    }    
    
    void CheckTime()
    {
        if (PlayerPrefs.HasKey("LastSession"))
        {
            ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSession"));            
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
        }
        
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
        }
    }
}
