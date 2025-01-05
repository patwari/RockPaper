using System.Collections;
using UnityEngine;
using Events;

namespace Utils {

    public class GoToLobby : MonoBehaviour {
        // Start is called before the first frame update
        private void Start() {
            // Load the lobby scene
            StartCoroutine(LoadLobby());
        }

        private IEnumerator LoadLobby() {
            yield return new WaitForSeconds(2f);

            if (DI.di.dataSaver.InGamePlay) {
                EventsModel.LOAD_SCENE?.Invoke(GameConstants.Scenes.GAMEPLAY, false, "Resuming Game");
            } else {
                // Load the lobby scene
                EventsModel.LOAD_SCENE?.Invoke(GameConstants.Scenes.LOBBY, false, "Loading Lobby");
            }
        }
    }
}