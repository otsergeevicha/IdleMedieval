using TMPro;
using UnityEngine.UI;
using UnityEngine;
using webgl;

public class KingdomsItem : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private int _totalEnemys;
    [SerializeField] private int _reward;
    [SerializeField] private int rewardGem;
    [SerializeField] private int _level;
    [SerializeField] private string TownName;
    [SerializeField] private string TownNameRu;
    [SerializeField] private float timer;
    private int _status;
    [SerializeField] private KingdomsWrapper _kingdomsWrapper;

    public GameObject ActivePoint;
    public Image PointImage;
    public TextMeshProUGUI TownNameDisplayText;
    public TextMeshProUGUI LevelDisplayText;
    public Sprite[] StatusSprites;

    public int GetId()
    {
        return _id;
    }

    private void Start()
    {
        CheckStatus();

        _kingdomsWrapper = FindObjectOfType<KingdomsWrapper>();

        if (PlayerPrefs.HasKey("EnemyStatus" + _id))
            _status = PlayerPrefs.GetInt("EnemyStatus" + _id);
            

        TownNameDisplayText.text = TextTranslator.GetText(TownNameRu, TownName);
        PointImage.sprite = StatusSprites[_status];
        LevelDisplayText.text = _id.ToString();
        UseTimer();
    }

    public void ChangeStatus(int status)
    {
        ActivePoint.SetActive(false);
        _status = status;
        PlayerPrefs.SetInt("EnemyStatus" + _id, _status);
        PointImage.sprite = StatusSprites[_status];

        UseTimer();
    }

    public void OpenWindow()
    {
        if (_status > 0 || _id > Tawnhall.BattleLevel + 1) return;
        _kingdomsWrapper._singleItem = this;
        _kingdomsWrapper.OpenAttackWindow(_id, _totalEnemys, _reward, rewardGem, timer);
    
        if (_kingdomsWrapper._Tutorial)
            if(_kingdomsWrapper._Tutorial.GetTutorialStep==10)
                _kingdomsWrapper._Tutorial.NextTutorialStep(11);
    }

    public void CheckStatus()
    {
        ActivePoint.SetActive(Tawnhall.BattleLevel + 1 == _id);
    }

    private void UseTimer()
    {
        if (_status ==1)
            _kingdomsWrapper.SetTimerPosition(GetComponent<RectTransform>());
    }
}
