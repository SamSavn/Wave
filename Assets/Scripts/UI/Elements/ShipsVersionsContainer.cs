using Wave.Services;
using Wave.Settings;

namespace Wave.UI
{
    using System.Collections.Generic;
    using UnityEngine;
    using Wave.Extentions;

    public class ShipsVersionsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private List<ShipVersionColor> _pool = new();
        private GameObject _colorPrefab;

        private void Awake()
        {
            _colorPrefab = ServiceLocator.Instance.Get<AssetsService>().GetShipVersionColorPrefab();
        }

        public void SetVersions(ShipInfo shipStats)
        {
            if (shipStats == null || !shipStats.HasVariants())
                return;

            ShipVersion[] variants = shipStats.GetVariants();
            int count = variants.Length;

            AddVersion(shipStats.GetMainVersion(), 0);

            for (int i = 0; i < count; i++)
                AddVersion(variants[i], i + 1);

            DeactivateUnusedItems(count);
        }

        private void AddVersion(ShipVersion version, int index)
        {
            ShipVersionColor versionColors = GetOrCreateVersion(index);
            versionColors.SetUp(version);
            versionColors.gameObject.SetActive(true);
        }

        private ShipVersionColor GetOrCreateVersion(int index)
        {
            if (index.IsInCollectionRange(_pool))
                return _pool[index];

            GameObject clone = Instantiate(_colorPrefab, _container);
            ShipVersionColor versionColor = clone.GetComponent<ShipVersionColor>();

            if (versionColor == null)
            {
                Debug.LogError("Prefab is missing ShipVersionColor component.");
                Destroy(clone);
                return null;
            }

            _pool.Add(versionColor);
            return versionColor;
        }

        private void DeactivateUnusedItems(int activeCount)
        {
            for (int i = activeCount; i < _pool.Count; i++)
                _pool[i].gameObject.SetActive(false);
        }
    }

}
