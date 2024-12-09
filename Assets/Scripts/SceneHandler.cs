using UnityEngine;
using Wave.Services;
using Wave.Ships;

namespace Wave.Handlers
{
    public enum SceneType
    {
        Game,
        Shop
    }

	public class SceneHandler : MonoBehaviour
	{
        [SerializeField] private GameObject _gameElements;
		[SerializeField] private ShipCamerasHandler _shipCamerasHandler;

        private void Awake()
        {
            ServiceLocator.Instance.Get<SceneService>().SetSceneHandler(this);
        }

        public void SetScene(SceneType sceneType)
        {
            _gameElements.SetActive(sceneType == SceneType.Game);
            _shipCamerasHandler.gameObject.SetActive(sceneType == SceneType.Shop);
        }
    } 
}
