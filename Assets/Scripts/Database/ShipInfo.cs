using System;
using UnityEngine;
using UnityEngine.Serialization;
using Wave.Extentions;

namespace Wave.Settings
{
	[CreateAssetMenu(fileName = "ShipStats", menuName = "Wave/Settings/Ship Stats")]
	public class ShipInfo : ScriptableObject
	{
		[SerializeField] private string _name;
        [SerializeField] private Vector3 _trailOrigin = Vector3.zero;
        [SerializeField] private ShipVersion _info;
		[SerializeField][FormerlySerializedAs("_variants")] private ShipVersion[] _versions;

		public string GetName() => _name;
        public Vector3 GetTrailOrigin() => _trailOrigin;
        public GameObject GetPrefab() => _info.GetPrefab();
		public ShipVersion GetMainVersion() => _info;
		public ShipVersion[] GetVersions() => _versions;
		public bool HasVersions() => !_versions.IsNullOrEmpty();

		public (Color primary, Color secondary) GetColors()
		{
			return new()
			{
				primary = _info.GetPrimaryColor(),
				secondary = _info.GetSecondaryColor(),
			};
        }

        public bool TryGetVariantColors(int index, out (Color primary, Color secondary) colors)
        {
			colors = default;

			if (!index.IsInCollectionRange(_versions))
				return false;

            colors = new()
            {
                primary = _info.GetPrimaryColor(),
                secondary = _info.GetSecondaryColor(),
            };

			return true;
        }
	}

    [Serializable]
    public class ShipVersion
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Color _primaryColor;
        [SerializeField] private Color _secondaryColor;

        public GameObject GetPrefab() => _prefab;
        public Color GetPrimaryColor() => _primaryColor;
        public Color GetSecondaryColor() => _secondaryColor;
    }
}
