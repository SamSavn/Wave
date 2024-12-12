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
            DataService dataService = new DataService();
            AssetsService assetsService = new AssetsService(addressablesService);

            ServiceLocator.Instance.Register(updateService);
            ServiceLocator.Instance.Register(coroutineService);
            ServiceLocator.Instance.Register(addressablesService);
            ServiceLocator.Instance.Register(dataService);
            ServiceLocator.Instance.Register(assetsService);

            GameService gameService = new GameService(updateService);
            ServiceLocator.Instance.Register(gameService);

            ServiceLocator.Instance.Register(new UiService());
            ServiceLocator.Instance.Register(new SceneService());

            ServiceLocator.Instance.Register(new InputService(coroutineService));
            ServiceLocator.Instance.Register(new ShipsService(dataService, assetsService));
            ServiceLocator.Instance.Register(new PlayerService(dataService, gameService));

            ServiceLocator.Instance.SetAsReady();
        }
    }
}
