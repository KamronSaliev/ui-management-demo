using System.Collections.Generic;
using System.Linq;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.View;
using UIManagementDemo.Core.View.Interfaces;
using UIManagementDemo.Core.ViewModel;
using UIManagementDemo.Core.ViewModel.Interfaces;
using UIManagementDemo.SaveSystem;
using UnityEngine;
using Zenject;

namespace UIManagementDemo.Core
{
    public class CoreTimerSpawner : ICoreTimerSpawner, IInitializable
    {
        public Dictionary<int, SpawnableTimerDataWrapper> Timers { get; } = new();

        private CallButtonView.Factory _timerCallButtonViewFactory;
        private ISpawnButtonView _spawnButtonView;
        private ITimerView _timerView;
        private IShowHideButtonsContainer _showHideButtonsContainer;
        private ISaveSystem _saveSystem;

        private SaveData _currentSaveData;

        private const int DefaultTimerCallButtonCount = 3;
        private const string NameMask = "Таймер {0}";

        [Inject]
        public void Construct
        (
            CallButtonView.Factory timerCallButtonViewFactory,
            ITimerView timerView,
            ISpawnButtonView spawnButtonView,
            IShowHideButtonsContainer showHideButtonsContainer,
            ISaveSystem saveSystem
        )
        {
            _timerCallButtonViewFactory = timerCallButtonViewFactory;
            _timerView = timerView;
            _spawnButtonView = spawnButtonView;
            _showHideButtonsContainer = showHideButtonsContainer;
            _saveSystem = saveSystem;
        }

        public void Initialize()
        {
            _currentSaveData = _saveSystem.Load();
            _showHideButtonsContainer.Add(_spawnButtonView.ShowHideButton);
            var initialCount = Mathf.Max(DefaultTimerCallButtonCount, _currentSaveData.TimerData.Count);

            for (var i = 0; i < initialCount; i++)
            {
                Spawn(i + 1);
            }

            _showHideButtonsContainer.Show();
        }

        public void Spawn()
        {
            var currentId = Timers.Count;
            Spawn(currentId + 1);
        }
        
        private void Spawn(int id)
        {
            var callButtonView = _timerCallButtonViewFactory.Create();
            var wrapper = GetOrCreateTimerDataWrapper(id);
            callButtonView.BindTo(wrapper.CallButtonViewModel);
            _showHideButtonsContainer.Add(callButtonView.ShowHideButton, true);
        }

        private SpawnableTimerDataWrapper GetOrCreateTimerDataWrapper(int id)
        {
            if (Timers.TryGetValue(id, out var wrapper))
            {
                return wrapper;
            }

            var timerData = _currentSaveData.TimerData.FirstOrDefault(t => t.Id == id) ?? new TimerData(id);
            var timerViewModel = CreateTimerViewModel(timerData);
            var callButtonViewModel = CreateCallButtonViewModel(timerViewModel);

            wrapper = new SpawnableTimerDataWrapper
            {
                TimerViewModel = timerViewModel,
                CallButtonViewModel = callButtonViewModel
            };

            Timers.Add(id, wrapper);

            return wrapper;
        }

        private CallButtonViewModel CreateCallButtonViewModel(TimerViewModel timerViewModel)
        {
            var callButtonViewModel = new CallButtonViewModel
            (
                string.Format(NameMask, timerViewModel.Id),
                timerViewModel,
                _timerView,
                _showHideButtonsContainer
            );
            callButtonViewModel.Initialize();
            return callButtonViewModel;
        }

        private TimerViewModel CreateTimerViewModel(TimerData timerData)
        {
            var timerViewModel = new TimerViewModel
            (
                timerData,
                _showHideButtonsContainer
            );
            timerViewModel.Initialize();
            return timerViewModel;
        }
    }
}