using UnityEngine;
using Wave.Settings;
using Wave.Ships;

namespace Wave.Services
{
    public class ShipsService : IService
    {
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

        public int GetPrice(int index, int version)
        {
            if (!IsShipUnlocked(index))
                return _assetsService.GetShipPrice();

            if (!IsVersionUnlocked(index, version))
                return _assetsService.GetVersionPrice();

            return 0;
        }

        public int GetShipsCount() => _pool.Count;

        public GameObject GetShip(int index) => _pool.GetShip(index);
        public ShipInfo GetInfo(int index) => _assetsService.GetShipInfo(index);        

        public GameObject GetModel(int index, int version = 0)
        {
            ShipInfo ship = _assetsService.GetShipInfo(index);
            return version == 0
                ? ship.GetPrefab()
                : ship.GetVersions()[version - 1].GetPrefab();
        }

        public string GetShipName(int index)
        {
            ShipInfo ship = _assetsService.GetShipInfo(index);
            return ship != null ? ship.GetName() : string.Empty;
        }

        public Vector3 GetTrailOrigin(int index)
        {
            ShipInfo ship = _assetsService.GetShipInfo(index);
            return ship != null ? ship.GetTrailOrigin() : Vector3.zero;
        }

        public void RecycleShip(GameObject ship, int index) => _pool.RecycleShip(ship, index);

        public bool IsShipUnlocked(int index) => _playerService.IsShipUnlocked(index);
        public bool IsVersionUnlocked(int index, int version) => _playerService.IsVersionUnlocked(index, version);
        public bool IsShipEquipped(int index, int version) => _playerService.IsShipEquipped(index, version);
        public bool IsShipEquipped(int index) => _playerService.IsShipEquipped(index);
        public void UnlockShip(int index, int version = 0) => _playerService.UnlockShip(index, version);
    }
}
