using UnityEngine;
using TMPro;
//using GameAnalyticsSDK.Setup;
using System.Collections;
using webgl;

public class RandomBonus : MonoBehaviour
{
    public static float GlobalTimer;

    public GameObject UIBtn;
    public GameObject ADSButton;
    [SerializeField] private GameObject SunRayOmg;
    [SerializeField] private GameObject[] _bonusObjects;
    [SerializeField] private int[] _bonusGift;
    [SerializeField] private GameObject[] _btns;
    [SerializeField] private TextMeshProUGUI _globalTimerDispalyText;
    private int _currentCount;
    private float _spinTimer;
    private float _timer;
    private int _winCount;
    private bool spin;
    private Tawnhall _tawnhall;
    private Wallet _wallet;
    private Animator _animator;
    private AudioSource _audioSource;

    private float _x2timer;
    private bool _x2bool;


    private void Start()
    {
        Time.timeScale = 1;
        
        if (!PlayerPrefs.HasKey("tutorial"))
            UIBtn.SetActive(false);
        
        if (PlayerPrefs.GetFloat("GlobalTimer") - TimeLastSession.ts.TotalSeconds < 0)
        {
            GlobalTimer = 0;
            SunRayOmg.SetActive(true);
        }
        else
        {
            GlobalTimer = (float)(PlayerPrefs.GetFloat("GlobalTimer") - TimeLastSession.ts.TotalSeconds);
        }
        PlayerPrefs.SetFloat("GlobalTimer", GlobalTimer);

        _tawnhall = FindObjectOfType<Tawnhall>();
        _wallet = FindObjectOfType<Wallet>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (GlobalTimer > 0)
        {
            GlobalTimer -= Time.deltaTime;
            ADSButton.SetActive(true);
            _globalTimerDispalyText.text = string.Format("{0:0}:{1:00}:{2:00}", (int)(GlobalTimer / 3600), (int)(GlobalTimer / 60 - (int)(GlobalTimer / 3600) * 60), (int)((GlobalTimer - (int)(GlobalTimer / 60) * 60))); // мин. и секунды
        }
        else
        {
            GlobalTimer = 0;
            SunRayOmg.SetActive(true);
            ADSButton.SetActive(false);
            _globalTimerDispalyText.text = TextTranslator.GetText("ВЗЯТЬ", "GET");
        }

        if (_x2bool)
        {
            if (_x2timer > 0)
            {
                _x2timer -= Time.deltaTime;
            }
            else
            {
                Time.timeScale = 1;
                _x2timer = 0;
                _x2bool = false;
            }
        }


        if (spin)
        {
            if (_spinTimer > 0)
            {
                _spinTimer -= Time.deltaTime;
                _timer += Time.deltaTime;

                if (_timer > .1f)
                {
                    _timer = 0;
                    _bonusObjects[_currentCount].SetActive(false);
                    _currentCount = Random.Range(0, _bonusObjects.Length);
                    _bonusObjects[_currentCount].SetActive(true);
                }
            }
            else
            {
                _spinTimer = 0;
                _bonusObjects[_currentCount].SetActive(false);
                _currentCount = Random.Range(0, _bonusObjects.Length - 1);
                _bonusObjects[_currentCount].SetActive(true);
                _btns[1].SetActive(true);
                spin = false;
            }
        }
    }

    public void SpinBtn()
    {
        _spinTimer = 3;
        spin = true;
        _btns[0].SetActive(false);
    }

    public void TakeBonus()
    {
        if (_currentCount <= 5)
        {
            _tawnhall.BonusWarrior(_currentCount);
        }
        else
        {
            _wallet.AddMoney(_bonusGift[_currentCount]);
        }

        GlobalTimer = 36000;
        PlayerPrefs.SetFloat("GlobalTimer", GlobalTimer);
        _btns[1].SetActive(false);
        _animator.SetBool("show", false);
        _audioSource.Play();
        StartCoroutine(BackToDefault());
    }

    public void OpenWindow() 
    {
        if (GlobalTimer > 0) return;
        _animator.SetBool("show", true);
    }

    public void AdsOpenWindow()
    {
        _animator.SetBool("show", true);
    }

    public void x2Activate()
    {
        Time.timeScale = 2;
        _x2timer = 120;
        _x2bool = true;        
    }

    IEnumerator BackToDefault()
    {
        yield return new WaitForSeconds(1.5f);
        _btns[0].SetActive(true);
        _bonusObjects[_currentCount].SetActive(false);
    }
}
