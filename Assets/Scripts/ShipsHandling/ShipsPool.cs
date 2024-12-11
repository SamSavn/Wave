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
        private PrefabsService _prefabsService;

        public int Count => _pool.Count;

        public ShipsPool()
        {
            _prefabsService = ServiceLocator.Instance.Get<PrefabsService>();
            _prefabsService.OnShipsLoaded?.Add(OnPrefabsLoaded);
        }

        public GameObject GetShip(int prefabIndex)
        {
            if (_pool.TryGetValue(prefabIndex, out GameObject prefab) && prefab != null)
            {
                _pool[prefabIndex] = null;
                return prefab;
            }

            if (!prefabIndex.IsInCollectionRange(_shipPrefabs))
                return null;

            return GameObject.Instantiate(_shipPrefabs[prefabIndex], _poolContainer);
        }

        public void RecycleShip(GameObject ship, int prefabIndex)
        {
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
