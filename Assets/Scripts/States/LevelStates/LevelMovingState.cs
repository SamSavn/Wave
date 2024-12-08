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

        public LevelMovingState(List<LevelBlock> blocks, float speed, LevelBlocksPool blocksPool)
        {
            _blocks = blocks;
            _speed = speed;
            _blocksPool = blocksPool;
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
            _blocks = null;
            _blocksPool = null;
        }

        private void TryRecycleBlocks()
        {
            if (_blocks.Count == 0)
                return;

            LevelBlock firstBlock = _blocks[0];

            if (firstBlock.Position.z >= -firstBlock.Width)
                return;

            _blocksPool.RecycleBlock(firstBlock);
            _blocks.RemoveAt(0);

            LevelBlock newBlock = _blocksPool.GetBlockFromPool();
            newBlock.Recycle(_blocks[^1]);
            _blocks.Add(newBlock);
        }
    }
}
