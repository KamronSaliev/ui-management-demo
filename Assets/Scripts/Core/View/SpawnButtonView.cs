using UnityEngine;
using UnityEngine.UI;

namespace UIManagementDemo.Core.View
{
    public class SpawnButtonView : MonoBehaviour
    {
        public Button Button => _button;

        public ShowHideButton ShowHideButton => _showHideButton;

        [SerializeField] private Button _button;
        [SerializeField] private ShowHideButton _showHideButton;
    }
}