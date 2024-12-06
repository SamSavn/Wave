using UnityEngine;
using Wave.Actors;

namespace Wave.States.PlayerStates
{
    public class ExplodingState : IState
    {
        private Player _player;
        private ParticleSystem _particle;

        public ExplodingState(Player player, ParticleSystem particle)
        {
            _player = player;
            _particle = particle;
        }

        public void Enter()
        {
            _player.SetVisible(false);
            _particle.Play();
            Time.timeScale = 0;
        }

        public void Execute()
        {

        }

        public void Exit()
        {
            _particle.Stop();
            _player.SetVisible(true);
            Time.timeScale = 1f;

            _particle = null;
            _player = null;
        }
    }
}
