using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Layer.Infrastructure
{
    public class SceneTransitionManager : MonoBehaviour
    {
        /// <summary> 任意シーンへ遷移する </summary>
        /// <param name="sceneName">遷移先のシーン</param>
        public async UniTask TransitionToScene(string sceneName)
        {
            await LoadSceneAsync(sceneName);
        }

        private void Awake()
        {
            if (!ServiceLocator.IsRegistered<SceneTransitionManager>())
            {
                ServiceLocator.RegisterService(this);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (ServiceLocator.TryGet(out SceneTransitionManager current) && ReferenceEquals(current, this))
            {
                ServiceLocator.UnregisterService<SceneTransitionManager>();
            }
        }

        /// <summary>
        /// シーン遷移を非同期で行う汎用コルーチン
        /// </summary>
        /// <param name="sceneName">遷移先</param>
        private async UniTask LoadSceneAsync(string sceneName)
        {
            Debug.Log($"{sceneName}：へ遷移する");
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
