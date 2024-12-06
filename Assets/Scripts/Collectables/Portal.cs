using UnityEngine;

namespace Wave.Collectables
{
    public class Portal : MonoBehaviour, ICollectable
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Color _activeColor;

        private Color _defaultColor;

        private void Reset()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
        }

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<MeshRenderer>();

            _defaultColor = _renderer.sharedMaterial.color;
        }

        public void Collect()
        {
            _renderer.material.color = _activeColor;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);

            if (!value)
                _renderer.material.color = _defaultColor;
        }
    } 
}
