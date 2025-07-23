using UnityEngine;

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

        private Vector3[] _trailPositions;
        private float[] _yTargets;

        private int _currentPoints = 0;
        private float _buildTimer = 0f;
        private bool _isFadingIn = false;
        private bool _isFadingOut = false;
        private float _fadeAlpha = 0f;

        private float SegmentSpacing => _trailLength / (_maxTrailPoints - 1);

        private void Reset()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            _trailPositions = new Vector3[_maxTrailPoints];
            _yTargets = new float[_maxTrailPoints];
            _lineRenderer.positionCount = 0;

            _isFadingIn = true;
            _isFadingOut = false;
        }

        public void TriggerFadeOut()
        {
            _isFadingOut = true;
            _isFadingIn = false;
        }

        private void Update()
        {
            Vector3 pos = transform.position;

            if (_isFadingIn && _currentPoints < _maxTrailPoints)
            {
                _buildTimer += Time.deltaTime;
                float timePerPoint = _fadeSpeed / _maxTrailPoints;

                if (_buildTimer >= timePerPoint)
                {
                    _buildTimer -= timePerPoint;
                    _currentPoints++;
                    _lineRenderer.positionCount = _currentPoints;
                }
            }

            for (int i = 0; i < _currentPoints; i++)
            {
                if (i == 0)
                    _yTargets[i] = pos.y;
                else
                    _yTargets[i] = Mathf.Lerp(_yTargets[i], _yTargets[i - 1], Time.deltaTime * (_maxTrailPoints / _fadeSpeed));

                Vector3 localOffset = -transform.forward * SegmentSpacing * i;
                Vector3 rotatedPosition = transform.position + localOffset;

                rotatedPosition.y = _yTargets[i];
                _trailPositions[i] = rotatedPosition;

            }

            _lineRenderer.SetPositions(_trailPositions);

            // Fade logic
            if (_isFadingIn && _fadeAlpha < 1f)
                _fadeAlpha = Mathf.MoveTowards(_fadeAlpha, 1f, Time.deltaTime / _fadeSpeed);
            else if (_isFadingOut)
                _fadeAlpha = Mathf.MoveTowards(_fadeAlpha, 0f, Time.deltaTime / _fadeSpeed);
        }
    }
}
