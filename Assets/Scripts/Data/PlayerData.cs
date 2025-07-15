using System.Collections.Generic;

namespace Wave.Data
{
	public class PlayerData
	{
		public ShipData equippedShip;
		public int bestScore;
		public int coins;

        public HashSet<ShipData> unlockedShips = new();
    } 
}
