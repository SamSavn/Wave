using System;
using UnityEngine;
using Wave.Database;
using Wave.Events;
using Wave.Settings;
using Wave.Extentions;

namespace Wave.Services
{
	public enum PrefabType
	{
		LevelBlock,
		PlayerShip
	}

	public class AssetsService : IService
	{
        private const string BLOCKS_ADDRESS = "EnvironmentBlocksDB";
        private const string SHIPS_ADDRESS = "PlayerShipsDB";
        private const string SHIP_V_COLOR_ADDRESS = "ShipVersionColor";

        private readonly AddressablesService _addressablesService;
		private EnvironmentBlocksDB _environmentBlocksDB;
		private PlayerShipsDB _playerShipsDB;
		private GameObject _shipVersionColor;

        private GameObject[] _allShipsPrfabs = Array.Empty<GameObject>();

        public EventDisparcher<bool> OnBlocksLoaded { get; } = new EventDisparcher<bool>();
		public EventDisparcher<bool> OnShipsLoaded { get; } = new EventDisparcher<bool>();

		public AssetsService(AddressablesService addressablesService)
		{
			_addressablesService = addressablesService;

			_addressablesService.LoadSingle<EnvironmentBlocksDB>(BLOCKS_ADDRESS, (db, state) =>
			{
				if (state) _environmentBlocksDB = db;
				OnBlocksLoaded?.Invoke(state);
			});

			_addressablesService.LoadSingle<PlayerShipsDB>(SHIPS_ADDRESS, (db, state) =>
			{
				if (state) _playerShipsDB = db;
				OnShipsLoaded?.Invoke(state);
			});

			_addressablesService.LoadSingle<GameObject>(SHIP_V_COLOR_ADDRESS, (v, state) =>
			{
				if (state) _shipVersionColor = v;
			});
		}

		public GameObject GetShipVersionColorPrefab() => _shipVersionColor;
		public ShipInfo GetShipInfo(int index) => _playerShipsDB.GetShipStats(index);

		public GameObject GetInitialPrefab(PrefabType prefabType)
		{
			switch (prefabType)
			{
				case PrefabType.LevelBlock:
					return _environmentBlocksDB?.GetInitialBlock();

                case PrefabType.PlayerShip:
				default:
					return null;
			}
		}

		public GameObject[] GetAllPrefabs(PrefabType prefabType)
		{
			switch (prefabType)
			{
				case PrefabType.LevelBlock:
                    return _environmentBlocksDB?.GetAllBlocks();

                case PrefabType.PlayerShip:
					return _playerShipsDB?.GetMainPrefabs();

                default:
					return Array.Empty<GameObject>();
			}
		}

		public GameObject GetPrefabAt(PrefabType prefabType, int index)
		{
			switch (prefabType)
			{
				case PrefabType.LevelBlock:
					return _environmentBlocksDB?.GetBlockAt(index);

                case PrefabType.PlayerShip:
					return _playerShipsDB?.GetShipAt(index);

				default:
					return null;
			}
		}

		public GameObject GetRandomPrefab(PrefabType prefabType)
		{
			switch (prefabType)
			{
				case PrefabType.LevelBlock:
					return _environmentBlocksDB?.GetRandomBlock();

                case PrefabType.PlayerShip:
					return _playerShipsDB?.GetRandomShip();

				default:
					return null;
			}
		}
    }
}
