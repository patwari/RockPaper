using UnityEngine;
using UnityEngine.UI;
using Events;

namespace Gameplay {
    public class LobbyManager : MonoBehaviour {
        [SerializeField] private Button playButton;

        private void Awake() {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void Start() {
            DI.di.soundManager.PlayBgm("lobby");
        }

        private void OnDestroy() {
            DI.di.soundManager.StopBgm();
        }

        private void OnPlayButtonClicked() {
            Debug.Log("LobbyManager :: OnPlayButtonClicked");
            playButton.interactable = false;

            DI.di.dataSaver.InGamePlay = true;
            DI.di.dataSaver.currRoundNumber = 1;
            DI.di.dataSaver.currStreak = 0;

            DI.di.soundManager.PlayDefaultButtonClick();
            EventsModel.LOAD_SCENE?.Invoke(GameConstants.Scenes.GAMEPLAY, false, "Loading Gameplay...");
        }
    }
}