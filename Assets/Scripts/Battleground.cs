using UnityEngine;
using TMPro;
using System.Collections;
using Random = UnityEngine.Random;

public class Battleground : MonoBehaviour
{
    private int _id;
    private int _swordmanTotal;
    private int _archeryTotal;
    private int _heavySwordman;
    private int _crossbowman;
    private int _commander;
    private int _knight;

    private int _reward;
    private int _rewardGem;
    private float _timer;
    private int _totalPlayerWarriors;
    private int _totalEnemyWarriors;

    public static int AtWar;
    public bool _adsSkipBattle;

    private KingdomsWrapper _kingdomsWrapper;
    private Tawnhall _tawnhall;
    private Wallet _wallet;
    private int battleStatus;
    public SaveBattleground _sv = new SaveBattleground(); //сохранение
    public Animator FrontNewsWrapper;
    public GameObject[] TypeOfNews;
    public TextMeshProUGUI RewardDisplayText, RewardGemDisplayText;

    private void Awake()
    {
        // AtWar = PlayerPrefs.GetInt("AtWar");
        AtWar = PlayerPrefs.GetInt("AtWar");
    }

    private void Start()
    {
        _kingdomsWrapper = FindObjectOfType<KingdomsWrapper>();
        _wallet = FindObjectOfType<Wallet>();
        _tawnhall = FindObjectOfType<Tawnhall>();
        // battleStatus = PlayerPrefs.GetInt("battleStatus");
        battleStatus = PlayerPrefs.GetInt("battleStatus");

        // if (PlayerPrefs.HasKey("Battle"))
        if (PlayerPrefs.HasKey("Battle"))
        {
            // _sv = JsonUtility.FromJson<SaveBattleground>(PlayerPrefs.GetString("Battle"));
            _sv = JsonUtility.FromJson<SaveBattleground>(PlayerPrefs.GetString("Battle"));
            _id = _sv.id;
            _swordmanTotal = _sv.swordmanTotal;
            _archeryTotal = _sv.archeryTotal;
            _heavySwordman = _sv.heavySwordman;
            _crossbowman = _sv.crossbowman;
            _commander = _sv.commander;
            _knight = _sv.knight;
            _reward = _sv.reward;
            _rewardGem = _sv.rewardGem;
            _timer = _sv.timer;

            _kingdomsWrapper.SetTimerActiveWar(_timer);
            // print(PlayerPrefs.GetString("Battle"));
            print(PlayerPrefs.GetString("Battle"));
        }


        CheckBattleStatus();
    }

    public void CheckBattleStatus()
    {
        if (AtWar > 0)
        {
            if (TimeLastSession.ts.TotalSeconds > _timer || _adsSkipBattle)
            {
                if (battleStatus == 2)
                {
                    TypeOfNews[1].SetActive(true);
                    RewardDisplayText.text = FormatMoneys.FormatMoney(_reward);
                    RewardGemDisplayText.text = FormatMoneys.FormatMoney(_rewardGem);
                    Tawnhall.BattleLevel = _id;
                    // PlayerPrefs.SetInt("BattleLevel", _id);
                    PlayerPrefs.SetInt("BattleLevel", _id);

                    AppMetEvents.SendEvent("End_Battle-" + _id+"_WIN");
          
                    foreach (KingdomsItem item in _kingdomsWrapper.Kingdomsitems)
                    {
                        if (_id + 1 == item.GetId())
                            item.CheckStatus();
                    }
                }
                else
                {
                    TypeOfNews[0].SetActive(true);
                    AppMetEvents.SendEvent("End_Battle-" + _id+"Lose");
                }

                AtWar = 0;
                // PlayerPrefs.SetInt("AtWar", AtWar);
                PlayerPrefs.SetInt("AtWar", AtWar);
                FrontNewsWrapper.SetBool("show", true);

                foreach (KingdomsItem item in _kingdomsWrapper.Kingdomsitems)
                {
                    if (_id == item.GetId())
                    {
                        item.ChangeStatus(battleStatus);
                        item.CheckStatus();
                    }
                }

                _adsSkipBattle = false;
            }
        }
    }


    public void WinCloseBtn(int coefficient)
    {
        if (battleStatus == 2)
        {
            _wallet.AddMoney(_reward * (coefficient + 1));
            _wallet.AddGem(_rewardGem * (coefficient + 1));
            _tawnhall.WarriorsComeBack(_swordmanTotal, _archeryTotal, _heavySwordman, _crossbowman, _commander, _knight);
        }

        FrontNewsWrapper.SetBool("show", false);
        StartCoroutine(FrontNewClose());
    }

    public void StartBattle(int id, int totalEnemyWarriors, int reward, int rewardGem, float timer)
    {
        _id = id;
        _totalEnemyWarriors = totalEnemyWarriors;
        _reward = reward;
        _rewardGem = rewardGem;
        _timer = timer;

        if (_totalEnemyWarriors > _totalPlayerWarriors)
        {
            battleStatus = 0; //Defeat  
        }
        else
        {
            battleStatus = 2; //Win
        }

        AtWar = 1;
        PlayerPrefs.SetInt("battleStatus", battleStatus);
        PlayerPrefs.SetInt("AtWar", AtWar);
        Save();

        print(_id + "ID FUNC");
    }

    public void GetWarriorDatas(int swordmanTotal, int archeryTotal, int heavySwordman, int crossbowman, int commander,
        int knight)
    {
        _swordmanTotal = swordmanTotal;
        _archeryTotal = archeryTotal;
        _heavySwordman = heavySwordman;
        _crossbowman = crossbowman;
        _commander = commander;
        _knight = knight;

        _totalPlayerWarriors = _swordmanTotal + _archeryTotal * 2 + _heavySwordman * 3 + _crossbowman * 4 + _commander * 5 +
                               _knight * 6;
    }

    public void Save()
    {
        _sv.id = _id;
        _sv.swordmanTotal = (int) (_swordmanTotal * Random.Range(0, 0.6f + TempleBuild.chance));
        _sv.archeryTotal = (int) (_archeryTotal * Random.Range(0, 0.6f + TempleBuild.chance));
        _sv.heavySwordman = (int) (_heavySwordman * Random.Range(0, 0.6f + TempleBuild.chance));
        _sv.crossbowman = (int) (_crossbowman * Random.Range(0, 0.6f + TempleBuild.chance));
        _sv.commander = (int) (_commander * Random.Range(0.1f, 0.6f + TempleBuild.chance));
        _sv.knight = (int) (_knight * Random.Range(0.1f, 0.8f + TempleBuild.chance));
        _sv.reward = _reward;
        _sv.rewardGem = _rewardGem;
        _sv.timer = KingdomsWrapper.TimerBattle;
        PlayerPrefs.SetString("Battle", JsonUtility.ToJson(_sv));
    }

    IEnumerator FrontNewClose()
    {
        yield return new WaitForSeconds(1.5f);
        TypeOfNews[0].SetActive(false);
        TypeOfNews[1].SetActive(false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Save();
    }
}
