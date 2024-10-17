using System;
using UnityEngine;
using TMPro;
using webgl;

public class KingdomsWrapper : MonoBehaviour
{
  public Animator AttackWindowAnim;
  public TextMeshProUGUI EnemysDisplayText;
  public TextMeshProUGUI RewardDisplayText;
  public TextMeshProUGUI RewardGemDisplayText;
  public Animator NoWarriorsText;
  public KingdomsItem[] Kingdomsitems;
  [SerializeField] private RectTransform timerTransform;
  [SerializeField] private TextMeshProUGUI timerDisplayText, timerActiveWarDisplayText;
  [HideInInspector] public KingdomsItem _singleItem;
  private Battleground _battleground;
  private Tawnhall _tawnhall;

  private int _id;
  private int _totalEnemys;
  private int _reward;
  private int _rewardGem;
  public static float TimerBattle;
  [HideInInspector] public Tutorial _Tutorial;

  private bool _atWar;

  private void Awake()
  {
    //Kingdomsitems = FindObjectsOfType<KingdomsItem>();
    _battleground = FindObjectOfType<Battleground>();
    _tawnhall = FindObjectOfType<Tawnhall>();
    _Tutorial = FindObjectOfType<Tutorial>();
  }

  private void Start()
  {
    _atWar = Battleground.AtWar > 0;
    print("ATWAR "+_atWar);
    timerTransform.gameObject.SetActive(_atWar);
  }

  private void Update()
  {
    if (_atWar)
    {
      if (TimerBattle <= 0)
      {
        _battleground._adsSkipBattle = true;
        _battleground.CheckBattleStatus();
        timerTransform.gameObject.SetActive(false);
      }
      else
      {
        TimerBattle -= Time.deltaTime;
        timerActiveWarDisplayText.text = string.Format("{00:00}:{01:00}:{02:00}", (int) (TimerBattle / 3600),
          (int) (TimerBattle / 60 - (int) (TimerBattle / 3600) * 60),
          (int) ((TimerBattle - (int) (TimerBattle / 60) * 60))); // мин. и секунды
      }
    }
  }

  public void SetTimerActiveWar(float value)
  {
    TimerBattle = value;
  }

  public void SetTimerPosition(RectTransform position)
  {
    timerTransform.gameObject.SetActive(_atWar);
    timerTransform.SetParent(position);
    timerTransform.localPosition = new Vector3(0, -97, 0);
    timerTransform.localScale = new Vector3(1, 1, 1);
  }

  public void OpenAttackWindow(int id, int totalEnemys, int reward, int gemReward, float timer)
  {
    EnemysDisplayText.text = FormatMoneys.TotalEnemyText(totalEnemys);
    RewardDisplayText.text = FormatMoneys.FormatMoney(reward);
    RewardGemDisplayText.SetText(gemReward.ToString());

    timerDisplayText.text = timer < 60 ? timer + TextTranslator.GetText("сек","sec") : (timer / 60) + TextTranslator.GetText("мин","min");

    if (timer >= 3600)
      timerDisplayText.text = (timer / 3600) + TextTranslator.GetText(" ч"," hr");

    _id = id;
    _totalEnemys = totalEnemys;
    _reward = reward;
    _rewardGem = gemReward;
    TimerBattle = timer;
    AttackWindowAnim.SetBool("show", true);
  }

  public void AttackBtn()
  {
    if (_tawnhall.TotalWarriors <= 0)
      NoWarriorsText.SetTrigger("in");

    if (_tawnhall.TotalWarriors <= 0 || Battleground.AtWar > 0) return;
    _singleItem.ChangeStatus(1);
    _tawnhall.InstanceWarriors();
    _battleground.StartBattle(_id, _totalEnemys, _reward, _rewardGem, TimerBattle);
    AttackWindowAnim.SetBool("show", false);
    _atWar = true;
    timerTransform.gameObject.SetActive(_atWar);
    
    AppMetEvents.SendEvent("Start_Battle-" + _id);
    
    if (_Tutorial)
      if(_Tutorial.GetTutorialStep==11)
        _Tutorial.NextTutorialStep(12);
    
    
  }

  public void CloseWindow()
  {
    AttackWindowAnim.SetBool("show", false);
  }
}