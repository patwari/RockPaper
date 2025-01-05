using UnityEngine;
namespace Utils {
    public class DataSaver : MonoBehaviour {
        private void Awake() {
            DI.di.SetDataSaver(this);
        }

        /// <summary>
        /// Total rounds won in entire lifetime
        /// </summary>
        public int roundsWon {
            get => PlayerPrefs.GetInt("roundsWon", 0);
            set => PlayerPrefs.SetInt("roundsWon", value);
        }

        /// <summary>
        /// Total rounds lost in entire lifetime
        /// </summary>
        public int roundsLost {
            get => PlayerPrefs.GetInt("roundsLost", 0);
            set => PlayerPrefs.SetInt("roundsLost", value);
        }

        /// <summary>
        /// Total rounds lost when player didn't make a move in entire lifetime
        /// </summary>
        public int roundsLostNoMove {
            get => PlayerPrefs.GetInt("roundsLostNoMove", 0);
            set => PlayerPrefs.SetInt("roundsLostNoMove", value);
        }

        /// <summary>
        /// Total rounds tied in entire lifetime
        /// </summary>
        public int roundsTied {
            get => PlayerPrefs.GetInt("roundsTied", 0);
            set => PlayerPrefs.SetInt("roundsTied", value);
        }

        /// <summary>
        /// Total rounds played in entire lifetime
        /// </summary>
        public int totalRounds {
            get => PlayerPrefs.GetInt("totalRounds", 0);
            set => PlayerPrefs.SetInt("totalRounds", value);
        }

        /// <summary>
        /// Max non-lose streak in entire lifetime
        /// </summary>
        public int maxStreak {
            get => PlayerPrefs.GetInt("maxStreak", 0);
            set => PlayerPrefs.SetInt("maxStreak", value);
        }

        /// <summary>
        /// Current non-lose streak until this streak ends
        /// </summary>
        public int currStreak {
            get => PlayerPrefs.GetInt("currStreak", 0);
            set => PlayerPrefs.SetInt("currStreak", value);
        }

        /// <summary>
        /// Current round number in the current streak
        /// </summary>
        public int currRoundNumber {
            get => PlayerPrefs.GetInt("currRoundNumber", 1);
            set => PlayerPrefs.SetInt("currRoundNumber", value);
        }

        /// <summary>
        /// A bool telling if player is in gameplay or not.
        /// </summary>
        public bool InGamePlay {
            get => PlayerPrefs.GetInt("InGamePlay", 0) == 1;
            set => PlayerPrefs.SetInt("InGamePlay", value ? 1 : 0);
        }

        public bool AllowExitOnLose {
            get => PlayerPrefs.GetInt("AllowExitOnLose", 1) == 1;
            set => PlayerPrefs.SetInt("AllowExitOnLose", value ? 1 : 0);
        }

        public bool CanPlayBgm {
            get => PlayerPrefs.GetInt("CanPlayBgm", 1) == 1;
            set => PlayerPrefs.SetInt("CanPlayBgm", value ? 1 : 0);
        }
    }
}