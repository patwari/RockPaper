using System.Collections;
using UnityEngine;
using Events;
using GameConstants;

namespace Utils {

    public class GoToLobby : MonoBehaviour {
        // Start is called before the first frame update
        private void Start() {
            // Load the lobby scene
            StartCoroutine(LoadLobby());
        }

        private IEnumerator LoadLobby() {
            yield return new WaitForSeconds(2f);
            // Load the lobby scene
            EventsModel.LOAD_SCENE?.Invoke(Scenes.LOBBY, false, "Loading Lobby");
        }
    }
}