using UnityEngine;
using Wave.Actors;
using Wave.Actors.Effects;

namespace Wave.States.PlayerStates
{
    public class PlayerExplodingState : IState
    {
        private Player _player;
        private ParticleSystem _particle;
        private PlayerTrail _playerTrail;

        public PlayerExplodingState(Player player, ParticleSystem particle, PlayerTrail trail)
        {
            _player = player;
            _particle = particle;
            _playerTrail = trail;
        }

        public void Enter()
        {
            _playerTrail.Hide();
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
