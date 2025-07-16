using Wave.Services;
using Wave.Settings;
using System.Collections.Generic;
using UnityEngine;
using Wave.Events;
using Wave.Extentions;
using Wave.Data;

namespace Wave.UI
{
    public class ShipsVersionsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private GameObject _colorPrefab;
        private List<ShipVersionColor> _pool = new();
        private ShipVersionColor _selectedVersion;
        private ShipVersionColor _equippedVersion;

        public EventDisparcher<int> OnVersionSelectionChanged { get; } = new EventDisparcher<int>();

        private void Awake()
        {
            _colorPrefab = ServiceLocator.Instance.Get<AssetsService>().GetShipVersionColorPrefab();
        }

        public void SetVersions(ColorVersionsSetData data)
        {
            if (data.shipInfo == null || !data.shipInfo.HasVersions())
                return;

            if (_selectedVersion != null)
            {
                _selectedVersion.Select(false);
                _selectedVersion = null;
            }

            AddVersion(new ColorVersionData()
            {
                version = data.shipInfo.GetMainVersion(),
                index = 0,
                selected = data.selectedVersion == 0,
                unlocked = data.shipsService.IsVersionUnlocked(data.shipIndex, 0)
            });

            ShipVersion[] versions = data.shipInfo.GetVersions();
            int count = versions.Length;
            int v;

            for (int i = 0; i < count; i++)
            {
                v = i + 1;

                AddVersion(new ColorVersionData()
                {
                    version = versions[i],
                    index = v,
                    selected = data.selectedVersion == v,
                    unlocked = data.shipsService.IsVersionUnlocked(data.shipIndex, v)
                });
            }

            DeactivateUnusedItems(count + 1);
        }

        private void AddVersion(ColorVersionData data)
        {
            ShipVersionColor versionColor = GetOrCreateVersion(data.index);
            versionColor.SetUp(data);
            versionColor.OnClick?.Add(SelectVersion);
            versionColor.gameObject.SetActive(true);

            if (data.selected)
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

        public void EquipVersion(int index)
        {
            if (!index.IsInCollectionRange(_pool))
            {
                return;
            }

            if (_equippedVersion != null)
            {
                _equippedVersion.Equip(false);
                _equippedVersion = null;
            }

            ShipVersionColor versionColor = _pool[index];
            _equippedVersion = versionColor;
            _equippedVersion.Equip(true);
        }

        public void UnlockVersion(int index)
        {
            if (!index.IsInCollectionRange(_pool))
            {
                Debug.LogError($"Version index {index} is out of range.");
                return;
            }

            ShipVersionColor versionColor = _pool[index];
            versionColor.Lock(false);
        }
    }
}