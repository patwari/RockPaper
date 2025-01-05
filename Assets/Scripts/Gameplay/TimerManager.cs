using System;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public class TimerManager : MonoBehaviour {
        [SerializeField] private Image loadingBar;
        [SerializeField] private GameObject view;
        [SerializeField] private TextMeshProUGUI percText;
        [SerializeField] private TextMeshProUGUI message;

        private float timeRemain = 0f;
        private bool timerRunning = false;

        private void Awake() {
            if (GameDI.di.timerManager != null) {
                throw new System.Exception("TimerManager already exists");
            }
            GameDI.di.SetTimerManager(this);
            view.SetActive(false);
            // message.text = "Time Remaining";
        }

        public void ReadyForNewRound() {
            view.SetActive(true);
            timeRemain = GameConstants.GameplayConstants.PER_ROUND_TIME;
            FillLoader(timeRemain);
        }

        public void StartCountdownTimer() {
            timerRunning = true;
        }

        private void Update() {
            if (!timerRunning) return;

            timeRemain -= Time.deltaTime;
            FillLoader(timeRemain);
            if (timeRemain <= 0f) {
                timerRunning = false;
                GameplayEvents.ROUND_TIME_END?.Invoke();
            }
        }

        /// <summary>
        /// It should be a/c to seconds remain
        /// </summary>
        /// <param name="perc"></param>
        private void FillLoader(float secRemain) {
            float perc = secRemain / GameConstants.GameplayConstants.PER_ROUND_TIME;
            loadingBar.fillAmount = Mathf.Clamp(perc, 0f, 1f);
            percText.text = $"{(Math.Round(secRemain * 10) / 10):F1} sec";          // show 1 decimal place
        }
    }
}