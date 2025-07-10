using System.Collections.Generic;
using Wave.Environment;
using Wave.Extentions;

namespace Wave.States.LevelStates
{
    public class LevelIdleState : IState
    {
        private readonly List<LevelBlock> _blocks;
        private readonly LevelBlocksPool _blocksPool;
        private readonly int _poolCapacity;
        private readonly int _maxBlocks;
        private bool _resetBlocks;

        public LevelIdleState(List<LevelBlock> activeBlocks, LevelBlocksPool blocksPool, int poolCapacity, int maxBlocks)
        {
            _blocks = activeBlocks;
            _blocksPool = blocksPool;
            _poolCapacity = poolCapacity;
            _maxBlocks = maxBlocks;
            _resetBlocks = _maxBlocks > 0;
        }

        public LevelIdleState(List<LevelBlock> activeBlocks) : this(activeBlocks, null, 0, 0)
        {
        }

        public void Enter()
        {
            if (!_resetBlocks)
            {
                _blocks.ForEach(block => block.SetActive(true, false));
                return;
            }

            if (!_blocksPool.Initialized)
            {
                _blocksPool.InitializePool(_poolCapacity);
            }

            RecycleBlocks();
            SpawnBlocks();
        }

        public void Execute() 
        {
            
        }

        public void Exit()
        {
            
        }

        private void RecycleBlocks()
        {
            _blocks.Foreach(block => _blocksPool.RecycleBlock(block));
            _blocks.Clear();
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
    }

}
