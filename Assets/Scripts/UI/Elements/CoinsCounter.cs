using UnityEngine;
using Wave.Services;

namespace Wave.UI
{
    public class CoinsCounter : MonoBehaviour
    {
        [SerializeField] ResizingLabel _coinsLabel;

        private PlayerService _playerService;

        private void Reset()
        {
            _coinsLabel = GetComponentInChildren<ResizingLabel>();
        }

        private void Awake()
        {
            ServiceLocator.Instance.Get<UiService>().SetCoinsCounter(this);

            _playerService = ServiceLocator.Instance.Get<PlayerService>();
            _playerService.OnCoinsChanged?.Add(SetValue);
        }

        private void OnEnable()
        {
            SetValue(_playerService.GetCoins());
        }

        private void SetValue(int value)
        {
            SetActive(true);
            _coinsLabel.SetValue(value);
        }

        public void SetActive(bool active) => gameObject.SetActive(active);
    } 
}
