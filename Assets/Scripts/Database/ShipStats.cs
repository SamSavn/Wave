using UnityEngine;

namespace Wave.Settings
{
	[CreateAssetMenu(fileName = "ShipStats", menuName = "Wave/Settings/Ship Stats")]
	public class ShipStats : ScriptableObject
	{
		[SerializeField] private GameObject _prefab;
		[SerializeField] private float _mass;
		[SerializeField] private float _power;
		[SerializeField] private float _speed;

		public GameObject GetPrefab() => _prefab;
		public float GetMass() => _mass;
		public float GetPower() => _power;
		public float GetSpeed() => _speed;
	} 
}
