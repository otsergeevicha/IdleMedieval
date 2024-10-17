using UnityEngine;

namespace webgl
{
    public class Quality : MonoBehaviour
    {
        private bool _isMobile;
        private void Awake()
        {
            if (_isMobile)
            {
                QualitySettings.SetQualityLevel(0, true);
            }
        }
    }
}
