using UnityEngine;
using Wave.Extentions;
using Wave.Services;

namespace Wave.Ships
{
    public class ShipCamera : MonoBehaviour
    {
        [SerializeField] private Transform _shipContainer;

        private ShipsService _shipsService;
        private GameObject _currentShip;
        private int _shipIndex;

        private void Reset()
        {
            _shipContainer = GetComponentInChildren<Transform>();
        }

        private void Awake()
        {
            _shipsService = ServiceLocator.Instance.Get<ShipsService>();

            if (_shipsService == null )
                _shipContainer = GetComponentInChildren<Transform>();
        }

        public void SetShip(GameObject ship, int index)
        {
            if (_currentShip != null)
            {
                _shipsService.RecycleShip(_currentShip, _shipIndex);
                _currentShip = null;
                _shipIndex = -1;
            }

            if (ship == null)
                return;

            _currentShip = ship;
            _shipIndex = index;

            _currentShip.SetLayer(Layer.ShipRender);
            _currentShip.transform.SetParent(_shipContainer);
            _currentShip.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _currentShip.SetActive(true);
        }
    } 
}
