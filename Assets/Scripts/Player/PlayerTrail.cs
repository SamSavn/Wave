using System.Collections;
using UnityEngine;
using Wave.Services;

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
        [SerializeField] private float _fadeSpeed = 1f;
        [SerializeField] private float _smoothTime = 1f;

        private UpdateService _updateService;
        private CoroutineService _coroutineService;
        private WaitForSeconds _waitForFade;

        private Vector3[] _trailPositions;
        private float[] _yTargets;

        private Vector3 Position => transform.position;
        private float SegmentSpacing => _trailLength / (_maxTrailPoints - 1);
        private bool IsWhole => CurrentPoints == _maxTrailPoints;

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
            _coroutineService = ServiceLocator.Instance.Get<CoroutineService>();
            _waitForFade = new WaitForSeconds(_fadeSpeed / _maxTrailPoints);
        }

        private void OnEnable()
        {
            _trailPositions = new Vector3[_maxTrailPoints];
            _yTargets = new float[_maxTrailPoints];
            _lineRenderer.positionCount = 0;
        }

        public void Show()
        {
            _coroutineService.StartCoroutine(ShowTrail());

            IEnumerator ShowTrail()
            {
                while (!IsWhole)
                {
                    CurrentPoints++;
                    yield return _waitForFade;
                }
            }

            _updateService.LateUpdate.Add(UpdateTrail);
        }

        public void Hide()
        {
            _updateService.LateUpdate.Remove(UpdateTrail);
            CurrentPoints = 0;
        }

        private void UpdateTrail(float dt)
        {
            if (!IsWhole)
                return;

            for (int i = 0; i < CurrentPoints; i++)
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

            _lineRenderer.SetPositions(_trailPositions);
        }
    }
}
