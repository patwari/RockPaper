using UnityEngine;
using UnityEngine.UI;
using Events;

namespace Gameplay {
    public class GameplayManager : MonoBehaviour {
        [SerializeField] private Button exitButton;

        private void Awake() {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked() {
            Debug.Log("GameplayManager :: OnExitButtonClicked");
            EventsModel.LOAD_SCENE?.Invoke("Lobby", false, "Loading Lobby...");
        }
    }
}