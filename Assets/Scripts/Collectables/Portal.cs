using UnityEngine;
using Wave.Services;

namespace Wave.Collectables
{
    public class Portal : MonoBehaviour, ICollectable
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Color _activeColor;
        [SerializeField] [Range(1, 100)] private int _points = 1;

        private PlayerService _playerService;
        private Color _defaultColor;

        private void Reset()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
        }

        private void Awake()
        {
            _playerService = ServiceLocator.Instance.Get<PlayerService>();

            if (_renderer == null)
                _renderer = GetComponentInChildren<MeshRenderer>();

            _defaultColor = _renderer.sharedMaterial.color;
        }

        public void Collect()
        {
            _renderer.material.color = _activeColor;
            _playerService.AddScore(_points);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);

            if (!value)
                _renderer.material.color = _defaultColor;
        }
    } 
}
