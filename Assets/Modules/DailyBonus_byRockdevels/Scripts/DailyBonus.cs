using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyBonus : MonoBehaviour
{
    [SerializeField] private Image[] _rewardedBg;
    [SerializeField] private Sprite _rewardedActiveBg;
    [SerializeField] private int[] _rewardQuantity;
    [SerializeField] private GameObject[] activeObject;
    private int _bonusDay;
    private Wallet _wallet;
    private Tawnhall _tawnhall;
    private Animator _animator;
    private AudioSource _audioSource;

    void Start()
    {
        _wallet = FindObjectOfType<Wallet>();
        _tawnhall = FindObjectOfType<Tawnhall>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        //if (!PlayerPrefs.HasKey("PlayDate"))
        if (!PlayerPrefs.HasKey("PlayDate"))
        {
            // PlayerPrefs.SetString("PlayDate", Convert.ToString(DateTime.Now));
            PlayerPrefs.SetString("PlayDate", Convert.ToString(DateTime.Now));
            _animator.SetBool("show", true);
        }
           

        // _bonusDay = PlayerPrefs.GetInt("_bonusDay");
        _bonusDay = PlayerPrefs.GetInt("_bonusDay");

        dayCheck();
    }

    public void dayCheck()
    {
        string stringDate = PlayerPrefs.GetString("PlayDate");
        DateTime oldDate = DateTime.Parse(stringDate);
        DateTime newDate = DateTime.Now;
        
        TimeSpan difference = newDate.Subtract(oldDate);

        Debug.Log("HOURS" + difference.Hours);
        Debug.Log("DAYS" + difference.Days);

        if (difference.Hours > 16 || difference.Days == 1)
        {
            Debug.Log("New Reward!");

            if (_bonusDay >= 4)
            {
                ResetCounter();
            }
            else
            {
                _bonusDay++;
                // PlayerPrefs.SetInt("_bonusDay", _bonusDay);
                PlayerPrefs.SetInt("_bonusDay", _bonusDay);
                _animator.SetBool("show", true);
            }               

            string newStringDate = Convert.ToString(newDate);
            // PlayerPrefs.SetString("PlayDate", newStringDate);
            PlayerPrefs.SetString("PlayDate", newStringDate);
        
        }
        else if (difference.Days > 1)
        {
            ResetCounter();
        }

        CheckBonus();
    }

    private void ResetCounter()
    {
        _bonusDay = 0;
        // PlayerPrefs.SetInt("_bonusDay", 0);
        PlayerPrefs.SetInt("_bonusDay", 0);
        _animator.SetBool("show", true);
        // PlayerPrefs.DeleteKey("PlayDate");
        PlayerPrefs.DeleteKey("PlayDate");
    }

    private void CheckBonus()
    {
        for(int i=0; i<_bonusDay+1; i++)
        {
            _rewardedBg[i].sprite = _rewardedActiveBg;
            activeObject[i].SetActive(true);
        }       
    }

    public void CloseWindow()
    {
        _audioSource.Play();
        _animator.SetBool("show", false);

        switch (_bonusDay)
        {
            case 0:
                _wallet.AddMoney(_rewardQuantity[_bonusDay]);
                break;
            
            case 1:
                _wallet.AddMoney(_rewardQuantity[_bonusDay]);
                break;
            
            case 2:
                _wallet.AddMoney(_rewardQuantity[_bonusDay]);
                _wallet.AddGem(10);
                break;
            
            case 3:
                _wallet.AddMoney(_rewardQuantity[_bonusDay]);
                _wallet.AddGem(10);
                break;
            
            case 4:
                _tawnhall.BonusWarrior(4);
                break;
        }
    }
}
