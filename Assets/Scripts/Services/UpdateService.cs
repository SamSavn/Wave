using UnityEngine;
using Wave.Events;

namespace Wave.Services
{
    public class UpdateService : IService
    {
        private class UpdateServiceBehaviour : MonoBehaviour
        {
            public UpdateService service;

            private void Update()
            {
                if (service == null)
                {
                    GameObject.Destroy(gameObject);
                    return;
                }

                service.Update?.Invoke(Time.deltaTime);
            }
        }

        public EventDisparcher<float> Update { get; } = new EventDisparcher<float>();

        public UpdateService()
        {
            GameObject updateGo = new GameObject("[Update Service]");
            UpdateServiceBehaviour behaviour = updateGo.AddComponent<UpdateServiceBehaviour>();
            behaviour.service = this;
            GameObject.DontDestroyOnLoad(updateGo);
        }
    } 
}
