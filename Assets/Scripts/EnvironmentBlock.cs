using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Wave.Collectables;
using Wave.Extentions;

namespace Wave.Environment
{
    public class EnvironmentBlock : MonoBehaviour
    {
        [SerializeField] private Transform _leftSocket;
        [SerializeField] private Transform _rightSocket;
        [SerializeField] private bool _initialBlock;

        private ICollectable[] _collectables = Array.Empty<ICollectable>();

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public float Width => Vector3.Distance(_leftSocket.position, _rightSocket.position);
        public bool IsInitial => _initialBlock;

        private void Awake()
        {
            _collectables = GetComponentsInChildren<ICollectable>();
        }

        public void SetActive(bool active)
        {
            if (active) SetCollectibles();
            else ResetCollectibles();

            gameObject.SetActive(active);
        }

        public void Place(int index)
        {
            Position = new Vector3(0, 0, index * Width);
            SetActive(true);
        }

        public void Move(float speed)
        {
            Position += Vector3.back * speed * Time.deltaTime;
        }

        public void Recycle(EnvironmentBlock lastBlock)
        {
            Position = new Vector3(0, 0, lastBlock.Position.z + Width);
            SetActive(true);
        }

        private void SetCollectibles()
        {
            if (_collectables.IsNullOrEmpty()) 
            {
                Debug.LogWarning($"No collectibles found in block {gameObject.name}");
                return; 
            }

            ResetCollectibles();

            ICollectable collectable;
            int rand = UnityEngine.Random.Range(1, _collectables.Length);
            int activeCount = 0;

            while (activeCount < rand)
            {
                collectable = _collectables.GetRandom();

                if (collectable != null)
                    collectable.SetActive(true);

                activeCount++;
            }
        }

        private void ResetCollectibles()
        {
            _collectables.Foreach(collectable => collectable.SetActive(false));
        }
    } 
}
