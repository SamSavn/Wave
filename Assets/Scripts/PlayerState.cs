using System.Collections.Generic;
using System.Linq;
using Wave.Data;

namespace Wave
{
    public class PlayerState
    {
        private PlayerData _data;

        public PlayerState(PlayerData data)
        {
            _data = data;
        }

        public ShipData EquippedShip => _data.equippedShip;
        public HashSet<ShipData> UnlockedShips => _data.unlockedShips;
        public int BestScore => _data.bestScore;
        public int Coins => _data.coins;

        public void EquipShip(int index, int version)
        {
            _data.equippedShip = new ShipData
            {
                index = index,
                version = version
            };
        }

        public void UnlockShip(int index, int version = 0)
        {
            var ship = _data.unlockedShips.FirstOrDefault(s => s.index == index);

            if (ship == null)
            {
                _data.unlockedShips.Add(new ShipData
                {
                    index = index,
                    version = version,
                    unlockedVersions = new HashSet<int> { version }
                });
            }
            else
            {
                ship.unlockedVersions.Add(version);
            }
        }

        public bool IsShipEquipped(int index, int version) =>
            _data.equippedShip != null && _data.equippedShip.index == index && _data.equippedShip.version == version;

        public bool IsShipUnlocked(int index) =>
            _data.unlockedShips.Any(s => s.index == index);

        public bool IsVersionUnlocked(int index, int version) =>
            _data.unlockedShips.Any(s => s.index == index && s.unlockedVersions.Contains(version));

        public void SetBestScore(int value) => _data.bestScore = value;
        public void SetCoins(int value) => _data.coins = value;

        public PlayerData GetRawData() => _data;
    }
}