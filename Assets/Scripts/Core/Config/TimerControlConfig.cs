using UnityEngine;

namespace UIManagementDemo.Core.Config
{
    [CreateAssetMenu(fileName = "TimerControlConfig", menuName = "UIManagementDemo/TimerControlConfig", order = 0)]
    public class TimerControlConfig : ScriptableObject
    {
        public int DefaultStep => _defaultStep;

        public int MaxStep => _maxStep;

        public int MillisecondsToIncreaseStep => _millisecondsToIncreaseStep;

        [SerializeField] private int _defaultStep = 1;
        [SerializeField] private int _maxStep = 10;
        [SerializeField] private int _millisecondsToIncreaseStep = 100;
    }
}