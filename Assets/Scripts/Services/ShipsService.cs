using UnityEngine;
using Wave.Ships;

namespace Wave.Services
{
	public class ShipsService : IService
	{
		private ShipCamerasHandler _shipCamerasHandler;
        private ShipsPool _pool;

		public ShipsService()
		{
			_pool = new ShipsPool();
		}

		public void SetShipCamerasHandler(ShipCamerasHandler shipCamerasHandler) => _shipCamerasHandler = shipCamerasHandler;

		public void SetSelectedShip(int index)
		{
			_shipCamerasHandler.SetShips(_pool, index);
		}

		public int GetShipsCount() => _pool.Count;
		public GameObject GetShip(int index) => _pool.GetShip(index);
		public void RecycleShip(GameObject ship, int index) => _pool.RecycleShip(ship, index);
    }
}
