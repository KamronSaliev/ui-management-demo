using UnityEditor;
using UnityEngine;
using Logger = Utilities.Logger;

namespace SaveSystem.Editor
{
    public class PlayerPrefsCleaner : UnityEditor.Editor
    {
        [MenuItem("Tools/Clean PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();

            Logger.DebugLogWarning(nameof(PlayerPrefsCleaner), "PlayerPrefs cleaned");
        }
    }
}