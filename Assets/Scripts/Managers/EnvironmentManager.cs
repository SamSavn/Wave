using System.Collections.Generic;
using UnityEngine;
using Wave.Environment;
using Wave.Services;

namespace Wave.Managers
{
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField] private GameObject _initialBlockPrefab;
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private int _poolCapacity = 10;
        [SerializeField] private int _maxBlocks = 3;
        [SerializeField] private float _speed = 5f;

        private List<EnvironmentBlock> _blocks = new List<EnvironmentBlock>();
        private Queue<EnvironmentBlock> _pool = new Queue<EnvironmentBlock>();

        private InputService _inputService;
        private UpdateService _updateService;

        private bool _moving;

        private void Awake()
        {
            _inputService = ServiceLocator.Instance.Get<InputService>();
            _updateService = ServiceLocator.Instance.Get<UpdateService>();

            InitializePool();

            _inputService.OnGameInputDown.Add(OnInputDown);
            _updateService.Update.Add(CustomUpdate);
        }

        private void Start()
        {
            SpawnInitialBlocks();
        }

        private void OnDestroy()
        {
            _inputService.OnGameInputDown.Remove(OnInputDown);
            _updateService.Update.Remove(CustomUpdate);
        }

        private void CustomUpdate(float dt)
        {
            if (!_moving)
                return;

            MoveBlocks();
            TryRecycleBlocks();
        }

        private void InitializePool()
        {
            for (int i = 0; i < _poolCapacity; i++)
            {
                AddBlock(_prefabs[Random.Range(0, _prefabs.Length)]);
            }
        }

        private void AddBlock(GameObject prefab)
        {
            GameObject clone = Instantiate(prefab, transform);
            clone.SetActive(false);
            _pool.Enqueue(clone.GetComponent<EnvironmentBlock>());
        }

        private void SpawnInitialBlocks()
        {
            EnvironmentBlock block = GetInitialBlock();
            block.Place(0);
            _blocks.Add(block);

            for (int i = 0; i < _maxBlocks; i++)
            {
                block = GetBlockFromPool();
                block.Place(i + 1);

                _blocks.Add(block);
            }
        }

        private void MoveBlocks()
        {
            foreach (EnvironmentBlock block in _blocks)
                block.Move(_speed);
        }

        private void TryRecycleBlocks()
        {
            if (_blocks.Count == 0)
                return;

            EnvironmentBlock firstBlock = _blocks[0];
            EnvironmentBlock newBlock;

            if (firstBlock.Position.z < -firstBlock.Width)
            {
                firstBlock.SetActive(false);

                if (!firstBlock.IsInitial)
                    _pool.Enqueue(firstBlock);

                _blocks.RemoveAt(0);

                newBlock = GetBlockFromPool();
                newBlock.Recycle(_blocks[^1]);
                _blocks.Add(newBlock);
            }
        }

        private EnvironmentBlock GetInitialBlock()
        {
            if (_initialBlockPrefab == null)
                return GetBlockFromPool();

            GameObject blockObject = Instantiate(_initialBlockPrefab, transform);

            if (blockObject.TryGetComponent(out EnvironmentBlock block))
                return block;

            return null;
        }

        private EnvironmentBlock GetBlockFromPool()
        {
            if (_pool.Count > 0)
                return _pool.Dequeue();

            GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];
            GameObject blockObject = Instantiate(prefab, transform);

            if (blockObject.TryGetComponent(out EnvironmentBlock block))
                return block;

            return null;
        }

        private void OnInputDown() => _moving = true;
    } 
}

