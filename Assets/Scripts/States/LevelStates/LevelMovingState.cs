using System.Collections.Generic;
using Wave.Environment;
using Wave.Extentions;

namespace Wave.States.LevelStates
{
    public class LevelMovingState : IState
    {
        private List<LevelBlock> _blocks;
        private LevelBlocksPool _blocksPool;
        private readonly float _speed;

        public LevelMovingState(List<LevelBlock> blocks, LevelBlocksPool pool, float speed)
        {
            _blocks = blocks != null ? blocks : new List<LevelBlock>();
            _blocksPool = pool;
            _speed = speed;
        }

        public void Enter() 
        {
            
        }

        public void Execute()
        {
            _blocks.Foreach(block => block.Move(_speed));
            TryRecycleBlocks();
        }

        public void Exit() 
        {
            _blocks.Clear();
            _blocks = null;
        }

        private void TryRecycleBlocks()
        {
            if (_blocks.Count == 0)
                return;

            LevelBlock firstBlock = _blocks[0];
            LevelBlock newBlock;

            if (firstBlock.Position.z < -firstBlock.Width)
            {
                if (!firstBlock.IsInitial)
                    _blocksPool.RecycleBlock(firstBlock);

                _blocks.RemoveAt(0);

                newBlock = _blocksPool.GetBlockFromPool();
                newBlock.Recycle(_blocks[^1]);
                _blocks.Add(newBlock);
            }
        }
    }
}
