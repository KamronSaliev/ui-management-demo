using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Utilities.ExtensionMethods
{
    public static class UniTaskExtensions
    {
        public static async UniTaskVoid SwitchToMainThread(this Action callback)
        {
            await UniTask.SwitchToMainThread();

            callback();
        }

        public static void Stop(ref CancellationTokenSource cts)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }
    }
}