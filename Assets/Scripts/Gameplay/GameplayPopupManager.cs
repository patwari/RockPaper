using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using System.Collections;

namespace Utils {
    public class GameplayPopupManager : MonoBehaviour {
        [SerializeField] private Image view;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private float inTime = 0.2f;
        [SerializeField] private float outTime = 0.2f;

        private void Awake() {
            view.gameObject.SetActive(false);
            Events.GameplayEvents.SHOW_POPUP += ShowPopup;
        }

        private void OnDestroy() {
            Events.GameplayEvents.SHOW_POPUP -= ShowPopup;
        }

        private void ShowPopup(string message, float duration, UnityAction midCallback, UnityAction completeCallback) {
            view.DOKill(false);
            view.gameObject.SetActive(true);
            view.DOFade(1, inTime);
            messageText.text = message;

            float midTime = duration / 2f;
            StartCoroutine(WaitAndDo(midTime, midCallback));

            view.DOFade(0, outTime).SetDelay(duration).OnComplete(() => {
                view.gameObject.SetActive(false);
                completeCallback?.Invoke();
            });

            // StartCoroutine(WaitAndDo(duration, completeCallback));
        }

        private IEnumerator WaitAndDo(float delay, UnityAction callback) {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
    }
}