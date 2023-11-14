using UnityEngine;

namespace UIManagementDemo.Core.Config
{
    [CreateAssetMenu(fileName = "ShowHideButtonConfig", menuName = "UIManagementDemo/ShowHideButtonConfig", order = 0)]
    public class ShowHideButtonConfig : ScriptableObject
    {
        public Vector3 ShowPosition => _showPosition;

        public Vector3 HidePosition => _hidePosition;

        public float TweenDuration => _tweenDuration;

        [SerializeField] private Vector3 _showPosition;
        [SerializeField] private Vector3 _hidePosition;
        [SerializeField] private float _tweenDuration;
    }
}