using System.Collections.Generic;
using UnityEngine;
using Wave.Extentions;
using Wave.Services;
using Wave.States;
using Wave.States.LevelStates;

namespace Wave.Environment
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private int _poolCapacity = 10;
        [SerializeField] private int _maxBlocks = 3;
        [SerializeField] private float _speed = 5f;

        private List<LevelBlock> _blocks = new();
        private LevelBlocksPool _blocksPool;
        private StateMachine _stateMachine;

        private PrefabsService _prefabsService;

        private void Awake()
        {
            _prefabsService = ServiceLocator.Instance.Get<PrefabsService>();

            _blocksPool = new LevelBlocksPool(transform, _prefabsService);
            _stateMachine = new StateMachine();

            _prefabsService.OnBlocksLoaded?.Add(OnPrefabsLoaded);
        }

        private void OnDestroy()
        {
            _stateMachine.Dispose();
            _blocksPool.Dispose();
        }

        public void StartMoving() => _stateMachine.SetState(new LevelMovingState(_blocks, _speed));
        public void StopMoving() => _stateMachine.SetState(new LevelIdleState(_blocksPool, _poolCapacity, _maxBlocks, SpawnBlocks));

        public void ResetLevel()
        {
            _blocks.Foreach(block => _blocksPool.RecycleBlock(block));
            _blocks.Clear();
            StopMoving();
        }

        private void SpawnBlocks()
        {
            LevelBlock block = _blocksPool.GetInitialBlock();
            block.Place(0);
            _blocks.Add(block);

            for (int i = 0; i < _maxBlocks; i++)
            {
                block = _blocksPool.GetBlockFromPool();
                block.Place(i + 1);
                _blocks.Add(block);
            }
        }

        private void OnPrefabsLoaded(bool success)
        {
            if (!success)
            {
                Debug.LogError("Unable to set current level: something went wrong loading the prefabs");
                return;
            }

            _prefabsService.OnBlocksLoaded?.Remove(OnPrefabsLoaded);

            ResetLevel();
            ServiceLocator.Instance.Get<GameService>().SetLevel(this);
        }
    }
}

