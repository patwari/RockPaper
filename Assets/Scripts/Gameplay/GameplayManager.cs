using UnityEngine;
using UnityEngine.UI;
using Events;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Gameplay {
    public partial class GameplayManager : MonoBehaviour {
        [SerializeField] private Button exitButton;

        [SerializeField] private Button rockButton;
        [SerializeField] private Button paperButton;
        [SerializeField] private Button scissorsButton;
        [SerializeField] private Button lizardButton;
        [SerializeField] private Button spockButton;
        private List<Button> allOptionButtons = new List<Button>();

        private PlayerShape currShape = PlayerShape.UNDEFINED;
        private PlayerShape botShape = PlayerShape.UNDEFINED;
        private bool hasPlayerMoved => currShape != PlayerShape.UNDEFINED;
        private bool hasBotMoved => botShape != PlayerShape.UNDEFINED;

        private State currState = State.INIT;
        private RoundResult currRoundResult = RoundResult.UNDEFINED;
        private string roundResultReason = "";
        private int roundNum = 0;

        private void Awake() {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            rockButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.ROCK, rockButton));
            paperButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.PAPER, paperButton));
            scissorsButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.SCISSORS, scissorsButton));
            lizardButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.LIZARD, lizardButton));
            spockButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.SPOCK, spockButton));
            allOptionButtons = new List<Button>() { rockButton, paperButton, scissorsButton, lizardButton, spockButton };

            GameplayEvents.BOT_MOVE_MADE += OnBotMoveMade;
        }

        private void OnDestroy() {
            GameplayEvents.BOT_MOVE_MADE -= OnBotMoveMade;
        }

        private void OnBotMoveMade(PlayerShape botShape) {
            this.botShape = botShape;
        }

        private void Start() {
            StartCoroutine(WaitAndStartRound());
        }

        private void OnExitButtonClicked() {
            Debug.Log("GameplayManager :: OnExitButtonClicked");
            EventsModel.LOAD_SCENE?.Invoke("Lobby", false, "Loading Lobby...");
        }

        private void OnShapeClicked(PlayerShape shape, Button button) {
            PlayerShape prev = currShape;
            Debug.Log($"GameplayManager :: OnShapeClicked :: [{prev} -> {shape}]");
            currShape = shape;

            foreach (Button item in allOptionButtons) {
                item.transform.DOKill();
                if (item == button) item.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
                else item.transform.DOScale(Vector3.one, 0.2f);
            }
        }

        private IEnumerator WaitAndStartRound() {
            GameplayEvents.SHOW_POPUP?.Invoke("NEXT ROUND", 1f, () => {
                MakeReadyForNextRound();
                GameDI.di.botManager.StartThinking();
                // StartCoroutine(WaitAndMakeBotMove());
                StartCoroutine(WaitAndEndRound());
            });
            // TODO: show a round start popup
            yield return new WaitForSeconds(1f);
        }

        // private IEnumerator WaitAndMakeBotMove() {
        //     float delay = UnityEngine.Random.Range(0.5f, GameConstants.GameplayConstants.ROUND_TIME - 0.5f);
        //     yield return new WaitForSeconds(delay);
        //     botShape = GameDI.di.botManager.MakeBotMove();
        // }

        private void MakeReadyForNextRound() {
            // TODO: show a popup => to get ready
            currShape = PlayerShape.UNDEFINED;
            botShape = PlayerShape.UNDEFINED;
            currState = State.WAITING_FOR_ROUND_START;
            roundNum++;
            GameDI.di.botManager.ReadyForNewRound();
            foreach (Button item in allOptionButtons) {
                item.transform.DOKill();
                item.transform.DOScale(Vector3.one, 0.2f);
            }
        }

        private IEnumerator WaitAndEndRound() {
            yield return new WaitForSeconds(GameConstants.GameplayConstants.PER_ROUND_TIME);
            OnTimeEnd();
        }

        private void OnTimeEnd() {
            currState = State.ROUND_OVER;
            StopAllCoroutines();

            if (!hasPlayerMoved) {
                currRoundResult = RoundResult.PLAYER_LOST_NO_MOVE;
                roundResultReason = "You did not make a move";
            } else if (currShape == botShape) {
                currRoundResult = RoundResult.TIE;
                roundResultReason = "Same Shape";
            } else {
                var result = GetResult(currShape, botShape);
                currRoundResult = result.Item1 ? RoundResult.PLAYER_WON : RoundResult.PLAYER_LOST;
                roundResultReason = result.Item2;
            }

            if (currRoundResult == RoundResult.PLAYER_LOST_NO_MOVE || currRoundResult == RoundResult.PLAYER_LOST) {
                // GameplayEvents.SHOW_POPUP?.Invoke("Game Over", 1f, () => {
                //     EventsModel.LOAD_SCENE?.Invoke("Lobby", false, "Loading Lobby...");
                // });
                // return;
                GameplayEvents.SHOW_POPUP?.Invoke($"Why??\n{roundResultReason}", 1f, () => {
                    StartCoroutine(WaitAndStartRound());
                });
                return;
            }

            if (currRoundResult == RoundResult.TIE) {
                GameplayEvents.SHOW_POPUP?.Invoke($"TIE\n{roundResultReason}", 1f, () => {
                    StartCoroutine(WaitAndStartRound());
                });
                return;
            }

            // means player won
            GameplayEvents.SHOW_POPUP?.Invoke($"You Won\n{roundResultReason}", 1f, () => {
                StartCoroutine(WaitAndStartRound());
            });
        }

        private enum State {
            INIT,                           // just initial
            WAITING_FOR_ROUND_START,        // waiting for next round to start. Currently showing NEXT ROUND popup.
            ROUND_ACTIVE,                   // gameplay active
            ROUND_OVER,                     // game has ended. Showing results. Wait for next round will start after this.
        }

        private enum RoundResult {
            UNDEFINED,
            PLAYER_WON,                     // player won the round
            PLAYER_LOST,                    // player lost the round
            PLAYER_LOST_NO_MOVE,            // player didn't make a move
            TIE                             // both bot and player made the same shape
        }
    }
}