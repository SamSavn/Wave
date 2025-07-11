using UnityEngine;
using Wave.Services;
using Wave.Settings;

namespace Wave.Ships
{
	public class ShipCamerasHandler : MonoBehaviour
	{
		[SerializeField] private ShipCamera _previousShipCam;
		[SerializeField] private ShipCamera _selectedShipCam;
		[SerializeField] private ShipCamera _nextShipCam;

        private void Awake()
        {
            ServiceLocator.Instance.Get<ShipsService>().SetShipCamerasHandler(this);
        }

        public void SetShips(ShipsPool pool, int selectedIndex)
		{
			int previousIndex = selectedIndex - 1;
			int nextIndex = selectedIndex + 1;

            _previousShipCam.SetShip(pool.GetShip(previousIndex), previousIndex);
			_selectedShipCam.SetShip(pool.GetShip(selectedIndex), selectedIndex);
			_nextShipCam.SetShip(pool.GetShip(nextIndex), nextIndex);
		}

		public void SetShipVersion(GameObject versionPrefab)
		{
			_selectedShipCam.SetShipVersion(versionPrefab);
        }
    }
}
