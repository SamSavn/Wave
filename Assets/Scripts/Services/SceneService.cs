using UnityEngine;
using Wave.Handlers;

namespace Wave.Services
{
    public class SceneService : IService
    {
        private SceneHandler _sceneHandler;

        public void SetSceneHandler(SceneHandler sceneHandler) => _sceneHandler = sceneHandler;

        public void SetScene(SceneType sceneType)
        {
            if (_sceneHandler == null)
            {
                Debug.LogError($"Unable to set scene to {sceneType}: No scene handler has been set. <b>Use {nameof(SetSceneHandler)} method to do so</b>");
                return;
            }

            _sceneHandler.SetScene(sceneType);
        }
    }
}
