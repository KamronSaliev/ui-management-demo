using System.Collections.Generic;
using UIManagementDemo.Core.Model;
using UIManagementDemo.Core.ViewModel;
using UIManagementDemo.SaveSystem;
using UniRx;
using UnityEngine;
using Zenject;
using Logger = Utilities.Logger;

namespace UIManagementDemo.Core.View
{
    public class TimerSpawnerView : BindableView<TimerSpawnerViewModel>
    {
        [SerializeField] private SpawnButtonView _spawnButtonView;
        [SerializeField] private string _nameMask = "Таймер {0}";

        // TODO: refactor, maybe merge
        private readonly Dictionary<int, TimerModel> _timerModels = new();
        private readonly Dictionary<int, TimerViewModel> _timerViewModels = new();
        private readonly Dictionary<int, TimerCallButtonViewModel> _timerCallButtonViewModels = new();

        private TimerCallButtonView.Factory _timerCallButtonViewFactory;
        private TimerView _timerView;
        private IShowHideButtonsContainer _showHideButtonsContainer;
        private ISaveSystem _saveSystem;

        private SaveData _currentSaveData;
        private int CurrentId => _timerModels.Count;

        private const int DefaultTimerCallButtonCount = 3;

        [Inject]
        public void Construct
        (
            TimerCallButtonView.Factory timerCallButtonViewFactory,
            TimerView timerView,
            IShowHideButtonsContainer showHideButtonsContainer,
            ISaveSystem saveSystem
        )
        {
            _timerCallButtonViewFactory = timerCallButtonViewFactory;
            _timerView = timerView;
            _showHideButtonsContainer = showHideButtonsContainer;
            _saveSystem = saveSystem;
        }

        protected override void OnBind(CompositeDisposable disposables)
        {
            Logger.DebugLog(this, "OnBind");

            _spawnButtonView.Button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var timerCallButtonView = Spawn(new TimerData(CurrentId + 1));
                    _showHideButtonsContainer.Add(timerCallButtonView.ShowHideButton, true);
                })
                .AddTo(disposables);
        }

        private void Start()
        {
            _currentSaveData = _saveSystem.Load();

            Logger.DebugLogWarning(this, $"CurrentSaveData: {_currentSaveData.TimerData.Count}");

            _showHideButtonsContainer.Add(_spawnButtonView.ShowHideButton);
            if (_currentSaveData.TimerData.Count == 0)
            {
                for (var i = 0; i < DefaultTimerCallButtonCount; i++)
                {
                    var timerCallButtonView = Spawn(new TimerData(i + 1));
                    _showHideButtonsContainer.Add(timerCallButtonView.ShowHideButton);
                }
            }
            else
            {
                for (var i = 0; i < _currentSaveData.TimerData.Count; i++)
                {
                    var timerCallButtonView = Spawn(_currentSaveData.TimerData[i]);
                    _showHideButtonsContainer.Add(timerCallButtonView.ShowHideButton);
                }
            }

            _showHideButtonsContainer.Show();
        }

        private void OnApplicationQuit()
        {
            var newSaveData = new SaveData();

            Logger.DebugLogWarning(this, $"OnApplicationQuit {_timerModels.Count}");
            foreach (var timerModel in _timerModels)
            {
                var id = timerModel.Value.Id;
                var time = timerModel.Value.Time;
                var state = timerModel.Value.State;
                newSaveData.TimerData.Add(new TimerData(id, time, state));
                Logger.DebugLogWarning(this, $"{id} {time} {state}");
            }

            _saveSystem.Save(newSaveData);

            Logger.DebugLogWarning(this, "Saved");
        }

        // TODO: refactor
        public TimerViewModel GetTimerViewModelById(int id)
        {
            return _timerViewModels.GetValueOrDefault(id);
        }

        // TODO: refactor
        public TimerCallButtonViewModel GetCallButtonViewModelById(int id)
        {
            return _timerCallButtonViewModels.GetValueOrDefault(id);
        }

        private TimerCallButtonView Spawn(TimerData timerData)
        {
            var id = timerData.Id;
            var time = timerData.Time;
            var state = timerData.State;
            var timerModel = new TimerModel(id, time, state);

            var timerCallButtonView = _timerCallButtonViewFactory.Create();
            var timerCallButtonViewModel = new TimerCallButtonViewModel
            (
                id,
                this,
                _timerView,
                timerCallButtonView,
                timerModel,
                new TimerCallButtonModel(string.Format(_nameMask, id))
            );
            timerCallButtonViewModel.Initialize();
            timerCallButtonView.BindTo(timerCallButtonViewModel).AddTo(this);
            _timerCallButtonViewModels.TryAdd(id, timerCallButtonViewModel);

            var timerViewModel = new TimerViewModel
            (
                id,
                this,
                timerCallButtonViewModel,
                _timerView,
                timerModel
            );
            timerViewModel.Initialize();
            _timerModels.TryAdd(id, timerModel);
            _timerViewModels.TryAdd(id, timerViewModel);

            Logger.DebugLog(this, $"Spawned {id} {time} {state}");

            return timerCallButtonView;
        }

        public void Show()
        {
            _showHideButtonsContainer.Show();
        }

        public void Hide()
        {
            _showHideButtonsContainer.Hide();
        }
    }
}