using DG.Tweening;
using UnityEngine;
using Wave.Extentions;
using Wave.Services;

namespace Wave.Ships
{
    public class ShipCamera : MonoBehaviour
    {
        [SerializeField] private Transform _shipContainer;
        [SerializeField] private bool _isMainCamera = false;

        private const float FLOATING_VALUE = 6f;
        private const float FLOATING_DURATION = 1f;

        private ShipsService _shipsService;
        private GameObject _currentShip;
        private MeshFilter _currentShipMeshFilter;
        private Tweener _tweener;

        private int _shipIndex;

        private void Reset()
        {
            _shipContainer = GetComponentInChildren<Transform>();
        }

        private void Awake()
        {
            if (_shipContainer == null )
                _shipContainer = GetComponentInChildren<Transform>();
        }

        private void OnDisable()
        {
            _tweener?.Rewind();
            _tweener?.Kill();
            _tweener = null;

            RecycleCurrentShip();
        }

        public void SetShip(GameObject ship, int index)
        {
            RecycleCurrentShip();

            if (ship == null)
                return;

            _currentShip = ship;
            _currentShipMeshFilter = _currentShip.GetComponent<MeshFilter>();
            _shipIndex = index;

            _currentShip.SetLayer(Layer.ShipRender);
            _currentShip.transform.SetParent(_shipContainer);
            _currentShip.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _currentShip.SetActive(true);

            StartFloating();
        }

        public void SetShipVersion(GameObject versionPrefab)
        {
            Mesh versionMesh = versionPrefab.GetComponent<MeshFilter>().sharedMesh;
            _currentShipMeshFilter.mesh = versionMesh;
        }

        [ContextMenu("Start Floating")]
        public void StartFloating()
        {
            if (!_isMainCamera || _currentShip == null || (_tweener != null && _tweener.IsPlaying()))
                return;

            _tweener = _shipContainer.DOLocalMoveY(FLOATING_VALUE, FLOATING_DURATION)
                                        .SetLoops(-1, LoopType.Yoyo)
                                        .SetEase(Ease.InOutSine);
        }

        public void RecycleCurrentShip()
        {
            if (_currentShip == null)
                return;

            _shipsService ??= ServiceLocator.Instance.Get<ShipsService>();
            _shipsService.RecycleShip(_currentShip, _shipIndex);
            _currentShip = null;
            _shipIndex = -1;
        }
    }
}
