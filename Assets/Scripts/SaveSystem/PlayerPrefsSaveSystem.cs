using System.Collections.Generic;
using UnityEngine;

namespace UIManagementDemo.SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string TimerCountSaveKey = "TimerCount";
        private const string TimerIdMaskSaveKey = "TimerId_{0}";
        private const string TimerTimerMaskSaveKey = "TimerTimer_{0}";
        private const string TimerStateMaskSaveKey = "TimerState_{0}";

        public void Save(SaveData data)
        {
            PlayerPrefs.SetInt(TimerCountSaveKey, data.TimerData.Count);

            for (var i = 0; i < data.TimerData.Count; i++)
            {
                PlayerPrefs.SetInt(string.Format(TimerIdMaskSaveKey, i), data.TimerData[i].Id);
                PlayerPrefs.SetInt(string.Format(TimerTimerMaskSaveKey, i), data.TimerData[i].Time);
                PlayerPrefs.SetInt(string.Format(TimerStateMaskSaveKey, i), data.TimerData[i].State ? 1 : 0);
            }

            PlayerPrefs.Save();
        }

        public SaveData Load()
        {
            var loadedData = new SaveData();

            var count = PlayerPrefs.GetInt(TimerCountSaveKey, 0);
            loadedData.TimerData.Clear();

            for (var i = 0; i < count; i++)
            {
                var id = PlayerPrefs.GetInt(string.Format(TimerIdMaskSaveKey, i), 0);
                var time = PlayerPrefs.GetInt(string.Format(TimerTimerMaskSaveKey, i), 0);
                var state = PlayerPrefs.GetInt(string.Format(TimerStateMaskSaveKey, i), 0) != 0;
                var timerData = new TimerData(id, time, state);

                loadedData.TimerData.Add(timerData);
            }

            return loadedData;
        }
    }
}