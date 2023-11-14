using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public class TaskExceptionLogger : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            TaskScheduler.UnobservedTaskException += (_, e) => Debug.LogException(e.Exception);
            UniTaskScheduler.UnobservedTaskException += Debug.LogException;
        }
    }
}