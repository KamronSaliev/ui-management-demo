using UnityEngine;

namespace UIManagementDemo.Core.Config
{
    [CreateAssetMenu(fileName = "ColorButtonConfig", menuName = "UIManagementDemo/ColorButtonConfig", order = 0)]
    public class ColorButtonConfig : ScriptableObject
    {
        public Color ColorOnInactive => _colorOnInactive;

        public Color ColorOnActive => _colorOnActive;

        public float TweenDuration => _tweenDuration;

        [SerializeField] private Color _colorOnInactive;
        [SerializeField] private Color _colorOnActive;
        [SerializeField] private float _tweenDuration;
    }
}