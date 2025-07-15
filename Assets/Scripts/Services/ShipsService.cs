using UnityEngine;
using Wave.Settings;
using Wave.Ships;

namespace Wave.Services
{
    public class ShipsService : IService
    {
        private const int SHIPS_BASE_PRICE = 100;

        private PlayerService _playerService;
        private AssetsService _assetsService;
        private ShipCamerasHandler _shipCamerasHandler;
        private ShipsPool _pool;

        public ShipsService(PlayerService playerState, AssetsService assetsService)
        {
            _playerService = playerState;
            _assetsService = assetsService;
            _pool = new ShipsPool();
        }

        public void SetShipCamerasHandler(ShipCamerasHandler handler) => _shipCamerasHandler = handler;
        public void SetSelectedShip(int index) => _shipCamerasHandler.SetShips(_pool, index);
        public void SetShipVersion(int shipIndex, int versionIndex) => _shipCamerasHandler.SetShipVersion(GetModel(shipIndex, versionIndex));

        public int GetShipPrice(int index) => SHIPS_BASE_PRICE /*+ dynamic logic later*/;
        public int GetShipsCount() => _pool.Count;
        public GameObject GetShip(int index) => _pool.GetShip(index);
        public ShipInfo GetStats(int index) => _assetsService.GetShipInfo(index);
        public GameObject GetModel(int index, int version = 0)
        {
            ShipInfo ship = _assetsService.GetShipInfo(index);
            return version == 0
                ? ship.GetPrefab()
                : ship.GetVersions()[version - 1].GetPrefab();
        }

        public void RecycleShip(GameObject ship, int index) => _pool.RecycleShip(ship, index);

        public bool IsShipUnlocked(int index) => _playerService.IsShipUnlocked(index);
        public bool IsVersionUnlocked(int index, int version) => _playerService.IsVersionUnlocked(index, version);
        public bool IsShipEquipped(int index) => _playerService.IsShipEquipped(index);
        public void UnlockShip(int index, int version = 0) => _playerService.UnlockShip(index, version);
    }
}
