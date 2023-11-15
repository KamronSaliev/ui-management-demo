using UnityEngine;

namespace UIManagementDemo.Core.Config
{
    [CreateAssetMenu(fileName = "ShowHideConfig", menuName = "UIManagementDemo/ShowHideConfig", order = 0)]
    public class ShowHideConfig : ScriptableObject
    {
        public Vector3 OnShowParameter => _onShowParameter;

        public Vector3 OnHideParameter => _onHideParameter;

        public float TweenDuration => _tweenDuration;

        [SerializeField] private Vector3 _onShowParameter;
        [SerializeField] private Vector3 _onHideParameter;
        [SerializeField] private float _tweenDuration;
    }
}