using System;
using UnityEngine;

namespace Gameplay {
    /// <summary>
    /// This cover exists only to create and delete GameDI object as necessary.
    /// </summary>
    public class GameDiCover : MonoBehaviour {
        private void Awake() {
            Events.EventsModel.PRE_SCENE_LOAD_BEGIN += OnPreSceneLoadBegin;
            Events.EventsModel.SCENE_LOAD_COMPLETED += OnSceneLoadCompleted;
        }

        private void OnDestroy() {
            Events.EventsModel.PRE_SCENE_LOAD_BEGIN -= OnPreSceneLoadBegin;
            Events.EventsModel.SCENE_LOAD_COMPLETED -= OnSceneLoadCompleted;
        }

        private void OnPreSceneLoadBegin(string sceneName) {
            if (sceneName == GameConstants.Scenes.GAMEPLAY) {
                GameDI.CreateNew();
            }
        }

        private void OnSceneLoadCompleted(string sceneName) {
            if (sceneName == GameConstants.Scenes.GAMEPLAY) {
                GameDI.Delete();
            }
        }
    }
}