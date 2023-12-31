using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.Mono.Interfaces;
using UIManagementDemo.Core.View;
using UIManagementDemo.Core.ViewModel;
using UIManagementDemo.Core.ViewModel.Interfaces;
using UIManagementDemo.SaveSystem;
using Zenject;
using Object = UnityEngine.Object;

namespace UIManagementDemo.Core
{
    public class CoreTimerSpawner : ICoreTimerSpawner, IInitializable
    {
        public Dictionary<int, TimerWrapper> Timers { get; } = new();

        private TimerViewModel.Factory _timerViewModelFactory;
        private CallButtonViewModel.Factory _callButtonViewModelFactory;
        private CallButtonView.Factory _callButtonViewFactory;
        private IControlButtonsShowHideProvider _controlButtonsShowHideProvider;
        private IShowHideButtonsContainer _showHideButtonsContainer;
        private ISaveSystem _saveSystem;

        private SaveData _currentSaveData;

        private const int DefaultTimerCallButtonCount = 3;
        private const string NameMask = "Timer {0}";
        private const float InitialDelay = 0.5f;

        [Inject]
        public void Construct
        (
            TimerViewModel.Factory timerViewModelFactory,
            CallButtonViewModel.Factory callButtonViewModelFactory,
            CallButtonView.Factory callButtonViewFactory,
            IControlButtonsShowHideProvider controlButtonsShowHideProvider,
            IShowHideButtonsContainer showHideButtonsContainer,
            ISaveSystem saveSystem
        )
        {
            _timerViewModelFactory = timerViewModelFactory;
            _callButtonViewModelFactory = callButtonViewModelFactory;
            _callButtonViewFactory = callButtonViewFactory;
            _controlButtonsShowHideProvider = controlButtonsShowHideProvider;
            _showHideButtonsContainer = showHideButtonsContainer;
            _saveSystem = saveSystem;
        }

        public async void Initialize()
        {
            _currentSaveData = _saveSystem.Load();

            _showHideButtonsContainer.Push(_controlButtonsShowHideProvider.ShowHideItem);
            var initialCount = Math.Max(DefaultTimerCallButtonCount, _currentSaveData.TimerData.Count);

            await UniTask.Delay(TimeSpan.FromSeconds(InitialDelay));
            
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
            var timerViewModel = GetOrCreateTimerViewModel(id);
            var callButtonViewModel = CreateCallButtonViewModel(timerViewModel);
            callButtonView.BindTo(callButtonViewModel);
            Timers.Add(id, new TimerWrapper(callButtonView, callButtonViewModel, timerViewModel));
            _showHideButtonsContainer.Push(callButtonView.ShowHideItem, true);
        }

        public void Destroy()
        {
            if (Timers.Count == 0)
            {
                return;
            }
            
            var lastId = Timers.Count;
            _showHideButtonsContainer.Pop();
            var timerWrapper = Timers.GetValueOrDefault(lastId);
            Object.Destroy(timerWrapper.CallButtonView.gameObject);
            timerWrapper.CallButtonViewModel.Dispose();
            timerWrapper.TimerViewModel.Dispose();
            Timers.Remove(lastId);
        }

        private TimerViewModel GetOrCreateTimerViewModel(int id)
        {
            var timerData = _currentSaveData.TimerData.FirstOrDefault(t => t.Id == id) ?? new TimerData(id);
            var timerViewModel = _timerViewModelFactory.Create(timerData);
            timerViewModel.Initialize();
            return timerViewModel;
        }

        private CallButtonViewModel CreateCallButtonViewModel(TimerViewModel timerViewModel)
        {
            var callButtonOptions = new CallButtonOptions(string.Format(NameMask, timerViewModel.Id), timerViewModel);
            var callButtonViewModel = _callButtonViewModelFactory.Create(callButtonOptions);
            callButtonViewModel.Initialize();
            return callButtonViewModel;
        }
    }
}