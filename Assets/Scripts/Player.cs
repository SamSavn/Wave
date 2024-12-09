using UnityEngine;
using Wave.Collectables;
using Wave.Extentions;
using Wave.Services;
using Wave.States;
using Wave.States.PlayerStates;

namespace Wave.Actors
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _explosionParticle;
        [SerializeField] private float _force = 10f;
        [SerializeField] private float _maxAngle = 50f;

        private InputService _inputService;
        private PrefabsService _prefabsService;
        private GameService _gameService;
        private PlayerService _playerService;
        private ShipsService _shipsService;

        private GameObject _model;
        private Collider _collider;

        private StateMachine _stateMachine;
        private float _currentAngle;

        private void Awake()
        {
            _inputService = ServiceLocator.Instance.Get<InputService>();
            _prefabsService = ServiceLocator.Instance.Get<PrefabsService>();
            _gameService = ServiceLocator.Instance.Get<GameService>();
            _playerService = ServiceLocator.Instance.Get<PlayerService>();
            _shipsService = ServiceLocator.Instance.Get<ShipsService>();

            _stateMachine = new StateMachine();
            _prefabsService.OnShipsLoaded?.Add(OnPrefabsLoaded);
            _gameService.SetPlayer(this);
        }

        private void OnDestroy()
        {
            _inputService.OnGameInputDown.Remove(OnInputDown);
            _inputService.OnGameInputUp.Remove(OnInputUp);            
            _stateMachine.Dispose();
        }

        private void AdjustRotation()
        {
            _currentAngle = Mathf.Clamp(-_rigidbody.linearVelocity.y, -_maxAngle, _maxAngle);
            transform.eulerAngles = new Vector3(_currentAngle, 0, 0);
        }

        public void SetVisible(bool active) => _model.SetActive(active);
        public void ResetState() => _stateMachine.SetState(new PlayerIdleState(transform, _rigidbody));
        public void Pause() => _stateMachine.SetState(new PlayerPausedState(_rigidbody));
        public void Resume() => _stateMachine.SetState(new PlayerFallingState(_rigidbody, AdjustRotation));
        public void Die() => _stateMachine.SetState(new PlayerExplodingState(this, _explosionParticle));

        private void OnPrefabsLoaded(bool success)
        {
            if (!success) 
                return;

            _prefabsService.OnShipsLoaded.Remove(OnPrefabsLoaded);

            _model = _shipsService.GetShip(_playerService.GetActiveShipIndex());
            _model.transform.SetParent(transform, false);
            _model.gameObject.SetLayer(Layer.Player);

            _collider = _model.GetComponent<Collider>();
            _collider.isTrigger = true;

            _inputService.OnGameInputDown.Add(OnInputDown);
            _inputService.OnGameInputUp.Add(OnInputUp);
        }

        private void OnInputDown()
        {
            if (_stateMachine.IsInState<PlayerExplodingState>() ||
                _stateMachine.IsInState<PlayerPausedState>())
            {
                return;
            }

            _stateMachine.SetState(new PlayerRisingState(_rigidbody, _force, AdjustRotation));
        }

        private void OnInputUp()
        {
            if (_stateMachine.IsInState<PlayerExplodingState>() ||
                _stateMachine.IsInState<PlayerPausedState>())
            {
                return;
            }

            _stateMachine.SetState(new PlayerFallingState(_rigidbody, AdjustRotation));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null)
                return;

            if (other.gameObject.HasLayerAndComponent(Layer.Collectible, out ICollectable collectable))
            {
                collectable.Collect();
            }
            else if (other.gameObject.HasLayer(Layer.Obstacle))
            {
                _gameService.EndGame();
            }
        }
    }
}
