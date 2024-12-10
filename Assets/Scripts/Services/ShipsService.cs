using System.Collections.Generic;
using UnityEngine;
using Wave.Ships;

namespace Wave.Services
{
	public class ShipsService : IService
	{
		private const string DATA_KEY = "ShipsData";
        private const int SHIPS_BASE_PRICE = 100;

        private DataService _dataService;

		private ShipCamerasHandler _shipCamerasHandler;
        private ShipsPool _pool;

		private List<int> _unlockedShips = new List<int>();

		public ShipsService(DataService dataService)
		{
			_dataService = dataService;
			_pool = new ShipsPool();
			_unlockedShips = _dataService.GetUnlockedShips() != null
								? new List<int>(_dataService.GetUnlockedShips())
								: new List<int>();

			UnlockShip(0);
		}

		public void SetShipCamerasHandler(ShipCamerasHandler shipCamerasHandler) => _shipCamerasHandler = shipCamerasHandler;
		public void SetSelectedShip(int index) =>_shipCamerasHandler.SetShips(_pool, index);

		public int GetShipPrice(int index) => SHIPS_BASE_PRICE + index / 5 * (SHIPS_BASE_PRICE / 2);
		public int GetShipsCount() => _pool.Count;
		public GameObject GetShip(int index) => _pool.GetShip(index);
		public void RecycleShip(GameObject ship, int index) => _pool.RecycleShip(ship, index);

		public bool IsShipUnlocked(int index) => _unlockedShips.Contains(index);
		public bool IsShipEquiped(int index) => _dataService.GetEquipedShip() == index;

		public void UnlockShip(int index)
		{
			if (IsShipUnlocked(index))
				return;

			_unlockedShips.Add(index);
			_dataService.SaveUnlockedShips(_unlockedShips.ToArray());
		}
    }
}
