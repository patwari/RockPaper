using System.Collections;
using TMPro;
using Events;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Gameplay {
    public class BotManager : MonoBehaviour {
        internal BotState currState { get; private set; } = BotState.INIT;
        internal string name { get; private set; } = "Bot";

        [SerializeField] private GameObject view;
        [SerializeField] private GameObject thinkingView;       // contains bot is thinking view
        [SerializeField] private GameObject shapeView;          // containing bot's shape

        [SerializeField] private ShapeViewManager botShape;
        [SerializeField] private TextMeshProUGUI botNameText;
        [SerializeField] private GameObject shapePosWhileThinkingRef;
        [SerializeField] private GameObject shapePosAfterThinkingRef;

        private PlayerShape bShape = PlayerShape.UNDEFINED;

        private Coroutine thinkingCoroutine;

        private static PlayerShape[] validShapes = new PlayerShape[] { PlayerShape.ROCK, PlayerShape.PAPER, PlayerShape.SCISSORS, PlayerShape.LIZARD, PlayerShape.SPOCK };

        private void Awake() {
            if (GameDI.di.botManager != null) {
                throw new System.Exception("BotManager already exists");
            }
            name = GetNewName();
            botNameText.text = name;
            GameDI.di.SetBotManager(this);

            view.SetActive(false);
            thinkingView.SetActive(false);
            shapeView.SetActive(false);
        }

        public void ReadyForNewRound() {
            view.SetActive(true);
            currState = BotState.INIT;
            bShape = PlayerShape.UNDEFINED;
            thinkingView.SetActive(false);
            shapeView.SetActive(false);
            if (thinkingCoroutine != null) StopCoroutine(thinkingCoroutine);
            thinkingCoroutine = null;
            shapeView.transform.position = shapePosWhileThinkingRef.transform.position;
        }

        public void StartThinking() {
            currState = BotState.THINKING;
            StartCoroutine(WaitAndMakeBotMove());
        }

        private IEnumerator WaitAndMakeBotMove() {
            thinkingView.SetActive(true);
            shapeView.SetActive(true);
            float delay = Random.Range(0.5f, GameConstants.GameplayConstants.PER_ROUND_TIME - 0.5f);
            thinkingCoroutine = StartCoroutine(DoThinkingAnim());
            yield return new WaitForSeconds(delay);
            if (thinkingCoroutine != null) StopCoroutine(thinkingCoroutine);
            thinkingCoroutine = null;

            thinkingView.SetActive(false);
            MakeBotMove();
        }

        private IEnumerator DoThinkingAnim() {
            // change shape every 50 ms.
            while (true) {
                List<PlayerShape> options = new();
                foreach (PlayerShape s in validShapes) {
                    if (s != botShape.currShape) {
                        options.Add(s);
                    }
                }
                PlayerShape next = Utils.ArrayUtils.GetRandomItem(options);
                botShape.SetShape(next);
                yield return new WaitForSeconds(0.05f);
            }
        }

        private string GetNewName() => Utils.ArrayUtils.GetRandomItem(new string[] { "Marcus", "Santa", "Barbie", "Datsun" });

        public void MakeBotMove() {
            bShape = Utils.ArrayUtils.GetRandomItem(validShapes);
            botShape.SetShape(bShape);
            currState = BotState.MOVED;
            shapeView.transform.DOMove(shapePosAfterThinkingRef.transform.position, 0.4f).SetEase(Ease.OutBounce);
            GameplayEvents.BOT_MOVE_MADE?.Invoke(bShape);
        }

        internal enum BotState {
            INIT,               // Initial state
            THINKING,           // bot is thinking
            MOVED,              // bot has moved
            OVER                // round is over
        }
    }
}