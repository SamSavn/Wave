using UnityEngine;
using Wave.Extentions;

[CreateAssetMenu(fileName = "PlayerShipsDB", menuName = "Wave/Database/Player Ships")]
public class PlayerShipsDB : ScriptableObject
{
    [SerializeField] private GameObject[] _ships;

    public GameObject[] GetAllShips() => _ships;

    public GameObject GetShipAt(int index)
    {
        if (index.IsInCollectionRange(_ships))
            return _ships[index];

        return null;
    }

    public GameObject GetRandomShip() => GetShipAt(Random.Range(0, _ships.Length));
}
