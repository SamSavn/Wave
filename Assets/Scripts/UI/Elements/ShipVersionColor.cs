using UnityEngine;
using UnityEngine.UI;
using Wave.Data;
using Wave.Events;
using Wave.Services;

namespace Wave.UI
{
    public class ShipVersionColor : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _colorsContainer;

        [Space]
        [SerializeField] private Image _primaryColor;
        [SerializeField] private Image _secondaryColor;

        [Space]
        [SerializeField] private GameObject _selection;
        [SerializeField] private GameObject _equippedIndicator;
        [SerializeField] private Image _lockIcon;

        private UiService _uiService;

        public int Index { get; private set; }

        public bool Selected
        {
            get => _selection.activeSelf;
            private set => _selection.SetActive(value);
        }

        public bool Equipped
        {
            get => _equippedIndicator.activeSelf;
            private set => _equippedIndicator.SetActive(value);
        }

        public bool Locked
        {
            get => _lockIcon.enabled && _colorsContainer.alpha < 1f;
            private set
            {
                _lockIcon.enabled = value;
                _colorsContainer.alpha = value ? 0.5f : 1f;
            }
        }

        public EventDisparcher<ShipVersionColor> OnClick { get; } = new EventDisparcher<ShipVersionColor>();

        private void Reset()
        {
            _button = GetComponent<Button>();
            _colorsContainer = GetComponentInChildren<CanvasGroup>();
        }

        private void Awake()
        {
            _button ??= GetComponent<Button>();
            _uiService = ServiceLocator.Instance.Get<UiService>();
        }

        private void OnEnable()
        {
            _uiService.RegisterButton(_button, OnButtonClick);            
        }

        private void OnDisable()
        {
            _uiService.UnregisterButton(_button);
        }

        public void SetUp(ColorVersionData data)
        {
            Index = data.index;
            _primaryColor.color = data.version.GetPrimaryColor();
            _secondaryColor.color = data.version.GetSecondaryColor();

            _colorsContainer.alpha = data.unlocked ? 1f : 0.5f;
            _lockIcon.enabled = !data.unlocked;

            Lock(!data.unlocked);
            Select(false);
            Equip(false);
        }

        public void Select(bool value) => Selected = value;
        public void Equip(bool value) => Equipped = value;
        public void Lock(bool value) => Locked = value;

        private void OnButtonClick()
        {
            OnClick?.Invoke(this);
        }
    }
}

