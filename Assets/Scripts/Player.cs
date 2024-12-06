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

        private UpdateService _updateService;
        private InputService _inputService;
        private PrefabsService _prefabsService;
        private GameService _gameService;

        private GameObject _modelPrefab;
        private GameObject _model;
        private Collider _collider;

        private StateMachine _stateMachine;
        private float _currentAngle;

        private void Awake()
        {
            _updateService = ServiceLocator.Instance.Get<UpdateService>();
            _inputService = ServiceLocator.Instance.Get<InputService>();
            _prefabsService = ServiceLocator.Instance.Get<PrefabsService>();
            _gameService = ServiceLocator.Instance.Get<GameService>();

            _gameService.SetPlayer(this);
            _stateMachine = new StateMachine();
            _prefabsService.OnShipsLoaded?.Add(OnPrefabsLoaded);
        }

        private void OnDestroy()
        {
            _inputService.OnGameInputDown.Remove(OnInputDown);
            _inputService.OnGameInputUp.Remove(OnInputUp);
            _updateService.Update.Remove(CustomUpdate);
            _prefabsService.OnShipsLoaded.Remove(OnPrefabsLoaded);
        }

        private void CustomUpdate(float dt)
        {
            _stateMachine.Update();
        }

        private void AdjustRotation()
        {
            _currentAngle = Mathf.Clamp(-_rigidbody.linearVelocity.y, -_maxAngle, _maxAngle);
            transform.eulerAngles = new Vector3(_currentAngle, 0, 0);
        }

        public void SetVisible(bool active) => _model.SetActive(active);
        public void ResetState() => _stateMachine.SetState(new IdleState(transform, _rigidbody));
        public void Die() => _stateMachine.SetState(new ExplodingState(this, _explosionParticle));

        private void OnPrefabsLoaded(bool success)
        {
            if (!success) 
                return;

            _modelPrefab = _prefabsService.GetInitialPrefab(PrefabType.PlayerShip);
            _model = Instantiate(_modelPrefab, Vector3.zero, Quaternion.identity, transform);
            _model.gameObject.SetLayer(Layer.Player);

            _collider = _model.GetComponent<Collider>();
            _collider.isTrigger = true;

            _inputService.OnGameInputDown.Add(OnInputDown);
            _inputService.OnGameInputUp.Add(OnInputUp);
            _updateService.Update.Add(CustomUpdate);
        }

        private void OnInputDown()
        {
            if (_stateMachine.CurrentState is ExplodingState)
                return;

            _stateMachine.SetState(new RisingState(_rigidbody, _force, AdjustRotation));
        }

        private void OnInputUp()
        {
            if (_stateMachine.CurrentState is ExplodingState)
                return;

            _stateMachine.SetState(new FallingState(_rigidbody, AdjustRotation));
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
