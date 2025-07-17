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

        private StateMachine _stateMachine;
        private List<LevelBlock> _blocks = new();
        private LevelBlocksPool _blocksPool;

        private void Awake()
        {
            _stateMachine ??= new StateMachine();
            _blocksPool = new LevelBlocksPool(transform);
            ServiceLocator.Instance.Get<GameService>().SetLevel(this);
        }

        private void OnDestroy()
        {
            _stateMachine.Dispose();
            _blocksPool.Dispose();
        }

        public void Move() => _stateMachine.SetState(new LevelMovingState(_blocks, _speed, _blocksPool));
        public void Idle() => _stateMachine.SetState(new LevelIdleState(_blocks, _blocksPool, _poolCapacity, _maxBlocks));
        public void Pause() => _stateMachine.SetState(new LevelPausedState());

        public void ResetLevel()
        {
            RecycleAllBlocks();
            Idle();
        }

        private void RecycleAllBlocks()
        {
            _blocks.Foreach(block => _blocksPool.RecycleBlock(block));
            _blocks.Clear();
        }
    }
}

