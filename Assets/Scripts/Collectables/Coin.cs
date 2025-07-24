using UnityEngine;
using Wave.Services;

namespace Wave.Collectables
{
    public class Coin : Collectable
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField][Range(1, 10)] private int _value;

        private void Reset()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _value = 1;
        }

        public override void Collect()
        {
            base.Collect();
            _meshRenderer.enabled = false;
            ServiceLocator.Instance.Get<PlayerService>().AddCoins(_value);
        }

        public override void SetActive(bool value)
        {
            base.SetActive(value);

            if (value)
                _meshRenderer.enabled = true;
        }
    }
}
