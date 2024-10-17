using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class Build : MonoBehaviour
{
    [SerializeField] private int _id; //Тип здания
    [SerializeField] private int _income; //Доход
    [SerializeField] private int _capacity; //Вместимость 
    [SerializeField] private float _timeCycle; //время 1 цикла    
    [SerializeField] private GameObject[] BuildLevelsPrefabs; //Модели Уровней здания
    [SerializeField] private int _needClickFarm;
    private int _fullness;
    private Transform _market;
    private Wallet _wallet;
    private int _currentClickFarm;
    private AudioSource _marketAudiosSource;

    public Save _sv = new Save(); //сохранение
    [HideInInspector] public int manager; //Менеджер в здании
    public GameObject DustBuilding;
    [HideInInspector] public int BuildLevels; //уровень здания
    public Transform PointNpcPorterInstance; // Точка создания NPC грузчика
    public GameObject NpcPorterPrefab; // Префаб грузчика
    private NPC_Porter _npcPorter;

    [Header("UI Build Elements")] public GameObject UIWrapper;
    public Image TimerCycleProgressDisplay;
    public Image ClickCycleProcess;
    public TextMeshProUGUI FullnessProductText;
    private Animator _uiWrapperAnimator;
    private Tutorial _tutorial;

    private GameObject Clone;
    private float _timer;

    public int GetId()
    {
        return _id;
    }

    public int GetIncome()
    {
        return _income;
    }

    public float GetTimeCycle()
    {
        return _timeCycle;
    }

    public int GetCapacity()
    {
        return _capacity;
    }

    private void Awake()
    {
        // if (PlayerPrefs.HasKey("Building" + _id))
        // {
        //     _sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Building" + _id));
        if (PlayerPrefs.HasKey("Building" + _id))
        {
            //_sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Building" + _id));
            _sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Building" + _id));
            _income = _sv.income;
            _capacity = _sv.capacity;
            _timeCycle = _sv.timeCycle;
            _fullness = _sv.fullness;
            BuildLevels = _sv.buildLevels;
            manager = _sv.manager;
            _timer = Random.Range(1, _timeCycle);
        }

        MainUI.ManagerInBuilds += manager;
    }

    private void Start()
    {
        _wallet = FindObjectOfType<Wallet>();
        _tutorial = FindObjectOfType<Tutorial>();

        _uiWrapperAnimator = UIWrapper.GetComponent<Animator>();

        if (BuildLevels > 0)
        {
            if (manager > 0)
            {
                _wallet.AddMoney((int) ((TimeLastSession.ts.TotalSeconds / _timeCycle) * _income) / 4);
                _wallet.AddEarnMoney((int) ((TimeLastSession.ts.TotalSeconds / _timeCycle) * _income) / 4);

                _fullness = 0;
            }
            else
            {
                if (TimeLastSession.ts.Days > 0 || TimeLastSession.ts.Hours > 0 ||
                    _fullness + ((TimeLastSession.ts.TotalSeconds / _timeCycle) * _income) >= _capacity)
                {
                    _fullness = _capacity;
                }
                else
                {
                    _fullness = _fullness + (int) ((TimeLastSession.ts.TotalSeconds / (_timeCycle*2)) * _income);
                }
            }
        }

        ChangeFullnessText();
        _market = FindObjectOfType<Market>().transform;
        _marketAudiosSource = _market.GetComponent<AudioSource>();
        CheckerBuilding();
        UIChecker();
    }

    private void Update()
    {
        if (BuildLevels <= 0) return;
        if (_timer >= _timeCycle)
        {
            DoIncomeInit();
            _timer = 0;
        }
        else
        {
            _timer += Time.deltaTime;
        }

        TimerCycleProgressDisplay.fillAmount = _timer / _timeCycle;
    }

    //Кликом зарабатвать ресурс
    public void ClickerFarm()
    {
        if (_fullness < _capacity)
        {
            _currentClickFarm++;

            if (_currentClickFarm >= _needClickFarm)
            {
                _fullness++;
                _currentClickFarm = 0;
                _uiWrapperAnimator.SetTrigger("in");
                _marketAudiosSource.Play();
            }

            ClickCycleProcess.fillAmount = (float) _currentClickFarm / _needClickFarm;
            ChangeFullnessText();

            if (!_tutorial) return;
            if (_tutorial)
                _tutorial.NextTutorialStep(5);
        }
    }

    // Апгрейд здания
    public void UpgradeBuild()
    {
        //_income += 3;
        _capacity += 3;
        BuildLevels++;
        _timeCycle =  GameBalance.BuildTimeCycle * (float)Math.Pow(GameBalance.BuildTimeCycleUpgradeCoef, BuildLevels);
        SaveBuildingsData();
    }

    //Доход здания количество/время
    public void DoIncomeInit()
    {
        if (_fullness <= _capacity)
            _fullness += _income;

        ChangeFullnessText();

        if (manager > 0) InstantiateNpcPorter();
    }

    //Постройка здания
    public void BuildNewBuilding()
    {
        SaveBuildingsData();
        Clone = Instantiate(DustBuilding, transform.position, Quaternion.identity);
        StartCoroutine(CheckerBuildBuilding());
        Destroy(Clone, 7);
    }

    //создание грузчика и передача ему данных
    public void InstantiateNpcPorter()
    {
        if (_fullness <= 0) return;
        _npcPorter = Instantiate(NpcPorterPrefab, PointNpcPorterInstance.position, Quaternion.identity)
            .GetComponent<NPC_Porter>();
        LoadPorter();
    
        if(_tutorial)
            if(_tutorial.GetTutorialStep==5)
                _tutorial.NextTutorialStep(6);
    }

    private void LoadPorter()
    {
        _npcPorter.NPC_Data(_id, _fullness, PointNpcPorterInstance.position, _market.position);
        _fullness = 0;
        ChangeFullnessText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPC_Porter>(out NPC_Porter npc_porter))
        {
            if (npc_porter.GetPorterId() == _id && npc_porter.done)
                Destroy(other.gameObject);
        }
    }

    //Постройка и проверка уровня моделиздания с задержкой
    IEnumerator CheckerBuildBuilding()
    {
        yield return new WaitForSeconds(7);
        CheckerBuilding();
        UIChecker();
    }

    private void CheckerBuilding()
    {
        foreach (GameObject buildings in BuildLevelsPrefabs)
        {
            buildings.SetActive(false);
        }

        if (BuildLevels >= 1)
        {
            BuildLevelsPrefabs[1].SetActive(true);
        }
        else
        {
            BuildLevelsPrefabs[0].SetActive(true);
        }
    }

    private void ChangeFullnessText()
    {
        FullnessProductText.text = _fullness.ToString();
        SaveBuildingsData();
    }

    private void UIChecker()
    {
        if (BuildLevels > 0)
            UIWrapper.SetActive(true);
    }


    private void SaveBuildingsData()
    {
        _sv.income = _income;
        _sv.capacity = _capacity;
        _sv.timeCycle = _timeCycle;
        _sv.fullness = _fullness;
        _sv.buildLevels = BuildLevels;
        _sv.manager = manager;
        PlayerPrefs.SetString("Building" + _id, JsonUtility.ToJson(_sv));
    }
}
