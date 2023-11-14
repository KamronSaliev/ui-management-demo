using UnityEngine;

namespace UIManagementDemo.Core.Config
{
    [CreateAssetMenu(fileName = "ShowHideButtonsContainerConfig", menuName = "UIManagementDemo/ShowHideButtonsContainerConfig", order = 0)]
    public class ShowHideButtonsContainerConfig : ScriptableObject
    {
        public float Delay => _delay;

        [SerializeField] private float _delay = 0.01f;
    }
}