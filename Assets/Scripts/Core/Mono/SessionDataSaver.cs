using UIManagementDemo.Core;
using UIManagementDemo.SaveSystem;
using UnityEngine;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Mono
{
    public class SessionDataSaver : MonoBehaviour
    {
        private ICoreTimerSpawner _coreTimerSpawner;
        private ISaveSystem _saveSystem;

        [Inject]
        public void Construct
        (
            ICoreTimerSpawner coreTimerSpawner,
            ISaveSystem saveSystem
        )
        {
            _coreTimerSpawner = coreTimerSpawner;
            _saveSystem = saveSystem;
        }

        private void OnApplicationQuit()
        {
            var newSaveData = new SaveData();

            foreach (var timer in _coreTimerSpawner.Timers.Values)
            {
                newSaveData.TimerData.Add(
                    new TimerData
                    (
                        timer.TimerViewModel.Id,
                        timer.TimerViewModel.Time.Value,
                        timer.TimerViewModel.State.Value
                    ));
            }

            _saveSystem.Save(newSaveData);

            Logger.DebugLogWarning(this, "Session Data Saved");
        }
    }
}