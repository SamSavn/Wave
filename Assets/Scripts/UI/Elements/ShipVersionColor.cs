using UnityEngine;
using UnityEngine.UI;
using Wave.Events;
using Wave.Settings;

namespace Wave.UI
{
    public class ShipVersionColor : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _primaryColor;
        [SerializeField] private Image _secondaryColor;
        [SerializeField] private Image _selection;

        public int Index { get; private set; }
        public bool Selected
        {
            get => _selection.enabled;
            set => _selection.enabled = value;
        }

        public EventDisparcher<ShipVersionColor> OnClick { get; } = new EventDisparcher<ShipVersionColor>();

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(OnButtonClick);
        }

        public void SetUp(ShipVersion ship, int index)
        {
            Index = index;
            _primaryColor.color = ship.GetPrimaryColor();
            _secondaryColor.color = ship.GetSecondaryColor();
            Select(false);
        }

        public void Select(bool value) => Selected = value;

        private void OnButtonClick()
        {
            OnClick?.Invoke(this);
        }
    }
}

