using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Button))]
    public class TextButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _label;

        [Space]
        [SerializeField][Range(0, 1f)] private float _normalAlpha;
        [SerializeField][Range(0, 1f)] private float _disabledAlpha;

        [Space]
        [SerializeField] private bool _resizeWithText;
        [SerializeField] private bool _useButtonTransitions;

        private ResizingLabel _resizingLabel;

        public Button.ButtonClickedEvent OnClick => _button.onClick;

        public bool Interactable
        {
            get => _button.interactable;
            set
            {
                _button.interactable = value;
                _canvasGroup.interactable = value;
                _canvasGroup.alpha = value ? _normalAlpha : _disabledAlpha;
            }
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            _label = GetComponentInChildren<TMP_Text>();

            _normalAlpha = 1f;
            _disabledAlpha = .5f;
        }

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (_button == null)
                _button = GetComponent<Button>();

            if (_resizeWithText && !_label.TryGetComponent(out _resizingLabel))
                _resizingLabel = _label.AddComponent<ResizingLabel>();

            if (!_useButtonTransitions)
                _button.transition = Selectable.Transition.None;
        }

        private void Start()
        {
            TryResize();
        }

        public void TryResize()
        {
            if (!_resizeWithText)
                return;

            float offset = _rectTransform.rect.width - _resizingLabel.Width;
            float targetWidth = _resizingLabel.Width + offset;

            _resizingLabel.Resize();
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(targetWidth, 1, targetWidth));
        }

        public void SetText(string value)
        {
            _label.text = value;
            TryResize();
        }
    } 
}
