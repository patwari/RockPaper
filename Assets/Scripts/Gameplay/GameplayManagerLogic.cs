using UnityEngine;
using UnityEngine.UI;
using Events;
using System.Collections;
using System;
using UnityEditor.VersionControl;

namespace Gameplay {
    public partial class GameplayManager : MonoBehaviour {
        /// <summary>
        /// Returns <bool, message>.
        /// bool = telling if player won or not.
        /// message = Who defeated whom, and why
        /// </summary>
        private Tuple<bool, string> GetResult(PlayerShape playerShape, PlayerShape botShape) {
            if (playerShape == PlayerShape.ROCK) {
                switch (botShape) {
                    case PlayerShape.SCISSORS:
                        return new(true, "Rock crushes Scissors");
                    case PlayerShape.LIZARD:
                        return new(true, "Rock crushes Lizard");
                    case PlayerShape.SPOCK:
                        return new(false, "Spock vaporizes Rock");
                    case PlayerShape.PAPER:
                        return new(false, "Paper covers Rock");
                    default:
                        throw new Exception($"Invalid :: GetResult :: player = {playerShape} :: bot = {botShape}");
                }
            }
            if (playerShape == PlayerShape.PAPER) {
                switch (botShape) {
                    case PlayerShape.ROCK:
                        return new(true, "Paper covers Rock");
                    case PlayerShape.SPOCK:
                        return new(true, "Paper disproves Spock");
                    case PlayerShape.LIZARD:
                        return new(false, "Lizard eats Paper");
                    case PlayerShape.SCISSORS:
                        return new(false, "Scissors cuts Paper");
                    default:
                        throw new Exception($"Invalid :: GetResult :: player = {playerShape} :: bot = {botShape}");
                }
            }
            if (playerShape == PlayerShape.SCISSORS) {
                switch (botShape) {
                    case PlayerShape.PAPER:
                        return new(true, "Scissors cuts Paper");
                    case PlayerShape.LIZARD:
                        return new(true, "Scissors decapitates Lizard");
                    case PlayerShape.ROCK:
                        return new(false, "Rock crushes Scissors");
                    case PlayerShape.SPOCK:
                        return new(false, "Spock smashes Scissors");
                    default:
                        throw new Exception($"Invalid :: GetResult :: player = {playerShape} :: bot = {botShape}");
                }
            }

            if (playerShape == PlayerShape.LIZARD) {
                switch (botShape) {
                    case PlayerShape.SPOCK:
                        return new(true, "Lizard poisons Spock");
                    case PlayerShape.PAPER:
                        return new(true, "Lizard eats Paper");
                    case PlayerShape.ROCK:
                        return new(false, "Rock crushes Lizard");
                    case PlayerShape.SCISSORS:
                        return new(false, "Scissors decapitates Lizard");
                    default:
                        throw new Exception($"Invalid :: GetResult :: player = {playerShape} :: bot = {botShape}");
                }
            }


            if (playerShape == PlayerShape.SPOCK) {
                switch (botShape) {
                    case PlayerShape.SCISSORS:
                        return new(true, "Spock smashes Scissors");
                    case PlayerShape.ROCK:
                        return new(true, "Spock vaporizes Rock");
                    case PlayerShape.LIZARD:
                        return new(false, "Lizard poisons Spock");
                    case PlayerShape.PAPER:
                        return new(false, "Paper disproves Spock");
                    default:
                        throw new Exception($"Invalid :: GetResult :: player = {playerShape} :: bot = {botShape}");
                }
            }

            throw new Exception($"Invalid :: GetResult :: player = {playerShape} :: bot = {botShape}");
        }
    }
}