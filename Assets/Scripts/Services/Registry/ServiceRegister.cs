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
            UiService uiService = new UiService(coroutineService);
            SceneService sceneService = new SceneService();

            ServiceLocator.Instance.Register(updateService);
            ServiceLocator.Instance.Register(coroutineService);
            ServiceLocator.Instance.Register(addressablesService);
            ServiceLocator.Instance.Register(dataService);
            ServiceLocator.Instance.Register(assetsService);
            ServiceLocator.Instance.Register(sceneService);
            ServiceLocator.Instance.Register(uiService);

            GameService gameService = new GameService(updateService, uiService, sceneService);
            ServiceLocator.Instance.Register(gameService);

            PlayerService playerService = new PlayerService(dataService, gameService);
            ServiceLocator.Instance.Register(playerService);

            ServiceLocator.Instance.Register(new InputService(coroutineService));
            ServiceLocator.Instance.Register(new ShipsService(playerService, assetsService));

            ServiceLocator.Instance.SetAsReady();
        }
    }
}
