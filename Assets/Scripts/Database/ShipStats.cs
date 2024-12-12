using System;
using UnityEngine;
using Wave.Extentions;

namespace Wave.Settings
{
	[CreateAssetMenu(fileName = "ShipStats", menuName = "Wave/Settings/Ship Stats")]
	public class ShipStats : ScriptableObject
	{
		[SerializeField] private ShipInfo _info;
		[SerializeField] private ShipInfo[] _variants;
		[SerializeField] private float _mass;
		[SerializeField] private float _power;
		[SerializeField] private float _speed;

		public GameObject GetPrefab() => _info.GetPrefab();
		public ShipInfo[] GetVariants() => _variants;
		public bool HasVariants() => !_variants.IsNullOrEmpty();

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

			if (!index.IsInCollectionRange(_variants))
				return false;

            colors = new()
            {
                primary = _info.GetPrimaryColor(),
                secondary = _info.GetSecondaryColor(),
            };

			return true;
        }

        public float GetMass() => _mass;
		public float GetPower() => _power;
		public float GetSpeed() => _speed;
	}

    [Serializable]
    public class ShipInfo
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Color _primaryColor;
        [SerializeField] private Color _secondaryColor;

        public GameObject GetPrefab() => _prefab;
        public Color GetPrimaryColor() => _primaryColor;
        public Color GetSecondaryColor() => _secondaryColor;
    }
}
