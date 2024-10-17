using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
  public TextMeshProUGUI MoneyDisplay;
  [SerializeField] TextMeshProUGUI gemDisplayText;
  public TextMeshProUGUI EarnMoneyDisplay;
  public Wallet _wallet;
  public Animator BuildingsWrapper;
  public Animator MarketWrapper;
  public Animator WarriorsWrapper;
  public Animator AttackListWrapper;
  public Animator EarnOfflineWrapper;
  public Animator GoldShopWindowWrappper;
  public Transform WarriorsWrapperInner;
  public Transform BuildingWrapperInner;
  public Transform AttackListWrapperInner;
  public GameObject TutorailWrapper;
  public static int ManagerInBuilds;

  private Tutorial _tutorial;
  private AudioSource _audioSource;
  private KingdomsWrapper _kingdomWrapper;
  private bool _isbuildingsWrapper;
  private bool _isMarketWrapper;
  private bool _isWarriorsWrapper;
  private bool _isAttackListWrapper;
  private bool _isEarnMoney;
  private bool _isGoldShopWindow;

  private void Awake()
  {
    if (!PlayerPrefs.HasKey("tutorial"))
      TutorailWrapper.SetActive(true);
   
    _wallet.GemChange += UpdateText;
    _wallet.EarnMoneyChange += UpdateEarnMoney;
    _wallet.MoneyChange += TextUpdate;
  }

  private void OnDisable()
  {
    _wallet.GemChange -= UpdateText;
    _wallet.MoneyChange -= TextUpdate;
    _wallet.EarnMoneyChange -= UpdateEarnMoney;
  }

  private void Start()
  {
    _audioSource = GetComponent<AudioSource>();

    _kingdomWrapper = FindObjectOfType<KingdomsWrapper>();
    _tutorial = FindObjectOfType<Tutorial>();

    if (ManagerInBuilds > 0)
      OpenEarnWrapper();
  }
  
  public void OpenBuildingsWrapper()
  {
    if (!_isbuildingsWrapper) BuildingWrapperInner.localPosition = new Vector2(0, 90);

    _isbuildingsWrapper = !_isbuildingsWrapper;
    BuildingsWrapper.SetBool("show", _isbuildingsWrapper);
    AdditionalDetails(_isbuildingsWrapper);

    if (!_tutorial) return;
      //tutorial - open build wrapper
      if(_tutorial.GetTutorialStep ==1)
        _tutorial.NextTutorialStep(2);
    
      //tutorial - close button
      if(_tutorial.GetTutorialStep ==3)
        _tutorial.NextTutorialStep(4);
  }

  public void OpenMarketWrapper()
  {
    _isMarketWrapper = !_isMarketWrapper;
    MarketWrapper.SetBool("show", _isMarketWrapper);
    AdditionalDetails(_isMarketWrapper);
  }

  public void OpenGoldShopWindow()
  {
    _isGoldShopWindow = !_isGoldShopWindow;
    GoldShopWindowWrappper.SetBool("show", _isGoldShopWindow);
    AdditionalDetails(_isGoldShopWindow);
  }

  public void OpenWarriorsWrapper()
  {
    if (!_isWarriorsWrapper) WarriorsWrapperInner.localPosition = new Vector2(0, 0);
    _isWarriorsWrapper = !_isWarriorsWrapper;
    WarriorsWrapper.SetBool("show", _isWarriorsWrapper);
    AdditionalDetails(_isWarriorsWrapper);


    if (_tutorial)
    {
      if(_tutorial.GetTutorialStep==7)
        _tutorial.NextTutorialStep(8);
      
      if(_tutorial.GetTutorialStep==9)
        _tutorial.NextTutorialStep(10);
    }
  }

  public void OpenAttackListWrapper()
  {
    _isAttackListWrapper = !_isAttackListWrapper;
    AttackListWrapper.SetBool("show", _isAttackListWrapper);
    AttackListWrapperInner.localPosition = new Vector2(0, 0);
    AdditionalDetails(_isAttackListWrapper);

    if (!_isAttackListWrapper)
      _kingdomWrapper.CloseWindow();
    
    if(_tutorial)
      if(_tutorial.GetTutorialStep==6)
        _tutorial.NextTutorialStep(7);
  }

  public void OpenEarnWrapper()
  {
    _isEarnMoney = !_isEarnMoney;
    EarnOfflineWrapper.SetBool("show", _isEarnMoney);
    AdditionalDetails(_isEarnMoney);
  }

  private void TextUpdate(float value)
  {
    MoneyDisplay.text = FormatMoneys.FormatMoney(value);
  }

  private void AdditionalDetails(bool isactive)
  {
    CameraScript.isDialogOpen = isactive;
    if (isactive)
      _audioSource.Play();
  }

  private void UpdateText(int value)
  {
    gemDisplayText.SetText(value.ToString());
  }

  private void UpdateEarnMoney(float value)
  {
    EarnMoneyDisplay.SetText(FormatMoneys.FormatMoney(value));
  }
}