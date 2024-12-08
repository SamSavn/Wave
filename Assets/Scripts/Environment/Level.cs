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

        public void StartMoving() => _stateMachine.SetState(new LevelMovingState(_blocks, _speed, _blocksPool));
        public void StopMoving() => _stateMachine.SetState(new LevelIdleState(_blocks, _blocksPool, _poolCapacity, _maxBlocks));
        public void Pause() => _stateMachine.SetState(new LevelPausedState());

        public void ResetLevel()
        {
            RecycleAllBlocks();
            StopMoving();
        }

        private void RecycleAllBlocks()
        {
            _blocks.Foreach(block => _blocksPool.RecycleBlock(block));
            _blocks.Clear();
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

