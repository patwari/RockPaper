using UnityEngine;
using UnityEngine.UI;
using Events;

namespace Gameplay {
    public class GameplayManager : MonoBehaviour {
        [SerializeField] private Button exitButton;

        [SerializeField] private Button rockButton;
        [SerializeField] private Button paperButton;
        [SerializeField] private Button scissorsButton;
        [SerializeField] private Button lizardButton;
        [SerializeField] private Button spockButton;

        private PlayerShape currShape = PlayerShape.UNDEFINED;

        private void Awake() {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void Start() {

        }

        private void OnExitButtonClicked() {
            Debug.Log("GameplayManager :: OnExitButtonClicked");
            EventsModel.LOAD_SCENE?.Invoke("Lobby", false, "Loading Lobby...");
        }

        private void OnShapeClicked(PlayerShape shape) {
            PlayerShape prev = currShape;
            Debug.Log($"GameplayManager :: OnShapeClicked :: [{prev} -> {shape}]");
            currShape = shape;

        }
    }
}