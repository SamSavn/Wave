using System.Collections.Generic;
using UnityEngine;
using Wave.Extentions;
using Wave.Services;

namespace Wave.Ships
{
    public class ShipsPool
    {
        private Dictionary<int, GameObject> _pool = new();
        private GameObject[] _shipPrefabs;
        private Transform _poolContainer;
        private AssetsService _prefabsService;

        public int Count => _pool.Count;

        public ShipsPool()
        {
            _prefabsService = ServiceLocator.Instance.Get<AssetsService>();
            _prefabsService.OnShipsLoaded?.Add(OnPrefabsLoaded);
        }

        public GameObject GetShip(int prefabIndex)
        {
            if (_pool.TryGetValue(prefabIndex, out GameObject prefab) && prefab != null)
                return prefab;

            if (!prefabIndex.IsInCollectionRange(_shipPrefabs))
            {
                Debug.LogWarning($"Unable to get ship: {nameof(prefabIndex)} ({prefabIndex}) is out of range (0-{_shipPrefabs.Length})");
                return null;
            }

            GameObject ship = GameObject.Instantiate(_shipPrefabs[prefabIndex], _poolContainer);
            _pool[prefabIndex] = ship;

            return ship;
        }

        public void RecycleShip(GameObject ship, int prefabIndex)
        {
            if (ship == null)
            {
                Debug.LogError($"Unable to recycle ship: {nameof(ship)} is NULL");
                return;
            }

            if (prefabIndex < 0)
            {
                Debug.LogError($"Unable to recycle ship: {nameof(prefabIndex)} cannot be a negative value");
                return;
            }

            ship.SetActive(false);
            ship.SetLayer(Layer.Default);
            ship.transform.SetParent(_poolContainer);
            _pool[prefabIndex] = ship;
        }

        private void OnPrefabsLoaded(bool success)
        {
            if (!success)
                return;

            _prefabsService.OnShipsLoaded?.Remove(OnPrefabsLoaded);

            _shipPrefabs = _prefabsService.GetAllPrefabs(PrefabType.PlayerShip);
            _poolContainer = new GameObject("[Ships Pool]").transform;
            GameObject.DontDestroyOnLoad(_poolContainer);

            int count = _shipPrefabs.Length;
            for (int i = 0; i < count; i++)
            {
                RecycleShip(GameObject.Instantiate(_shipPrefabs[i], _poolContainer), i);
            }
        }
    }
}
