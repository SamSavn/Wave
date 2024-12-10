using System;
using UnityEngine;
using Wave.Database;
using Wave.Events;

namespace Wave.Services
{
	public enum PrefabType
	{
		LevelBlock,
		PlayerShip
	}

	public class PrefabsService : IService
	{
        private const string BLOCKS_ADDRESS = "EnvironmentBlocksDB";
        private const string SHIPS_ADDRESS = "PlayerShipsDB";

        private readonly AddressablesService _addressablesService;
		private EnvironmentBlocksDB _environmentBlocksDB;
		private PlayerShipsDB _playerShipsDB;

		public EventDisparcher<bool> OnBlocksLoaded { get; } = new EventDisparcher<bool>();
		public EventDisparcher<bool> OnShipsLoaded { get; } = new EventDisparcher<bool>();

		public PrefabsService(AddressablesService addressablesService)
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
		}

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
					return _playerShipsDB?.GetAllPrefabs();

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
