using UIManagementDemo.SaveSystem;
using UnityEngine;
using Zenject;

namespace UIManagementDemo.Core.Mono
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
                var timerViewModel = timer.TimerViewModel;
                newSaveData.TimerData.Add(new TimerData
                (
                    timerViewModel.Id, 
                    timerViewModel.Time.Value,
                    timerViewModel.State.Value));
            }

            _saveSystem.Save(newSaveData);
        }
    }
}