using Wave.Services;
using Wave.Settings;
using System.Collections.Generic;
using UnityEngine;
using Wave.Events;
using Wave.Extentions;

namespace Wave.UI
{
    public class ShipsVersionsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private GameObject _colorPrefab;
        private List<ShipVersionColor> _pool = new();
        private ShipVersionColor _selectedVersion;

        public EventDisparcher<int> OnVersionSelectionChanged { get; } = new EventDisparcher<int>();

        private void Awake()
        {
            _colorPrefab = ServiceLocator.Instance.Get<AssetsService>().GetShipVersionColorPrefab();
        }

        public void SetVersions(ShipInfo shipInfo, int selectedVersion)
        {
            if (shipInfo == null || !shipInfo.HasVersions())
                return;

            if (_selectedVersion != null)
            {
                _selectedVersion.Select(false);
                _selectedVersion = null;
            }

            ShipVersion[] variants = shipInfo.GetVersions();
            int count = variants.Length;

            AddVersion(shipInfo.GetMainVersion(), 0, selectedVersion);

            for (int i = 0; i < count; i++)
                AddVersion(variants[i], i + 1, selectedVersion);

            DeactivateUnusedItems(count + 1);
        }

        private void AddVersion(ShipVersion version, int index, int selected)
        {
            ShipVersionColor versionColor = GetOrCreateVersion(index);
            versionColor.SetUp(version, index);
            versionColor.OnClick?.Add(SelectVersion);
            versionColor.gameObject.SetActive(true);

            if (index == selected)
                SelectVersion(versionColor);
        }

        private ShipVersionColor GetOrCreateVersion(int index)
        {
            if (index.IsInCollectionRange(_pool))
                return _pool[index];

            GameObject clone = Instantiate(_colorPrefab, _container);

            if (!clone.TryGetComponent(out ShipVersionColor versionColor))
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
            ShipVersionColor versionColor;

            for (int i = activeCount; i < _pool.Count; i++)
            {
                versionColor = _pool[i];
                versionColor.gameObject.SetActive(false);
                versionColor.OnClick.Remove(SelectVersion);
            }
        }

        private void SelectVersion(ShipVersionColor version)
        {
            if (_selectedVersion == version)
                return;

            if (_selectedVersion != null)
                _selectedVersion.Select(false);

            _selectedVersion = version;
            _selectedVersion.Select(true);

            OnVersionSelectionChanged?.Invoke(version.Index);
        }
    }
}