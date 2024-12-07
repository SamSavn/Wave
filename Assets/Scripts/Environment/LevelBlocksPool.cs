using System;
using System.Collections.Generic;
using UnityEngine;
using Wave.Services;

namespace Wave.Environment
{
    public class LevelBlocksPool : IDisposable
    {
        private Queue<LevelBlock> _pool = new();
        private Transform _parentTransform;
        private PrefabsService _prefabsService;
        private GameObject _initialPrefab;

        public LevelBlocksPool(Transform parentTransform, PrefabsService prefabsService)
        {
            _parentTransform = parentTransform;
            _prefabsService = prefabsService;
        }

        public void InitializePool(int capacity)
        {
            _initialPrefab = _prefabsService.GetInitialPrefab(PrefabType.EnvironmentBlock);

            for (int i = 0; i < capacity; i++)
            {
                AddBlock(_prefabsService.GetRandomPrefab(PrefabType.EnvironmentBlock));
            }
        }

        public void AddBlock(GameObject prefab)
        {
            GameObject clone = GameObject.Instantiate(prefab, _parentTransform);
            clone.SetActive(false);
            _pool.Enqueue(clone.GetComponent<LevelBlock>());
        }

        public LevelBlock GetInitialBlock()
        {
            if (_initialPrefab == null)
                return GetBlockFromPool();

            GameObject blockObject = GameObject.Instantiate(_initialPrefab, _parentTransform);
            return blockObject.GetComponent<LevelBlock>();
        }

        public LevelBlock GetBlockFromPool()
        {
            if (_pool.Count > 0)
                return _pool.Dequeue();

            GameObject blockObject = GameObject.Instantiate(_prefabsService.GetRandomPrefab(PrefabType.EnvironmentBlock), _parentTransform);
            return blockObject.GetComponent<LevelBlock>();
        }

        public void RecycleBlock(LevelBlock block)
        {
            block.SetActive(false);
            _pool.Enqueue(block);
        }

        public void Dispose()
        {
            _pool.Clear();
            _pool = null;

            _parentTransform = null;
            _initialPrefab = null;
            _prefabsService = null;
        }
    }
}
