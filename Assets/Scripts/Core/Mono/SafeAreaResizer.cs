using UnityEngine;
using Utilities.ExtensionMethods;

namespace UIManagementDemo.Core.Mono
{
    public class SafeAreaResizer : MonoBehaviour
    {
        [SerializeField] private RectTransform _safeArea;

        private void Start()
        {
            _safeArea.ResizeBySafeArea();
        }
    }
}