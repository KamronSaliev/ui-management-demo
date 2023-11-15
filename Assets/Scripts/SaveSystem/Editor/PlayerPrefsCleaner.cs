using UnityEditor;
using UnityEngine;

namespace SaveSystem.Editor
{
    public class PlayerPrefsCleaner : UnityEditor.Editor
    {
        [MenuItem("Tools/Clean PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}