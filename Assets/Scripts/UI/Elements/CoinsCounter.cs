using TMPro;
using UnityEngine;
using Wave.Services;

namespace Wave.UI
{
    public class CoinsCounter : MonoBehaviour
    {
        [SerializeField] TMP_Text _coinsLabel;
        private PlayerService _playerService;

        private void Reset()
        {
            _coinsLabel = GetComponentInChildren<TMP_Text>();
        }

        private void Awake()
        {
            ServiceLocator.Instance.Get<UiService>().SetCoinsCounter(this);
            _playerService = ServiceLocator.Instance.Get<PlayerService>();
        }

        private void OnEnable()
        {
            SetValue(_playerService.GetCoins());
            _playerService.OnCoinsChanged?.Add(SetValue);
        }

        private void OnDisable()
        {
            _playerService.OnCoinsChanged?.Remove(SetValue);            
        }

        private void SetValue(int value)
        {
            SetActive(true);

            if (_coinsLabel != null)
                _coinsLabel.text = value.ToString();
        }

        public void SetActive(bool active) => gameObject.SetActive(active);
    } 
}
