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

        public void SetValue(string value)
		{
            if (_label != null)
                _label.text = value;

            if (_rectTransform != null)
			    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(_label.preferredWidth, 1, _label.preferredWidth));
		}

		public void SetValue(float value) => SetValue(value.ToString());
	} 
}
