using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIManagementDemo.Core
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            LoadSceneAsync().Forget();
        }

        private async UniTaskVoid LoadSceneAsync()
        {
            await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}