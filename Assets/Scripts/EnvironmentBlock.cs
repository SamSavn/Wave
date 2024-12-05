using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wave.Environment
{
    public class EnvironmentBlock : MonoBehaviour
    {
        [SerializeField] private Transform _leftSocket;
        [SerializeField] private Transform _rightSocket;
        [SerializeField] private bool _initialBlock;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public float Width => Vector3.Distance(_leftSocket.position, _rightSocket.position);
        public bool IsInitial => _initialBlock;

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

        public void SetActive(bool active) => gameObject.SetActive(active);
    } 
}
