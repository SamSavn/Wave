using UnityEngine;

namespace Wave.CustomEditors
{
    public class SetAnchorsToCorners : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
        Destroy(this);        
#endif
        }

#if UNITY_EDITOR

        public void Process()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                SetAnchors(rectTransform, transform.parent.GetComponent<RectTransform>());
            }
        }

        public void ProcessChildren()
        {
            ProcessChildren(transform);
        }

        private void SetAnchors(RectTransform rectTransform, RectTransform parentTransform)
        {
            if (rectTransform == null || parentTransform == null)
            {
                return;
            }

            Vector2 newAnchorsMin = new Vector2(rectTransform.anchorMin.x + rectTransform.offsetMin.x / parentTransform.rect.width,
                                             rectTransform.anchorMin.y + rectTransform.offsetMin.y / parentTransform.rect.height);
            Vector2 newAnchorsMax = new Vector2(rectTransform.anchorMax.x + rectTransform.offsetMax.x / parentTransform.rect.width,
                                             rectTransform.anchorMax.y + rectTransform.offsetMax.y / parentTransform.rect.height);

            rectTransform.anchorMin = newAnchorsMin;
            rectTransform.anchorMax = newAnchorsMax;
            rectTransform.offsetMin = rectTransform.offsetMax = new Vector2(0, 0);

        }

        private void ProcessChildren(Transform parent)
        {
            RectTransform parentRect = parent.GetComponent<RectTransform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                RectTransform childRectTransform = parent.GetChild(i).GetComponent<RectTransform>();

                if (childRectTransform != null)
                {
                    SetAnchors(childRectTransform, parentRect);
                }

                // Recursively process the children of this child
                ProcessChildren(parent.GetChild(i));
            }
        }
#endif
    } 
}
