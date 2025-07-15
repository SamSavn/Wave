using System.Collections.Generic;

namespace Wave.Data
{
	public class PlayerData
	{
		public ShipData equippedShip;
		public int bestScore;
		public int coins;

        public HashSet<int> unlockedShips = new();
        public Dictionary<int, HashSet<int>> unlockedVersions = new();

		public PlayerData()
		{
			equippedShip = new ShipData() { index = 0, version = 0 };
			bestScore = 0;
			coins = 0;

			unlockedShips = new HashSet<int>() { 0 };
			unlockedVersions = new Dictionary<int, HashSet<int>>()
            {
                { 0, new HashSet<int> { 0 } }
            };
        }
    } 
}
