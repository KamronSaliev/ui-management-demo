using System.Collections.Generic;
using UIManagementDemo.Core.Model;

namespace UIManagementDemo.Core
{
    public interface ICoreTimerSpawner
    {
        Dictionary<int, TimerWrapper> Timers { get; }

        void Spawn();

        void Destroy();
    }
}