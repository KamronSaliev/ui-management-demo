using System.Collections.Generic;
using UIManagementDemo.Core.ViewModel;

namespace UIManagementDemo.Core
{
    public interface ICoreTimerSpawner
    {
        Dictionary<int, TimerViewModel> Timers { get; }
        
        void Spawn();
    }
}