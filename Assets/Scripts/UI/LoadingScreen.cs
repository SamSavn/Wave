using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wave.Services;

namespace Wave.UI.Screens
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _loadingBar;

        private const int PROGRESS_STEPS = 3;
        private const float TOTAL_FAKE_LOADING_TIME = 5f;

        private AssetsService _prefabsService;

        private float _realProgress = 0f;
        private float _fakeProgress = 0f;
        private float _elapsedTime = 0f;

        private bool _isLoading = false;

        private void Awake()
        {
            _isLoading = true;
        }

        private void Start()
        {
            _loadingBar.fillAmount = 0;
            StartCoroutine(WaitForServices());
        }

        private void Update()
        {
            if (!_isLoading)
                return;

            _elapsedTime += Time.deltaTime;
            _fakeProgress = Mathf.Clamp01(_elapsedTime / TOTAL_FAKE_LOADING_TIME);

            float targetProgress = Mathf.Max(_realProgress, _fakeProgress);
            _loadingBar.fillAmount = Mathf.Lerp(_loadingBar.fillAmount, targetProgress, Time.deltaTime * 5f);

            if (_loadingBar.fillAmount >= 0.99f && ServiceLocator.Instance.IsReady && _fakeProgress >= 1f)
            {
                _isLoading = false;
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            }
        }

        private void AddProgressStep()
        {
            _realProgress += 1f / PROGRESS_STEPS;
        }

        private IEnumerator OnLoadComplete()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }

        private IEnumerator WaitForServices()
        {
            yield return new WaitUntil(() => ServiceLocator.Instance.IsReady);

            _prefabsService = ServiceLocator.Instance.Get<AssetsService>();
            _prefabsService.OnBlocksLoaded?.Add(OnBlocksLoaded);
            _prefabsService.OnShipsLoaded?.Add(OnShipsLoaded);

            AddProgressStep();
        }

        private void OnBlocksLoaded(bool success)
        {
            if (success)
                AddProgressStep();
        }

        private void OnShipsLoaded(bool success)
        {
            if (success)
                AddProgressStep();
        }
    }
}
