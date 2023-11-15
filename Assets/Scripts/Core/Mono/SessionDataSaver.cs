using UIManagementDemo.Core;
using UIManagementDemo.SaveSystem;
using UnityEngine;
using Zenject;

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
                newSaveData.TimerData.Add(new TimerData(timer.Id, timer.Time.Value, timer.State.Value));
            }

            _saveSystem.Save(newSaveData);
        }
    }
}