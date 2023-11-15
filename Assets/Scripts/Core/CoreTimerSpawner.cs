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

        private TimerViewModel.Factory _timerViewModelFactory;
        private CallButtonViewModel.Factory _callButtonViewModelFactory;
        private CallButtonView.Factory _callButtonViewFactory;
        private ISpawnButtonView _spawnButtonView;
        private IShowHideButtonsContainer _showHideButtonsContainer;
        private ISaveSystem _saveSystem;

        private SaveData _currentSaveData;

        private const int DefaultTimerCallButtonCount = 3;
        private const string NameMask = "Таймер {0}";

        [Inject]
        public void Construct
        (
            TimerViewModel.Factory timerViewModelFactory,
            CallButtonViewModel.Factory callButtonViewModelFactory,
            CallButtonView.Factory callButtonViewFactory,
            ISpawnButtonView spawnButtonView,
            IShowHideButtonsContainer showHideButtonsContainer,
            ISaveSystem saveSystem
        )
        {
            _timerViewModelFactory = timerViewModelFactory;
            _callButtonViewModelFactory = callButtonViewModelFactory;
            _callButtonViewFactory = callButtonViewFactory;
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
            var callButtonView = _callButtonViewFactory.Create();
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

            wrapper = new SpawnableTimerDataWrapper(timerViewModel, callButtonViewModel);
            Timers.Add(id, wrapper);

            return wrapper;
        }

        private CallButtonViewModel CreateCallButtonViewModel(TimerViewModel timerViewModel)
        {
            var callButtonOptions = new CallButtonOptions(string.Format(NameMask, timerViewModel.Id), timerViewModel);
            var callButtonViewModel = _callButtonViewModelFactory.Create(callButtonOptions);
            callButtonViewModel.Initialize();
            return callButtonViewModel;
        }

        private TimerViewModel CreateTimerViewModel(TimerData timerData)
        {
            var timerViewModel = _timerViewModelFactory.Create(timerData);
            timerViewModel.Initialize();
            return timerViewModel;
        }
    }
}