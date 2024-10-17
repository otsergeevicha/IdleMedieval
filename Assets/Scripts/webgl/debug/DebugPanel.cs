using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace webgl.debug
{
    public class DebugPanel : MonoBehaviour
    {
        private const string DebugButtonPath = "Ui/Debug/DebugButton";
        
        [SerializeField] private Button _visibleButton;
        private DebugButton _buttonPrefab;
        private Wallet _wallet;
        

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _wallet = FindObjectOfType<Wallet>();
            _visibleButton.onClick.AddListener(ChangeVisible);
            _buttonPrefab = Resources.Load<DebugButton>(DebugButtonPath);
            AddButtons();
        }

        public void AddButton(string name, string buttonText, UnityAction action)
        {
            var button = Instantiate(_buttonPrefab, transform);
            button.name = name;
            button.Construct(name, buttonText, action);
        }

        private void ChangeVisible()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void AddButtons()
        {
            //AddButton("Device", "SWITCH", Singleton<UiControllers>.Instance.SwitchDevice);
            AddButton("Money", "+5k", () => _wallet.AddMoney(5000));
            AddButton("Gem", "+500", () => _wallet.AddGem(500));
            AddButton("Time", "x30", () => Time.timeScale = 30);
            AddButton("Time", "x1", () => Time.timeScale = 1);
        }
    }
}
