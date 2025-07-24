using UnityEngine;
using Wave.Services;
using static UnityEngine.ParticleSystem;

namespace Wave.Actors.Effects
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlayerTrail : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("Settings")]
        [SerializeField] private int _maxTrailPoints = 5;
        [SerializeField] private float _trailLength = 2f;
        [SerializeField] private float _smoothTime = 1f;

        private UpdateService _updateService;

        private Vector3[] _trailPositions;
        private float[] _yTargets;

        private Vector3 Position => transform.position;
        private float SegmentSpacing => _trailLength / (_maxTrailPoints - 1);
        private bool IsVisible => CurrentPoints == _maxTrailPoints;

        private int CurrentPoints
        {
            get => _lineRenderer.positionCount;
            set => _lineRenderer.positionCount = Mathf.Clamp(value, 0, _maxTrailPoints);
        }

        private void Reset()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Awake()
        {
            _updateService = ServiceLocator.Instance.Get<UpdateService>();
        }

        private void OnEnable()
        {
            ResetPoints();
        }

        public void Initialize(GameObject model, Vector3 origin)
        {
            if (origin == Vector3.zero)
            {
                MeshFilter meshFilter = model.GetComponent<MeshFilter>();
                Vector3 center = meshFilter.mesh.bounds.center;
                Vector3 size = meshFilter.mesh.bounds.size;
                float zOffset = size.z * .5f;
                float yOffset = size.y / 3f;

                origin = new Vector3(center.x, center.y - yOffset, center.z - zOffset);
            }                

            transform.localPosition = origin;
            Hide();
        }

        public void Show()
        {
            CurrentPoints = _maxTrailPoints;
            _updateService.LateUpdate.Add(UpdateTrail);
        }

        public void Hide()
        {
            _updateService.LateUpdate.Remove(UpdateTrail);
            ResetPoints();
        }

        private void UpdateTrail(float dt)
        {
            if (!IsVisible)
                return;

            for (int i = 0; i < CurrentPoints; i++)
                UpdatePoint(i, dt);

            _lineRenderer.SetPositions(_trailPositions);
        }

        private void UpdatePoint(int i, float dt)
        {
            if (i == 0)
                _yTargets[i] = Position.y;
            else
                _yTargets[i] = Mathf.Lerp(_yTargets[i], _yTargets[i - 1], dt * (_maxTrailPoints / _smoothTime));

            Vector3 localOffset = -transform.forward * SegmentSpacing * i;
            Vector3 rotatedPosition = Position + localOffset;

            rotatedPosition.y = _yTargets[i];
            _trailPositions[i] = rotatedPosition;
        }

        private void ResetPoints()
        {
            _trailPositions = new Vector3[_maxTrailPoints];
            _yTargets = new float[_maxTrailPoints];
            _lineRenderer.positionCount = 0;
        }
    }
}
