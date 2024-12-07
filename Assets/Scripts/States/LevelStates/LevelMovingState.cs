using System.Collections.Generic;
using Wave.Environment;
using Wave.Extentions;

namespace Wave.States.LevelStates
{
    public class LevelMovingState : IState
    {
        private List<LevelBlock> _blocks;
        private readonly float _speed;

        public LevelMovingState(List<LevelBlock> blocks, float speed)
        {
            _blocks = blocks != null ? blocks : new List<LevelBlock>();
            _speed = speed;
        }

        public void Enter() 
        {
            
        }

        public void Execute()
        {
            _blocks.Foreach(block => block.Move(_speed));
        }

        public void Exit() 
        {
            _blocks.Clear();
            _blocks = null;
        }
    }
}
