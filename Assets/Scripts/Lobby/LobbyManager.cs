using UnityEngine;
using UnityEngine.UI;
using Events;

namespace Gameplay {
    public class LobbyManager : MonoBehaviour {
        [SerializeField] private Button playButton;

        private void Awake() {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked() {
            Debug.Log("LobbyManager :: OnPlayButtonClicked");
            EventsModel.LOAD_SCENE?.Invoke("Gameplay", false, "Loading Gameplay...");
        }
    }
}