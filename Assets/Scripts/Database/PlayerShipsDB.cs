using UnityEngine;
using Wave.Extentions;
using Wave.Settings;

namespace Wave.Database
{
    [CreateAssetMenu(fileName = "PlayerShipsDB", menuName = "Wave/Database/Player Ships")]
    public class PlayerShipsDB : ScriptableObject
    {
        [SerializeField] private int _basePrice = 100;
        [SerializeField] private int _versionPrice = 50;

        [SerializeField] private ShipInfo[] _shipInfo;

        public int BasePrice => _basePrice;
        public int VersionPrice => _versionPrice;

        public GameObject[] GetMainPrefabs()
        {
            GameObject[] prefabs = new GameObject[_shipInfo.Length];

            for (int i = 0; i < _shipInfo.Length; i++)
                prefabs[i] = _shipInfo[i].GetPrefab();

            return prefabs;
        }

        public GameObject GetShipAt(int index) => GetShipStats(index)?.GetPrefab();

        public ShipInfo GetShipStats(int index)
        {
            if (index.IsInCollectionRange(_shipInfo))
                return _shipInfo[index];

            return null;
        }

        public GameObject GetRandomShip() => GetShipAt(Random.Range(0, _shipInfo.Length));
    } 
}
