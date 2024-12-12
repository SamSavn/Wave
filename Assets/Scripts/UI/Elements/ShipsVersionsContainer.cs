using Wave.Services;
using Wave.Settings;

namespace Wave.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ShipsVersionsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private List<ShipVersionColor> _pool = new();
        private GameObject _colorPrefab;

        private void Awake()
        {
            _colorPrefab = ServiceLocator.Instance.Get<AssetsService>().GetShipVersionColorPrefab();
        }

        public void SetVersions(ShipStats info)
        {
            if (info == null || !info.HasVariants())
                return;

            ShipInfo[] variants = info.GetVariants();
            (Color primary, Color secondary) colors = default;
            int count = variants.Length;

            for (int i = 0; i < count; i++)
            {
                if (!info.TryGetVariantColors(i, out colors))
                    continue;

                ShipVersionColor version = GetOrCreateVersion();
                version.SetUp(variants[i]);
                version.gameObject.SetActive(true);
            }

            DeactivateUnusedItems(count);
        }

        private ShipVersionColor GetOrCreateVersion()
        {
            foreach (ShipVersionColor version in _pool)
            {
                if (!version.gameObject.activeSelf)
                    return version;
            }

            GameObject versionObject = Instantiate(_colorPrefab, _container);
            ShipVersionColor newVersion = versionObject.GetComponent<ShipVersionColor>();

            if (newVersion == null)
            {
                Debug.LogError("Prefab is missing ShipVersionColor component.");
                Destroy(versionObject);
                return null;
            }

            _pool.Add(newVersion);
            return newVersion;
        }

        private void DeactivateUnusedItems(int activeCount)
        {
            for (int i = activeCount; i < _pool.Count; i++)
                _pool[i].gameObject.SetActive(false);
        }
    }

}
