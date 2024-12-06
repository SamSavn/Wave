using UnityEngine;

namespace Wave.Extentions
{
    public static class ObjectsExtentions
    {
        public static bool HasLayer(this GameObject obj, Layer layer)
        {
            return obj != null && obj.layer == (int)layer;
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
    }
}
