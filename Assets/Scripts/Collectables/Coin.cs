using UnityEngine;
using Wave.Services;

namespace Wave.Collectables
{
    public class Coin : MonoBehaviour, ICollectable
    {
        public void Collect()
        {
            SetActive(false);
            ServiceLocator.Instance.Get<PlayerService>().AddCoins(1);
        }

        public void SetActive(bool value) => gameObject.SetActive(value);
    }
}
