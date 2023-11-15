using System;
using UnityEngine;

namespace UIManagementDemo.SaveSystem
{
    [Serializable]
    public class TimerData
    {
        public int Id => _id;

        public int Time => _time;

        public bool State => _state;

        [SerializeField] private int _id;
        [SerializeField] private int _time;
        [SerializeField] private bool _state;

        public TimerData(int id)
        {
            _id = id;
        }

        public TimerData(int id, int time, bool state)
        {
            _id = id;
            _time = time;
            _state = state;
        }
    }
}