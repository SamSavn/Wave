using DG.Tweening;
using UnityEngine;

namespace Wave.Services
{
    public static class ServiceRegister
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            DOTween.Init(recycleAllByDefault: true, useSafeMode: true, LogBehaviour.ErrorsOnly).SetCapacity(200, 50);

            UpdateService updateService = new UpdateService();
            CoroutineService coroutineService = new CoroutineService();
            AddressablesService addressablesService = new AddressablesService(coroutineService);

            ServiceLocator.Instance.Register(updateService);
            ServiceLocator.Instance.Register(coroutineService);
            ServiceLocator.Instance.Register(addressablesService);

            ServiceLocator.Instance.Register(new InputService(updateService));
            ServiceLocator.Instance.Register(new PrefabsService(addressablesService));
            ServiceLocator.Instance.Register(new GameService(updateService));
        }
    }
}
