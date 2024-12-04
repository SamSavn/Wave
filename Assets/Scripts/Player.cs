using UnityEngine;
using Wave.States.PlayerStates;

namespace Wave.Actors
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _modelPrefab;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _force = 10f;
        [SerializeField] private float _maxAngle = 50f;

        private GameObject _model;
        private Collider _collider;

        private IPlayerState _currentState;

        private float _currentAngle;

        private void Awake()
        {
            _model = Instantiate(_modelPrefab, Vector3.zero, Quaternion.identity, transform);
            _collider = _model.GetComponent<Collider>();
        }

        private void Start()
        {
            SetState(new IdleState(transform, _rigidbody));
        }

        private void Update()
        {
            if (_currentState != null)
                _currentState.Execute();

            if (Input.GetKey(KeyCode.Space))
                SetState(new RisingState(_rigidbody, _force, AdjustRotation));
            else
                SetState(new FallingState(_rigidbody, AdjustRotation));
        }

        private void AdjustRotation()
        {
            _currentAngle = Mathf.Clamp(-_rigidbody.linearVelocity.y, -_maxAngle, _maxAngle);
            transform.eulerAngles = new Vector3(_currentAngle, 0, 0);
        }

        private void SetState(IPlayerState state)
        {
            if (state == null)
                return;

            if (_currentState != null)
            {
                if (_currentState == state)
                    return;

                _currentState.Exit();
            }

            _currentState = state;
            _currentState.Enter();
        }
    }
}
