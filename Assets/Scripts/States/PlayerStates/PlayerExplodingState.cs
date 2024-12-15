using UnityEngine;
using Wave.Actors;

namespace Wave.States.PlayerStates
{
    public class PlayerExplodingState : IState
    {
        private Player _player;
        private ParticleSystem _particle;

        public PlayerExplodingState(Player player, ParticleSystem particle)
        {
            _player = player;
            _particle = particle;
        }

        public void Enter()
        {
            _player.SetVisible(false);
            _particle.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void Execute()
        {

        }

        public void Exit()
        {
            _particle.Stop();
            _particle.gameObject.SetActive(false);
            _player.SetVisible(true);
            Time.timeScale = 1f;

            _particle = null;
            _player = null;
        }
    }
}
