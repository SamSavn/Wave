using System.Collections;
using UnityEngine;

namespace Wave.Collectables
{
    public abstract class Collectable : MonoBehaviour, ICollectable
    {
        [SerializeField] protected ParticleSystem _collectEffect;

        private void Reset()
        {
            _collectEffect = GetComponentInChildren<ParticleSystem>(true);
        }

        public virtual void Collect()
        {
            _collectEffect.Play(true);
        }

        public virtual void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}