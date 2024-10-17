
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialItem[] tutorialItems;
    [SerializeField] private RectTransform tutorialHand;
    private int _tutorialStep;
    private RandomBonus _randomBonus;
    private Wallet _wallet;
    public int GetTutorialStep => _tutorialStep;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("tutorial"))
            Destroy(gameObject);
    }

    private void Start()
    {
        _wallet = FindObjectOfType<Wallet>();
        _randomBonus = FindObjectOfType<RandomBonus>();
        
        if (!PlayerPrefs.HasKey("tutorial"))
        {
            _tutorialStep = PlayerPrefs.GetInt("_tutorialStep");
        }

        NextTutorialStep(_tutorialStep);
    }

    public void NextTutorialStep(int step)
    {
        if (_tutorialStep > step) return;

        _tutorialStep = step;
        tutorialHand.gameObject.SetActive(true);

        for (int i = 0; i < tutorialItems[_tutorialStep].DisableGameobjects.Length; i++)
            tutorialItems[_tutorialStep].DisableGameobjects[i].SetActive(false);

        for (int i = 0; i < tutorialItems[_tutorialStep].EnableGameobjects.Length; i++)
            tutorialItems[_tutorialStep].EnableGameobjects[i].SetActive(true);

        tutorialHand.SetParent(tutorialItems[_tutorialStep].TutorialTarget);
        tutorialHand.localPosition = Vector3.zero;

        switch (_tutorialStep)
        {
            case 1:
                // GameAnalytics.NewDesignEvent("Tutorial_START", 1);
                break;
            case 8:
                _wallet.AddMoney(700);
                break;
        }
        
        PlayerPrefs.SetInt("_tutorialStep", _tutorialStep);
    }


    public void TutorialCoin(int coin)
    {
        _wallet.AddMoney(coin);
    }

    public void FinishBtn(int coin)
    {
        _wallet.AddMoney(coin);
        _randomBonus.UIBtn.SetActive(true);
        PlayerPrefs.SetInt("tutorial", 1);
        //GameAnalytics.NewDesignEvent("Tutorial_END", 1);
        NextTutorialStep(13);
    }
}
