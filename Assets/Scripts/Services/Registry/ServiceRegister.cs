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
            UiService uiService = new UiService();
            SceneService sceneService = new SceneService();

            ServiceLocator.Instance.Register(updateService);
            ServiceLocator.Instance.Register(coroutineService);
            ServiceLocator.Instance.Register(addressablesService);
            ServiceLocator.Instance.Register(dataService);
            ServiceLocator.Instance.Register(uiService);
            ServiceLocator.Instance.Register(sceneService);

            GameService gameService = new GameService(updateService, uiService, sceneService);
            ServiceLocator.Instance.Register(gameService);

            ServiceLocator.Instance.Register(new InputService(coroutineService));
            ServiceLocator.Instance.Register(new PrefabsService(addressablesService));
            ServiceLocator.Instance.Register(new ShipsService(dataService));
            ServiceLocator.Instance.Register(new PlayerService(dataService, gameService));

            ServiceLocator.Instance.SetAsReady();
        }
    }
}
