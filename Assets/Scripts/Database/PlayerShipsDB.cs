using System;
using UnityEngine;
using Wave.Extentions;
using Wave.Settings;

namespace Wave.Database
{
    [CreateAssetMenu(fileName = "PlayerShipsDB", menuName = "Wave/Database/Player Ships")]
    public class PlayerShipsDB : ScriptableObject
    {
        [SerializeField] private ShipInfo[] _shipStats;
        private GameObject[] _allPrfabs = Array.Empty<GameObject>();

        public GameObject[] GetAllPrefabs()
        {
            if (_allPrfabs.IsNullOrEmpty())
            {
                _allPrfabs = new GameObject[_shipStats.Length];
                int count = _allPrfabs.Length;

                for (int i = 0; i < count; i++)
                    _allPrfabs[i] = _shipStats[i].GetPrefab();
            }

            return _allPrfabs;
        }

        public GameObject GetShipAt(int index) => GetShipStats(index)?.GetPrefab();

        public ShipInfo GetShipStats(int index)
        {
            if (index.IsInCollectionRange(_shipStats))
                return _shipStats[index];

            return null;
        }

        public GameObject GetRandomShip() => GetShipAt(UnityEngine.Random.Range(0, _shipStats.Length));
    } 
}
