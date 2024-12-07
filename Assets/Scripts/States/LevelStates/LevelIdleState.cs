using System;
using UnityEngine;
using Wave.Environment;

namespace Wave.States.LevelStates
{
    public class LevelIdleState : IState
    {
        private LevelBlocksPool _blocksPool;
        private Action _spawnBlocks;
        private readonly int _poolCapacity;
        private readonly int _maxBlocks;

        public LevelIdleState(LevelBlocksPool blocksPool, int poolCapacity, int maxBlocks, Action spawnBlocks)
        {
            _blocksPool = blocksPool;
            _poolCapacity = poolCapacity;
            _maxBlocks = maxBlocks;
            _spawnBlocks = spawnBlocks;
        }

        public void Enter()
        {
            _blocksPool.InitializePool(_poolCapacity);
            _spawnBlocks?.Invoke();
        }

        public void Execute()
        {
            
        }

        public void Exit() 
        {
            _blocksPool = null;
            _spawnBlocks = null;
        }
    }
}
