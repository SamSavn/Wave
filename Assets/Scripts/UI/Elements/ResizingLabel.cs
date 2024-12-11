using TMPro;
using UnityEngine;

namespace Wave.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TMP_Text))]
	public class ResizingLabel : MonoBehaviour
	{
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private TMP_Text _label;
        [SerializeField] private float _offset;
        [SerializeField] private bool _resizeOnAwake;

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
			_label = GetComponent<TMP_Text>();
        }

        private void Awake()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (_label == null)
                _label = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            if (_resizeOnAwake)
                Resize();
        }

        public void SetValue(string value)
		{
            if (_label != null)
                _label.text = value;

            Resize();
		}

		public void SetValue(float value) => SetValue(value.ToString());

        public void Resize()
        {
            float targetWidth = _label.preferredWidth + _offset;

            if (_rectTransform != null)
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(targetWidth, 1, targetWidth));
        }
	} 
}
