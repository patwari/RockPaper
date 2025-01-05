using UnityEngine;
using UnityEngine.UI;
using Events;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

namespace Gameplay {
    public partial class GameplayManager : MonoBehaviour {
        [SerializeField] private Button exitButton;
        [SerializeField] private TextMeshProUGUI infoText;

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

        private void Awake() {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            rockButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.ROCK, rockButton));
            paperButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.PAPER, paperButton));
            scissorsButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.SCISSORS, scissorsButton));
            lizardButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.LIZARD, lizardButton));
            spockButton.onClick.AddListener(() => OnShapeClicked(PlayerShape.SPOCK, spockButton));
            allOptionButtons = new List<Button>() { rockButton, paperButton, scissorsButton, lizardButton, spockButton };

            GameplayEvents.BOT_MOVE_MADE += OnBotMoveMade;
            GameplayEvents.ROUND_TIME_END += OnRoundTimeEnd;
            UpdateInfoText();
        }

        private void OnDestroy() {
            GameplayEvents.BOT_MOVE_MADE -= OnBotMoveMade;
            GameplayEvents.ROUND_TIME_END -= OnRoundTimeEnd;
        }

        private void OnBotMoveMade(PlayerShape botShape) {
            this.botShape = botShape;
        }

        private void Start() {
            StartNextRound();
        }

        private void OnExitButtonClicked() {
            DI.di.dataSaver.InGamePlay = false;
            DI.di.dataSaver.currRoundNumber = 0;
            DI.di.dataSaver.currStreak = 0;
            Debug.Log("GameplayManager :: OnExitButtonClicked");
            EventsModel.LOAD_SCENE?.Invoke(GameConstants.Scenes.LOBBY, false, "Loading Lobby...");
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

        private void StartNextRound() {
            GameplayEvents.SHOW_POPUP?.Invoke("NEXT ROUND", 1f, () => {
                MakeReadyForNextRound();
                GameDI.di.botManager.ReadyForNewRound();
                GameDI.di.timerManager.ReadyForNewRound();
            }, () => {
                GameDI.di.botManager.StartThinking();
                GameDI.di.timerManager.StartCountdownTimer();
            });
        }

        private void MakeReadyForNextRound() {
            currShape = PlayerShape.UNDEFINED;
            botShape = PlayerShape.UNDEFINED;
            currState = State.WAITING_FOR_ROUND_START;
            DI.di.dataSaver.currRoundNumber++;
            foreach (Button b in allOptionButtons) {
                b.transform.DOKill();
                b.transform.DOScale(Vector3.one, 0.2f);
                b.interactable = true;
            }
        }

        private void OnRoundTimeEnd() {
            currState = State.ROUND_END_SHOWING_RESULT;
            StopAllCoroutines();

            StartCoroutine(BeginRoundEndSequence());
        }

        private IEnumerator BeginRoundEndSequence() {
            foreach (Button b in allOptionButtons) {
                b.interactable = false;
            }
            GameDI.di.botManager.RevealOnRoundEnd();

            // 1 second for the player to absorb the output.
            yield return new WaitForSeconds(1f);
            currState = State.ROUND_OVER;

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

            SaveRoundData();

            if (currRoundResult == RoundResult.PLAYER_LOST_NO_MOVE || currRoundResult == RoundResult.PLAYER_LOST) {
                // GameplayEvents.SHOW_POPUP?.Invoke("Game Over", 1f, null, () => {
                //     OnExitButtonClicked();
                // });
                GameplayEvents.SHOW_POPUP?.Invoke($"Why??\n{roundResultReason}", 2f, null, () => {
                    StartNextRound();
                });
            }

            if (currRoundResult == RoundResult.PLAYER_LOST) {
                GameplayEvents.SHOW_POPUP?.Invoke($"You Lost\n{roundResultReason}", 2f, null, () => {
                    StartNextRound();
                });
            } else if (currRoundResult == RoundResult.TIE) {
                GameplayEvents.SHOW_POPUP?.Invoke($"Tie\n{roundResultReason}", 2f, null, () => {
                    StartNextRound();
                });
            } else {
                // means player won
                GameplayEvents.SHOW_POPUP?.Invoke($"You Won\n{roundResultReason}", 2f, null, () => {
                    StartNextRound();
                });
            }
        }

        private void SaveRoundData() {
            DI.di.dataSaver.totalRounds++;
            switch (currRoundResult) {
                case RoundResult.PLAYER_WON:
                    DI.di.dataSaver.roundsWon++;
                    DI.di.dataSaver.currStreak++;
                    DI.di.dataSaver.maxStreak = Mathf.Max(DI.di.dataSaver.maxStreak, DI.di.dataSaver.currStreak);
                    break;
                case RoundResult.PLAYER_LOST:
                    DI.di.dataSaver.roundsLost++;
                    break;
                case RoundResult.PLAYER_LOST_NO_MOVE:
                    DI.di.dataSaver.roundsLost++;
                    break;
                case RoundResult.TIE:
                    DI.di.dataSaver.roundsTied++;
                    DI.di.dataSaver.currStreak++;
                    DI.di.dataSaver.maxStreak = Mathf.Max(DI.di.dataSaver.maxStreak, DI.di.dataSaver.currStreak);
                    break;
                default:
                    throw new Exception("Invalid Round Result");
                    break;
            }
            UpdateInfoText();
        }

        private void UpdateInfoText() {
            infoText.text = $"Round = {DI.di.dataSaver.currRoundNumber}\n" +
                            $"Max Streak = {DI.di.dataSaver.maxStreak}";
        }

        private enum State {
            INIT,                           // just initial
            WAITING_FOR_ROUND_START,        // waiting for next round to start. Currently showing NEXT ROUND popup.
            ROUND_ACTIVE,                   // gameplay active
            ROUND_END_SHOWING_RESULT,       // round has ended. In End sequence.
            ROUND_OVER,                     // round end sequence is completed. Wait for next round will start after this.
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