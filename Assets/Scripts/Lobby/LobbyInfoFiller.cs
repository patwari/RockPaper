using UnityEngine;
using TMPro;
using System;
using Events;

namespace Lobby {
    public class LobbyInfoFiller : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI infoText;

        private void Awake() {
            UpdateInfoText();
            EventsModel.DATA_CLEARED += UpdateInfoText;
        }

        private void OnDestroy() {
            EventsModel.DATA_CLEARED -= UpdateInfoText;
        }

        private void UpdateInfoText() {
            int winRate = DI.di.dataSaver.totalRounds == 0 ? 0 : (int)(DI.di.dataSaver.roundsWon * 100 / DI.di.dataSaver.totalRounds);
            infoText.text = $"Win Rate = {DI.di.dataSaver.roundsWon} / {DI.di.dataSaver.totalRounds} = {winRate}%\n" +
                            $"Rounds Won = {DI.di.dataSaver.roundsWon}\n" +
                            $"Rounds Tied = {DI.di.dataSaver.roundsTied}\n" +
                            $"Rounds Lost = {DI.di.dataSaver.roundsLost}\n" +
                            $"Rounds Lost Timeout = {DI.di.dataSaver.roundsLostNoMove}\n" +
                            $"Max Streak = {DI.di.dataSaver.maxStreak}\n";
        }
    }
}