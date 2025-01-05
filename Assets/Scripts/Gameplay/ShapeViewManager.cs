using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Gameplay {
    public class ShapeViewManager : MonoBehaviour {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Sprite mysterySprite;

        private ShapeImageConfig shapeConfig => GameDI.di.shapeImageConfig;
        public PlayerShape currShape { get; private set; } = PlayerShape.UNDEFINED;


        internal void SetShape(PlayerShape shape) {
            currShape = shape;
            switch (shape) {
                case PlayerShape.ROCK:
                    icon.sprite = shapeConfig.rock;
                    label.text = "Rock";
                    break;
                case PlayerShape.PAPER:
                    icon.sprite = shapeConfig.paper;
                    label.text = "Paper";
                    break;
                case PlayerShape.SCISSORS:
                    icon.sprite = shapeConfig.scissors;
                    label.text = "Scissors";
                    break;
                case PlayerShape.LIZARD:
                    icon.sprite = shapeConfig.lizard;
                    label.text = "Lizard";
                    break;
                case PlayerShape.SPOCK:
                    icon.sprite = shapeConfig.spock;
                    label.text = "Spock";
                    break;
                default:
                    Debug.LogError($"ShapeViewManager :: Shape {shape} not found in ShapeImageConfig");
                    icon.sprite = null;
                    label.text = "Undefined";
                    break;
            }
        }

        /// <summary>
        /// For a scenario where the shape has been defined. But we do not want to show it.
        /// </summary>
        internal void EnableMystery() {
            icon.sprite = mysterySprite;
            label.text = "???";
        }

        internal void RemoveMystery() {
            SetShape(currShape);
            icon.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }
    }
}