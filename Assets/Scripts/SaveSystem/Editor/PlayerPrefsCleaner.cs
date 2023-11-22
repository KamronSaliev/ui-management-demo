using UnityEditor;
using UnityEngine;

namespace UIManagementDemo.SaveSystem.Editor
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