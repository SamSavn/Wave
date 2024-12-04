using UnityEngine;

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
        private float _currentAngle;

        private void Awake()
        {
            _model = Instantiate(_modelPrefab, Vector3.zero, Quaternion.identity, transform);
            _collider = _model.GetComponent<Collider>();
        }

        private void Start()
        {
            _rigidbody.isKinematic = true;
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            if (!Input.GetKey(KeyCode.Space))
                return;

            if (_rigidbody.isKinematic)
                _rigidbody.isKinematic = false;

            ApplyUpwardForce();
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Space) || _rigidbody.isKinematic)
                return;

            ApplyGravity();
        }

        private void ApplyUpwardForce()
        {
            _rigidbody.AddForce(Vector3.up * _force, ForceMode.Force);
            AdjustRotation();
        }

        private void ApplyGravity()
        {
            _rigidbody.linearVelocity += Physics.gravity * Time.fixedDeltaTime * 2;
            AdjustRotation();
        }

        private void AdjustRotation()
        {
            _currentAngle = Mathf.Clamp(-_rigidbody.linearVelocity.y, -_maxAngle, _maxAngle);
            transform.eulerAngles = new Vector3(_currentAngle, 0, 0);
        }
    }
}
