using UnityEngine;
using System.Collections;
using TMPro;

public class Tawnhall : MonoBehaviour
{
    public static int BattleLevel;
    [SerializeField] private int[] _warriorPrices;
    private int _swordmanTotal;
    private int _archeryTotal;
    private int _heavySwordman;
    private int _crossbowman;
    private int _commander;
    private int _knight;
    private BuildCitizenHouse _buildCitizenHouse;
    private Wallet _wallet;
    private WarriorsSave _sv = new WarriorsSave();
    private Warrior _warrior;
    private AudioSource _audioSource;
    private Battleground _battleground;

    //public TextMeshProUGUI CitizenDisplayText;
    public TextMeshProUGUI[] WarriorTotalDisplayText;
    public TextMeshProUGUI[] WarriorPricesDisplayText;

    public Transform[] StartEndPost;
    public Warrior[] WarriorsGO;

    private Warrior Clone;
    public int TotalWarriors;
    private Tutorial _tutorial;

    private void Awake()
    {
        //BattleLevel = PlayerPrefs.GetInt("BattleLevel");
        BattleLevel = PlayerPrefs.GetInt("BattleLevel");
    }

    private void Start()
    {
        _buildCitizenHouse = FindObjectOfType<BuildCitizenHouse>();
        _audioSource = GetComponent<AudioSource>();
        _wallet = FindObjectOfType<Wallet>();
        _battleground = FindObjectOfType<Battleground>();
        _tutorial = FindObjectOfType<Tutorial>();

        // if (PlayerPrefs.HasKey("Warriors"))
        // {
        //     _sv = JsonUtility.FromJson<WarriorsSave>(PlayerPrefs.GetString("Warriors"));
        if (PlayerPrefs.HasKey("Warriors"))
        {
            _sv = JsonUtility.FromJson<WarriorsSave>(PlayerPrefs.GetString("Warriors"));

            _swordmanTotal = _sv.SwordmanTotal;
            _archeryTotal = _sv.ArcheryTotal;
            _heavySwordman = _sv.HeavySwordman;
            _crossbowman = _sv.Crossbowman;
            _commander = _sv.Commander;
            _knight = _sv.Knight;
        }

        CheckWarriorText();
        CheckTotalWarriorsInTawn();
    }

    public void InstanceWarriors()
    {
        _audioSource.Play();
        _battleground.GetWarriorDatas(_swordmanTotal, _archeryTotal, _heavySwordman, _crossbowman, _commander, _knight);
        StartCoroutine(WarriorsInstance(_swordmanTotal, _archeryTotal, _heavySwordman, _crossbowman, _commander, _knight));
        _swordmanTotal = 0;
        _archeryTotal = 0;
        _heavySwordman = 0;
        _crossbowman = 0;
        _commander = 0;
        _knight = 0;
        CheckWarriorText();
        CheckTotalWarriorsInTawn();
        SaveWarriorsData();
    }

    private void CheckWarriorText()
    {
        WarriorTotalDisplayText[0].text = _swordmanTotal.ToString();
        WarriorTotalDisplayText[1].text = _archeryTotal.ToString();
        WarriorTotalDisplayText[2].text = _heavySwordman.ToString();
        WarriorTotalDisplayText[3].text = _crossbowman.ToString();
        WarriorTotalDisplayText[4].text = _commander.ToString();
        WarriorTotalDisplayText[5].text = _knight.ToString();

        for (int i = 0; i < WarriorPricesDisplayText.Length; i++)
        {
            WarriorPricesDisplayText[i].text = FormatMoneys.FormatMoney(_warriorPrices[i]);
        }
    }

    public void WarriorsComeBack(int swordmanTotal, int archeryTotal, int heavySwordman, int crossbowman, int commander, int knight)
    {
        _swordmanTotal = swordmanTotal;
        _archeryTotal = archeryTotal;
        _heavySwordman = heavySwordman;
        _crossbowman = crossbowman;
        _commander = commander;
        _knight = knight;

        CheckWarriorText();
        CheckTotalWarriorsInTawn();
        SaveWarriorsData();
    }

    private void CheckTotalWarriorsInTawn()
    {
        TotalWarriors = _swordmanTotal + _archeryTotal + _heavySwordman + _crossbowman + _commander + _knight;
    }

    private void SaveWarriorsData()
    {
        _sv.SwordmanTotal = _swordmanTotal;
        _sv.ArcheryTotal = _archeryTotal;
        _sv.HeavySwordman = _heavySwordman;
        _sv.Crossbowman = _crossbowman;
        _sv.Commander = _commander;
        _sv.Knight = _knight;
        
        PlayerPrefs.SetString("Warriors", JsonUtility.ToJson(_sv));
    }

    public void HireWarriorBtn(int id)
    {
        if (_wallet.GetMoney() >= _warriorPrices[id])
        {
            _wallet.UseMoney(_warriorPrices[id]);

            CheckWarriorsType(id);
            
            if(_tutorial)
                if(_tutorial.GetTutorialStep==8)
                    _tutorial.NextTutorialStep(9);
        }
    }

    public void AdsHireWarrior(int id)
    {
        AdsAddWarriorsType(id);
    }

    public void BonusWarrior(int id)
    {
        CheckWarriorsType(id);
    }

    private void AdsAddWarriorsType(int id)
    {
        switch (id)
        {
            case 0:
                _swordmanTotal += 5;
                break;

            case 1:
                _archeryTotal += 4;
                break;

            case 2:
                _heavySwordman += 3;
                break;

            case 3:
                _crossbowman += 2;
                break;

            case 4:
                _commander += 1;
                break;
        }

        SaveWarriorsData();
        CheckWarriorText();
        CheckTotalWarriorsInTawn();
    }


    private void CheckWarriorsType(int id)
    {
        switch (id)
        {
            case 0:
                _swordmanTotal += 5;
                break;

            case 1:
                _archeryTotal += 5;
                break;

            case 2:
                _heavySwordman += 5;
                break;

            case 3:
                _crossbowman += 5;
                break;

            case 4:
                _commander += 5;
                break;

            case 5:
                _knight += 5;
                break;
        }

        SaveWarriorsData();
        CheckWarriorText();
        CheckTotalWarriorsInTawn();
    }

    IEnumerator WarriorsInstance(int swordman, int archery, int heavyswordman, int crossbowman, int commander, int knight)
    {

        if (swordman > 0)
        {
            yield return new WaitForSeconds(3f);
            Clone = Instantiate(WarriorsGO[0], StartEndPost[0].position, Quaternion.identity);
            Clone.TotalWarriors = swordman;
            Clone.endPos = StartEndPost[1];
        }

        if (archery > 0)
        {
            yield return new WaitForSeconds(1f);
            Clone = Instantiate(WarriorsGO[1], StartEndPost[0].position, Quaternion.identity);
            Clone.TotalWarriors = archery;
            Clone.endPos = StartEndPost[1];
        }

        if (heavyswordman > 0)
        {
            yield return new WaitForSeconds(1.2f);
            Clone = Instantiate(WarriorsGO[2], StartEndPost[0].position, Quaternion.identity);
            Clone.TotalWarriors = heavyswordman;
            Clone.endPos = StartEndPost[1];
        }

        if (crossbowman > 0)
        {
            yield return new WaitForSeconds(1.2f);
            Clone = Instantiate(WarriorsGO[3], StartEndPost[0].position, Quaternion.identity);
            Clone.TotalWarriors = crossbowman;
            Clone.endPos = StartEndPost[1];
        }

        if (commander > 0)
        {
            yield return new WaitForSeconds(1.2f);
            Clone = Instantiate(WarriorsGO[4], StartEndPost[0].position, Quaternion.identity);
            Clone.TotalWarriors = commander;
            Clone.endPos = StartEndPost[1];
        }

        if (knight > 0)
        {
            yield return new WaitForSeconds(1.2f);
            Clone = Instantiate(WarriorsGO[5], StartEndPost[0].position, Quaternion.identity);
            Clone.TotalWarriors = knight;
            Clone.endPos = StartEndPost[1];
        }
    }
}
