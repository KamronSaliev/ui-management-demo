using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIManagementDemo.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public List<TimerData> TimerData => _timerData;

        [SerializeField] private List<TimerData> _timerData = new();
    }
}