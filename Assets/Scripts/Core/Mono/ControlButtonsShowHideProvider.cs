using UIManagementDemo.Core.Mono.Interfaces;
using UnityEngine;

namespace UIManagementDemo.Core.Mono
{
    public class ControlButtonsShowHideProvider : MonoBehaviour, IControlButtonsShowHideProvider
    {
        public ShowHideItem ShowHideItem => _showHideItem;

        [SerializeField] private ShowHideItem _showHideItem;
    }
}