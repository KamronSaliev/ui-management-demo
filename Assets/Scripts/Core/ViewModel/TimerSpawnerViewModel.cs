using System;
using Utilities;
using Utilities.ExtensionMethods.RX;
using Zenject;

namespace UIManagementDemo.Core.ViewModel
{
    [Obsolete]
    public class TimerSpawnerViewModel : DisposableObject, IInitializable
    {
        public void Initialize()
        {
            Logger.DebugLog(this, "Initialize");
        }
    }
}