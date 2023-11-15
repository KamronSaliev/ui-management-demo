using System.Collections.Generic;
using UIManagementDemo.Core.Model;

namespace UIManagementDemo.Core
{
    public interface ICoreTimerSpawner
    {
        Dictionary<int, SpawnableTimerDataWrapper> Timers { get; }
        
        void Spawn();
    }
}