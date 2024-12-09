using TMPro;
using UnityEngine;

namespace Wave.UI
{
	public class ResizingLabel : MonoBehaviour
	{
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private TMP_Text _label;

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
			_label = GetComponent<TMP_Text>();
        }

        public void SetValue(string value)
		{
			_label.text = value;
			_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(_label.preferredWidth, 1, _label.preferredWidth));
		}

		public void SetValue(float value) => SetValue(value.ToString());
	} 
}
