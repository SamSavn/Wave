using UnityEngine;
using Wave.Extentions;

namespace Wave.Database
{
    [CreateAssetMenu(fileName = "EnvironmentBlocksDB", menuName = "Wave/Database/Environment Blocks")]
    public class EnvironmentBlocksDB : ScriptableObject
    {
        [SerializeField] private GameObject _startingBlock;
        [SerializeField] private GameObject[] _blocks;

        public GameObject GetInitialBlock() => _startingBlock;
        public GameObject[] GetAllBlocks() => _blocks;

        public GameObject GetBlockAt(int index)
        {
            if (index.IsInCollectionRange(_blocks))
                return _blocks[index];

            return null;
        }

        public GameObject GetRandomBlock() => GetBlockAt(Random.Range(0, _blocks.Length));
    } 
}
