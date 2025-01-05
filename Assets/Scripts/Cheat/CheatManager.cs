using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public class CheatManager : MonoBehaviour {
        [SerializeField] private Button triggerButton;
        [SerializeField] private GameObject view;

        [SerializeField] private Button exitOnLoseButton;
        [SerializeField] private TextMeshProUGUI exitOnLoseText;
        [SerializeField] private Button canPlayBgmButton;
        [SerializeField] private TextMeshProUGUI canPlayBgmText;
        [SerializeField] private Button clearDataButton;

        private void Awake() {
            triggerButton.onClick.AddListener(OnTriggerButtonClicked);
            triggerButton.gameObject.SetActive(true);
            view.SetActive(false);

            exitOnLoseButton.onClick.AddListener(OnExitOnLoseClicked);
            exitOnLoseText.text = "Exit On Lose = " + (DI.di.dataSaver.AllowExitOnLose ? "YES" : "NO");
            canPlayBgmButton.onClick.AddListener(OnCanPlayBgmClicked);
            canPlayBgmText.text = "BGM = " + (DI.di.dataSaver.CanPlayBgm ? "ON" : "OFF");

            clearDataButton.onClick.AddListener(OnClearDataClicked);
        }

        private void OnCanPlayBgmClicked() {
            bool prvCanPlayBgm = DI.di.dataSaver.CanPlayBgm;
            DI.di.dataSaver.CanPlayBgm = !prvCanPlayBgm;
            canPlayBgmText.text = "BGM = " + (DI.di.dataSaver.CanPlayBgm ? "ON" : "OFF");
            Events.EventsModel.ON_BGM_STATE_CHANGED?.Invoke();
        }

        private void OnTriggerButtonClicked() {
            view.SetActive(!view.activeSelf);
        }

        private void OnExitOnLoseClicked() {
            bool prvExit = DI.di.dataSaver.AllowExitOnLose;
            DI.di.dataSaver.AllowExitOnLose = !prvExit;
            exitOnLoseText.text = "Exit On Lose = " + (DI.di.dataSaver.AllowExitOnLose ? "YES" : "NO");
        }

        private void OnClearDataClicked() {
            PlayerPrefs.DeleteAll();
            Events.EventsModel.DATA_CLEARED?.Invoke();
        }
    }
}