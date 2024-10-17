using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace webgl
{
    public class SoundSettings : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private TMP_Text _text;

        private bool _enabled = true;

        private void Start()
        {
            _enabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
            _button.onClick.AddListener(Change);
            SetSound();
            SetSprite();
            SetText();
        }

        private void Change()
        {
            _enabled = !_enabled;
            SetSound();
            SetSprite();
            SetText();
            PlayerPrefs.SetInt("SoundEnabled", _enabled ? 1 : 0);
        }

        private void SetSound()
        {
            AudioListener.volume = _enabled ? 1 : 0;
        }
        
        private void SetSprite()
        {
            _image.sprite = _enabled ? _onSprite : _offSprite;
        }
        
        private void SetText()
        {
            if (_enabled)
            {
                TextTranslator.SetText("ЗВУК ВКЛ", "SOUND ON", _text);
            }
            else
            {
                TextTranslator.SetText("ЗВУК ВЫКЛ", "SOUND OFF", _text);
            }
        }
    }
}
