using UnityEngine;

namespace Wave.Extentions
{
    public static class ObjectsExtentions
    {
        public static void SetLayer(this GameObject obj, Layer layer, bool includeChildren = true)
        {
            if (obj == null)
                return;

            obj.layer = (int)layer;

            if (includeChildren && obj.transform.childCount > 0)
            {
                Transform transform = obj.transform;
                int count = transform.childCount;

                for (int i = 0; i < count; i++)
                {
                    transform.GetChild(i).gameObject.SetLayer(layer, includeChildren);
                }
            }
        }

        public static bool HasLayer(this GameObject obj, Layer layer)
        {
            return obj != null && obj.layer == (int)layer;
        }

        public static bool HasLayer(this GameObject obj, LayerMask layerMask)
        {
            return obj != null && ((1 << obj.layer) & layerMask.value) != 0;
        }

        public static bool HasLayerAndComponent<T>(this GameObject obj, Layer layer, out T component) where T : class
        {
            component = null;

            if (!obj.HasLayer(layer))
                return false;

            if (!obj.TryGetComponent(out component)) 
                return false;

            return true;
        }

        public static void SwapMesh(this GameObject obj, GameObject other, bool applyScale = false)
        {
            if (obj == null || other == null)
            {
                Debug.LogError("SwapMesh failed: One of the objects is null.");
                return;
            }

            if (!obj.TryGetComponent(out MeshFilter to) ||
                !other.TryGetComponent(out MeshFilter from))
            {
                Debug.LogError("SwapMesh failed: One of the objects does not have a MeshFilter component.");
                return;
            }

            if (from.sharedMesh == null)
            {
                Debug.LogError("SwapMesh failed: The source object does not have a shared mesh.");
                return;
            }

            to.mesh = from.sharedMesh;

            if (applyScale)
                to.transform.localScale = from.transform.lossyScale;

            if (to.TryGetComponent(out MeshCollider collider))
                collider.sharedMesh = from.sharedMesh;
        }
    }
}
