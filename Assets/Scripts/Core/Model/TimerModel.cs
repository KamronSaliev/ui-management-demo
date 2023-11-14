using UnityEngine;

namespace UIManagementDemo.Core.Model
{
    public class TimerModel
    {
        public int Id { get; private set; }
        public int Time { get; private set; }
        public bool State { get; private set; }

        public TimerModel(int id, int time, bool state)
        {
            Id = id;
            Time = time;
            State = state;
        }

        public void UpdateTime(int delta)
        {
            var newValue = Mathf.Max(0.0f, Time + delta);
            Time = (int)newValue;
        }
        
        public void UpdateState(bool state)
        {
            State = state;
        }
    }
}